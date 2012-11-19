using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common
{
    [Serializable]
    public class EventArgs<T> : EventArgs
    {
        public T Entity { get; private set; }

        public EventArgs(T entity)
        {
            Entity = entity;
        }
    }
}
