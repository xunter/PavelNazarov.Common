using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PavelNazarov.Common.Collections
{
    public class ObservableCollectionWrapper<T> : ICollection<T>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private readonly ICollection<T> _original;

        public ICollection<T> OriginalCollection
        {
            get { return _original; }
        }

        public ObservableCollectionWrapper(ICollection<T> original)
        {
            _original = original;
        }

        public void Add(T item)
        {
            _original.Add(item);
            FirePropertyChanged("Count");
            FireCollectionChanged(NotifyCollectionChangedAction.Add);
        }

        public void Clear()
        {
            _original.Clear();
            FirePropertyChanged("Count");
            FireCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        public bool Contains(T item)
        {
            return _original.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _original.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _original.Count; }
        }

        public bool IsReadOnly
        {
            get { return _original.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            if (_original.Remove(item))
            {
                FirePropertyChanged("Count");
                FireCollectionChanged(NotifyCollectionChangedAction.Remove);
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _original.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_original).GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void FireCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
