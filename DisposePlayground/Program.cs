using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DisposePlayground
{

    class Program
    {
        private static readonly StringBuilder s_builder = new StringBuilder();

        /// <summary>
        /// Attaches two event handlers to same event. Handlers operate on same disposable object asynchronously.
        /// Object is only disposed when both event handlers dispose of object.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Busy...");
            var listenToMe = new ListenToMe();
            listenToMe.OnObjectReady += OnObjectReady;
            listenToMe.OnObjectReady += OnObjectReady2;
            listenToMe.Start(); // raises OnObjectReady with disposable object in 1500 ms
            Thread.Sleep(2000);
            Console.WriteLine(s_builder.ToString()); // threads can't write to console directly
            Console.ReadLine();

        }

        private static void OnObjectReady(object sender, DisposableObject.DisposableObjectEventArgs e)
        {
            DisposableObject disposable;
            using (disposable = e.OpenDisposableObject())
            {
                s_builder.AppendLine(string.Format("1-before: {0}", disposable.Value));
                Thread.Sleep(250);
            }

            s_builder.AppendLine(string.Format("1-after: {0}", disposable.Value));

            Thread.Sleep(750);

            s_builder.AppendLine(string.Format("1-late: {0}", disposable.Value));
        }

        private static void OnObjectReady2(object sender, DisposableObject.DisposableObjectEventArgs e)
        {
            DisposableObject disposable;
            using (disposable = e.OpenDisposableObject())
            {
                s_builder.AppendLine(string.Format("2-before: {0}", disposable.Value));
                Thread.Sleep(750);

                s_builder.AppendLine(string.Format("2-between: {0}", disposable.Value));
            }

            s_builder.AppendLine(string.Format("2-after: {0}", disposable.Value));


        }
    }

}
