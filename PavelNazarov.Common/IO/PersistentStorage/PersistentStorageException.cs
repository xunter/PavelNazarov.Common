using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PavelNazarov.Common.IO.PersistentStorage
{
    public class PersistentStorageException : ApplicationException
    {
        public PersistentStorageException(string message)
            : base(message)
        { }

        public PersistentStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}