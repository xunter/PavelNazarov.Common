using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common
{
    public sealed class Pair<TFirst, TSecond>
    {
        private readonly TFirst _first;
        private readonly TSecond _second;

        public TFirst First
        {
            get { return _first; }
        }

        public TSecond Second
        {
            get { return _second; }
        }

        public Pair(TFirst first, TSecond second)
        {
            _first = first;
            _second = second;
        }
    }
}
