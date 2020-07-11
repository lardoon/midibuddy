using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using Melanchall.DryWetMidi.Core;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Text.Json;
using Melanchall.DryWetMidi.Common;
using System.Reflection;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System.Linq.Expressions;

namespace MidiBuddyWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MidiFileUploadController : ControllerBase
    {
        private IWebHostEnvironment _hostingEnvironment;

        public MidiFileUploadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile upload)
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions();

                using (var stream = upload.OpenReadStream())
                {
                    MidiFile midiFile;
                    try
                    {
                        midiFile = MidiFile.Read(stream);
                    }
                    catch (Exception e)
                    {
                        return new JsonResult(e);
                    }
                    var midiResult = new MidiFileResult()
                    {
                        FileName = upload.FileName,
                        Tracks = midiFile.GetTrackChunks().Where(t => t.GetNotes().Any()).Select((t, i) => new MidiTrack()
                        {
                            Channel = t.GetChannels().First() + 1,
                            Id = i + 1,
                            Name = t.Events.OfType<SequenceTrackNameEvent>().FirstOrDefault()?.Text,
                            Instrument = Convert.ToString(t.Events.OfType<ProgramChangeEvent>().FirstOrDefault()?.ProgramNumber ?? new SevenBitNumber(0)),
                            Notes = t.GetNotes().Distinct(new NoteEqualityComparer()).OrderBy(n => n.NoteNumber).Select(n => new MidiNote()
                            {
                                Name = Enum.GetName(typeof(NoteName), n.NoteName),
                                Number = n.NoteNumber,
                                Octave = n.Octave,
                                Instrument = Convert.ToString(t.Events.OfType<ProgramChangeEvent>().FirstOrDefault()?.ProgramNumber ?? new SevenBitNumber(0))
                            })
                        })
                    };
                    return new JsonResult(midiResult, options);
                }

            }
            catch (System.Exception ex)
            {
                return new JsonResult("Upload Failed: " + ex.Message);
            }
        }

    }

}