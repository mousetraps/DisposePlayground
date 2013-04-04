using System;
using System.Diagnostics;

namespace DisposePlayground
{
    

    public abstract class ManagedDisposableObject<T> : IDisposable where T : ManagedDisposableObject<T>
    {

        private readonly object m_lock = new object();
        private int m_safeInstanceCount = 0;

        public T OpenInstance()
        {
            lock (m_lock)
            {
                m_safeInstanceCount++;
                return (T)this;
            }
        }

        private bool IsReadyToDispose()
        {
            if (m_safeInstanceCount <= 0)
            {
                Debug.WriteLine("count = {0}, ready to dispose", m_safeInstanceCount);
                return true;
            }
            
            Debug.WriteLine("count = {0}, not ready to dispose", m_safeInstanceCount);
            return false;
        }

        void IDisposable.Dispose()
        {
            lock (m_lock)
            {
                m_safeInstanceCount--;

                if (IsReadyToDispose())
                {
                    Dispose();
                }
            }

        }

        protected abstract void Dispose();

    }
}
