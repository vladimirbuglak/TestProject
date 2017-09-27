using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class Shop
    {
        private IService Service { get; set; }
        readonly object synchRoot = new object();
        private Timer _restoreTimer;
        public Shop(IService service)
        {
            Service = service;
        }

        public async Task Perfom()
        {
            Monitor.Enter(synchRoot);
            try
            {
                await Service.SubmitFeed();
                Monitor.Exit(synchRoot);
            }
            catch (Exception ex)
            {
                _restoreTimer = new Timer(Restored, null, 5000, -1);
            }
        }

        private void Restored(object state)
        {
            Monitor.Exit(synchRoot);
        }
    }
}
