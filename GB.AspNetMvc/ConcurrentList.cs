namespace GB.AspNetMvc
{
    public class ConcurrentList<T>
    {
        private readonly List<T> _list = new();
        private readonly ReaderWriterLockSlim _locker = new();
        public void Add(T item)
        {
            try
            {
                _locker.EnterWriteLock();
                _list.Add(item);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public void Clear()
        {
            try
            {
                _locker.EnterWriteLock();
                _list.Clear();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            try
            {
                _locker.EnterReadLock();
                return _list.Contains(item);
            }
            finally
            {
                _locker.ExitReadLock();
            }

        }

        public bool Remove(T item)
        {
            try
            {
                _locker.EnterWriteLock();
                return _list.Remove(item);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public int Count()
        {
            try
            {
                _locker.EnterReadLock();
                return _list.Count;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public void RemoveAt(int index)
        {
            try
            {
                _locker.EnterWriteLock();
                _list.RemoveAt(index);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public void Sort()
        {
            try
            {
                _locker.EnterWriteLock();
                _list.Sort();
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }
    }
}
