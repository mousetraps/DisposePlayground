using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DisposePlayground
{
    public class ListenToMe
    {
        private Timer m_timer;

        private readonly MyDisposableObject  m_managedMyDisposableInstanceEventArgs = new MyDisposableObject();

        public void Start()
        {
            m_timer = new Timer(500);
            m_timer.Elapsed += timer_Elapsed;
            m_timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_timer.Stop();
            m_timer.Elapsed -= timer_Elapsed;
            m_timer.Close();

            if (OnObjectReady != null)
            {
                var eventListeners = OnObjectReady.GetInvocationList();
                for (int index = 0; index < eventListeners.Count(); index++)
                {
                    var methodToInvoke = (EventHandler<MyDisposableObject>) eventListeners[index];
                    methodToInvoke.BeginInvoke(this, m_managedMyDisposableInstanceEventArgs, EndAsyncEvent, null);
                }
            }

        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (AsyncResult) iar;
            var invokedMethod = (EventHandler<MyDisposableObject>) ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch (Exception)
            {
                Debug.WriteLine("an event listener went kaboom!");
            }
        }

        public EventHandler<MyDisposableObject> OnObjectReady;


    }
}
