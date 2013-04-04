using System;
using System.Diagnostics;

namespace DisposePlayground
{
    

    public class DisposableInstanceManager<T> : IDisposable where T:IDisposable
    {
        public T Value { get; private set; }

        private readonly object m_lock = new object();
        private int m_safeInstanceCount = 0;

        public DisposableInstanceManager(T value)
        {
            Value = value;
        }

        public DisposableInstanceManager<T> OpenInstance()
        {
            lock (m_lock)
            {
                m_safeInstanceCount++;
                return this;
            }
        }

        public void Dispose()
        {
            lock (m_lock)
            {

                m_safeInstanceCount--;

                if (m_safeInstanceCount <= 0)
                {
                    Debug.WriteLine("count = {0}, disposing", m_safeInstanceCount);
                    Value.Dispose();

                }
                else
                {
                    Debug.WriteLine("count = {0}, not disposing:", m_safeInstanceCount);
                }
            }

        }
    }

    public class Data : IDisposable
    {
        private string m_data = "hello there!";

        public void Dispose()
        {
            m_data = "disposed :(";
        }

        public override string ToString()
        {
            return m_data;
        }
    }
}
