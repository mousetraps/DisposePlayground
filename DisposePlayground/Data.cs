using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposePlayground
{
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
