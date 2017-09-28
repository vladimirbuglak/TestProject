using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class Shop
    {
        private IService Service { get; set; }
        public readonly object synchRoot = new object();
        private Timer _restoreTimer;

        private int _taskNumber;
        public Shop(IService service)
        {
            Service = service;
        }

        public void Perfom(int taskNumber)
        {
            _taskNumber = taskNumber;
            Console.WriteLine($"Perfom for {taskNumber}");
            Monitor.Enter(synchRoot);
            try
            {
                Console.WriteLine(taskNumber);
                Service.SubmitFeed();
                Monitor.Exit(synchRoot);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Set up timer for task {taskNumber}");
                _restoreTimer = new Timer(Restored, null, 3000, -1);
            }
            Console.WriteLine($"End for {taskNumber}");
        }

        private void Restored(object state)
        {
            Console.WriteLine($"Restored for {_taskNumber}");
            Monitor.Exit(synchRoot);
        }
    }
}
