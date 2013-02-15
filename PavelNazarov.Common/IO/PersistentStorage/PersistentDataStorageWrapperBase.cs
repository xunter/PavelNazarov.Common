using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PavelNazarov.Common.IO.PersistentStorage
{
    public abstract class PersistentDataStorageWrapperBase<T>
    {
        private DateTime _persistentFileLastModifiedDate;
        private T _storedData;
        private readonly Object _locker = new Object();

        protected abstract string FilePath { get; }

        protected T StoredData
        {
            get
            {
                if (String.IsNullOrEmpty(FilePath))
                {
                    throw new NullReferenceException("You must specify file path!");
                }
                else if (!File.Exists(FilePath))
                {
                    throw new FileNotFoundException("File does not found!", FilePath);
                }
                var lastModifiedDate = File.GetLastWriteTime(FilePath);
                lock (_locker)
                {
                    bool fileHasChanged = IsPersistentStorageModified();
                    if (_storedData == null || fileHasChanged)
                    {
                        LoadDataFromPersistentStorage(FilePath, lastModifiedDate);
                    }
                    return _storedData;
                }
            }
        }

        protected virtual bool IsPersistentStorageModified()
        {
            var lastModifiedDate = File.GetLastWriteTime(FilePath);
            bool fileHasChanged = _persistentFileLastModifiedDate == DateTime.MinValue || _persistentFileLastModifiedDate < lastModifiedDate;
            return fileHasChanged;
        }

        private void LoadDataFromPersistentStorage(string filePath, DateTime lastModifiedDate)
        {
            _storedData = CustomLoadDataFromPersistentStorage(filePath);
            _persistentFileLastModifiedDate = lastModifiedDate;
        }

        protected abstract T CustomLoadDataFromPersistentStorage(string filePath);
    }
}
