using CsvHelper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidiBuddyWeb.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MIdiBuddyWebTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDrumkitControllerGetDrumkits()
        {
            var controller = new DrumKitController();
            var result = controller.GetDrumkits();
        }

        [TestMethod]
        public async Task ConvertCsv()
        {
            var csvPath = @"C:\Users\sweet\Downloads\Beat Buddy Midi Drum Map - Map (1).csv";
            using (var reader = new StreamReader(csvPath))
            using (var writer = new StreamWriter("map.csv"))
            using (var csvReader = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                using(var csvWriter = new CsvHelper.CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                await csvReader.ReadAsync();
                csvReader.ReadHeader();
                csvWriter.WriteHeader<Record>();
                await csvWriter.NextRecordAsync();
                while (await csvReader.ReadAsync())
                {
                    var inRecord = csvReader.GetRecord<dynamic>() as IDictionary<string, object>;
                    var keys = inRecord.Keys;
                    var outRecord = new Record()
                    {
                        Number = Convert.ToInt32(inRecord["Number"]),
                        MidiNote = Convert.ToString(inRecord["Note"]),
                        OctaveMiddleC3 = Convert.ToInt32(inRecord["Octave (Middle C3)"]),
                        OctaveMiddleC4 = Convert.ToInt32(inRecord["Octave (Middle C4)"])
                    };
                    for(var k = 4; k < keys.Count; k++)
                    {
                        var key = keys.ElementAt(k);
                        var value = Convert.ToString(inRecord[key]);
                        // if (string.IsNullOrWhiteSpace(value))
                        //    continue;
                        var instrimentNote = GetInstrumentNote(value);
                        outRecord.Kit = key;
                        outRecord.Instrument = instrimentNote.Instrument;
                        outRecord.KitNote = instrimentNote.KitNote;
                        csvWriter.WriteRecord(outRecord);
                        await csvWriter.NextRecordAsync();
                    }
                }
            }
        }

        [TestMethod]
        public async Task ConvertJson()
        {
            var csvPath = @"C:\Users\sweet\Downloads\Beat Buddy Midi Drum Map - Map (1).csv";
            var records = new Dictionary<Tuple<int, int, int, string>, Dictionary<string, InstrumentNote>>();
            using (var reader = new StreamReader(csvPath))
            using (var writer = new StreamWriter("map.json"))
            using (var csvReader = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            
            {
                await csvReader.ReadAsync();
                csvReader.ReadHeader();
               
                while (await csvReader.ReadAsync())
                {

                    var inRecord = csvReader.GetRecord<dynamic>() as IDictionary<string, object>;
                    var keys = inRecord.Keys;
                    var number = Convert.ToInt32(inRecord["Number"]);
                    var midiNote = Convert.ToString(inRecord["Note"]);
                    var octaveMiddleC3 = Convert.ToInt32(inRecord["Octave (Middle C3)"]);
                    var octaveMiddleC4 = Convert.ToInt32(inRecord["Octave (Middle C4)"]);
                    var dictKey = Tuple.Create(number, octaveMiddleC3, octaveMiddleC4, midiNote);
                    Dictionary<string, InstrumentNote> dict;
                    if(!records.TryGetValue(dictKey, out dict))
                    {
                        dict = new Dictionary<string, InstrumentNote>();
                        records.Add(dictKey, dict);
                    }
                    
                    for (var k = 4; k < keys.Count; k++)
                    {
                        var key = keys.ElementAt(k);
                        var value = Convert.ToString(inRecord[key]);
                         if (string.IsNullOrWhiteSpace(value))
                           continue;
                        var instrimentNote = GetInstrumentNote(value);
                        dict.Add(key, instrimentNote);
                        
                    }
                }
                var output = new List<JsonRecord>();
                foreach(var record in records.Keys)
                {
                    output.Add(new JsonRecord()
                    {
                        Kits = records[record],
                        Number = record.Item1,
                        OctaveMiddleC3 = record.Item2,
                        OctaveMiddleC4 = record.Item3,
                        MidiNote = record.Item4
                    });
                }
                var json = JsonConvert.SerializeObject(output);
                writer.Write(json);
            }
        }

        public InstrumentNote GetInstrumentNote(string value)
        {
            var instrumentNote = new InstrumentNote();
            if(string.IsNullOrWhiteSpace(value))
            {
                return instrumentNote;
            }
            if (value.StartsWith("Hammond ", StringComparison.InvariantCultureIgnoreCase))
                instrumentNote.Instrument = "Hammond";
            if (value.StartsWith("Bass ", StringComparison.InvariantCultureIgnoreCase) && !value.StartsWith("Bass Drum", StringComparison.InvariantCultureIgnoreCase))
                instrumentNote.Instrument = "Bass";
            if (value.StartsWith("Organ ", StringComparison.InvariantCultureIgnoreCase))
                instrumentNote.Instrument = "Organ";
            if(string.IsNullOrWhiteSpace(instrumentNote.Instrument))
            {
                instrumentNote.Instrument = "Drums";
                instrumentNote.KitNote = value;
            } else
            {
                instrumentNote.KitNote = value.Substring(instrumentNote.Instrument.Length).Trim();
            }
            return instrumentNote;
        }

        public class JsonRecord
        {
            public int Number { get; set; }
            public string MidiNote { get; set; }
            public int OctaveMiddleC3 { get; set; }
            public int OctaveMiddleC4 { get; set; }

            public Dictionary<string, InstrumentNote> Kits { get; set; }
        }

        public class Record
        {
            public int Number { get; set; }
            public string MidiNote { get; set; }
            public int OctaveMiddleC3 { get; set; }
            public int OctaveMiddleC4 { get; set; }
            public string Kit { get; set; }
            public string Instrument { get; set; }
            public string KitNote { get; set; }
        }

        public class InstrumentNote
        {
            public string Instrument { get; set; }
            public string KitNote { get; set; }
        }

    }

}
