using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common.IO.PersistentStorage
{
    public interface IPersistentStorage
    {
        object Get(string key);
        void Set(string key, object objectToStore);        
    }
}
