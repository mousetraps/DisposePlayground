using System;
using System.Diagnostics;

namespace DisposePlayground
{
    

    public class DisposableObject : IDisposable
    {
        private volatile string m_value = "hello there!";
        public string Value
        {
            get { return m_value; }
            private set { m_value = value; }
        }

        private readonly object m_lock = new object();
        private int m_safeInstanceCount = 0;

        protected DisposableObject OpenData()
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
                    m_value = "disposed :(";

                }
                else
                {
                    Debug.WriteLine("count = {0}, not disposing:", m_safeInstanceCount);
                }
            }

        }


        public class DisposableObjectEventArgs : EventArgs
        {
            private readonly DisposableObject m_disposableObject;

            public DisposableObjectEventArgs(DisposableObject disposableObject)
            {
                m_disposableObject = disposableObject;
            }

            public DisposableObject OpenDisposableObject()
            {
                return m_disposableObject.OpenData();
            }
        }
    }
}
