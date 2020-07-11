using System.Collections.Generic;

namespace MidiBuddyWeb.Controllers
{
    class MidiTrack
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Channel { get; set; }

        public IEnumerable<MidiNote> Notes { get; set; }
        public string Instrument { get; internal set; }
    }
}