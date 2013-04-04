using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposePlayground
{
    public class MyDisposableObject : ManagedDisposableObject<MyDisposableObject>
    {
        private string m_data = "hello there!";
        public string Data { 
            get { return m_data; }
            private set { m_data = value; } 
        }

        public override string ToString()
        {
            return Data;
        }

        protected override void Dispose()
        {
            Data = "disposed :(";
        }
    }
}
