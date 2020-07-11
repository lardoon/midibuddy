namespace MidiBuddyWeb.Controllers
{
    public class MidiNote
    {
        public int Number { get; set; }

        public int Octave { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Instrument { get; set; }

        public string InstrumentFamily { get; set; }
    }
}