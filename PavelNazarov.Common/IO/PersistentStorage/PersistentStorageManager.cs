using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PavelNazarov.Common.IO.PersistentStorage;

namespace PavelNazarov.Common.IO.PersistentStorage
{
    public class PersistentStorageManager
    {
        private static PersistentStorageManager _instance;

        private PersistentStorageManager()
        { }

        public static PersistentStorageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(PersistentStorageManager))
                    {
                        if (_instance == null)
                            _instance = new PersistentStorageManager();
                    }
                }
                return _instance;
            }
        }

        private IPersistentStorage _storage;

        public void Init(IPersistentStorage storage)
        {
            _storage = storage;
        }

        public IPersistentStorage PersistentStorage
        {
            get { return _storage; }
        }
    }
}