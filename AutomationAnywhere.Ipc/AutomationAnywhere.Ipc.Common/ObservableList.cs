using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Windows.Threading;

namespace AutomationAnywhere.Ipc.Common
{
    public class ObservableList<T> : IObservableList<T>
    {
        private readonly IList<T> _collection = new List<T>();
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly ReaderWriterLock _sync = new ReaderWriterLock();

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged == null)
                return;

            foreach (var del in CollectionChanged.GetInvocationList())
            {
                var handler = del as NotifyCollectionChangedEventHandler;
                if(handler == null)
                    continue;
                // If the subscriber is a DispatcherObject and different thread.
                var dispatcherObject = handler.Target as DispatcherObject;

                if (dispatcherObject != null && !dispatcherObject.CheckAccess())
                {
                    if (args.Action == NotifyCollectionChangedAction.Reset)
                        dispatcherObject.Dispatcher.Invoke
                              (DispatcherPriority.DataBind, handler, this, args);
                    else
                        // Invoke handler in the target dispatcher's thread... 
                        // asynchronously for better responsiveness.
                        dispatcherObject.Dispatcher.BeginInvoke
                              (DispatcherPriority.DataBind, handler, this, args);
                }
                else
                {
                    // Execute handler as is.
                    handler(this, args);
                }
            }
        }

        public void Add(T item)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                _collection.Add(item);
                OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                          NotifyCollectionChangedAction.Add, item));
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public void Clear()
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                _collection.Clear();
                OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Reset));
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public bool Contains(T item)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                var result = _collection.Contains(item);
                return result;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                _collection.CopyTo(array, arrayIndex);
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public int Count
        {
            get
            {
                _sync.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    return _collection.Count;
                }
                finally
                {
                    _sync.ReleaseReaderLock();
                }
            }
        }

        public bool IsReadOnly
        {
            get { return _collection.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                var index = _collection.IndexOf(item);
                if (index == -1)
                    return false;
                var result = _collection.Remove(item);
                if (result)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
                return result;
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            _sync.AcquireReaderLock(Timeout.Infinite);
            try
            {
                var result = _collection.IndexOf(item);
                return result;
            }
            finally
            {
                _sync.ReleaseReaderLock();
            }
        }

        public void Insert(int index, T item)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                _collection.Insert(index, item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public void RemoveAt(int index)
        {
            _sync.AcquireWriterLock(Timeout.Infinite);
            try
            {
                if (_collection.Count == 0 || _collection.Count <= index)
                    return;
                var item = _collection[index];
                _collection.RemoveAt(index);
                OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                           NotifyCollectionChangedAction.Remove, item, index));
            }
            finally
            {
                _sync.ReleaseWriterLock();
            }
        }

        public T this[int index]
        {
            get
            {
                _sync.AcquireReaderLock(Timeout.Infinite);
                try
                {
                    var result = _collection[index];
                    return result;
                }
                finally
                {
                    _sync.ReleaseReaderLock();
                }
            }
            set
            {
                _sync.AcquireWriterLock(Timeout.Infinite);
                try
                {
                    if (_collection.Count == 0 || _collection.Count <= index)
                        return;
                    var item = _collection[index];
                    _collection[index] = value;
                    OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                               NotifyCollectionChangedAction.Replace, value, item, index));
                }
                finally
                {
                    _sync.ReleaseWriterLock();
                }
            }

        }
    }
}
