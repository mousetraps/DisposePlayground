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

        private DisposableObject.DisposableObjectEventArgs m_disposableObjectEventArgs =
            new DisposableObject.DisposableObjectEventArgs(new DisposableObject());

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
                    var methodToInvoke = (EventHandler<DisposableObject.DisposableObjectEventArgs>) eventListeners[index];
                    methodToInvoke.BeginInvoke(this, m_disposableObjectEventArgs, EndAsyncEvent, null);
                }
            }

        }

        private void EndAsyncEvent(IAsyncResult iar)
        {
            var ar = (AsyncResult) iar;
            var invokedMethod = (EventHandler<DisposableObject.DisposableObjectEventArgs>) ar.AsyncDelegate;

            try
            {
                invokedMethod.EndInvoke(iar);
            }
            catch (Exception)
            {
                Debug.WriteLine("an event listener went kaboom!");
            }
        }

        public EventHandler<DisposableObject.DisposableObjectEventArgs> OnObjectReady;


    }
}
