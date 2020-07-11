using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MidiBuddyWeb.Controllers
{
    internal class NoteEqualityComparer : IEqualityComparer<Note>
    {
        public bool Equals([AllowNull] Note x, [AllowNull] Note y)
        {
            if (x == null && y != null || x != null && y == null)
                return false;
            if (x == null)
                return true;
            return x.NoteNumber == y.NoteNumber;
        }



        public int GetHashCode([DisallowNull] Note obj)
        {
            return obj.NoteNumber;
        }

    }

}