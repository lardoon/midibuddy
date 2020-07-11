using System.Collections.Generic;

namespace MidiBuddyWeb.Controllers
{
    class MidiFileResult
    {
        public string Name { get; set; }

        public string FileName { get; set; }

        public IEnumerable<MidiTrack> Tracks { get; set; }
    }
}