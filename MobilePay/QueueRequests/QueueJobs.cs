namespace MobilePay.QueueRequests
{
    using MobilePay.Models;
    using MobilePay.Storage;
    using System.Collections.Concurrent;

    public sealed class QueueJobs
    {
        public ConcurrentQueue<Transaction> queue = new ConcurrentQueue<Transaction>();

        private static QueueJobs instance = null;

        private QueueJobs()
        {
        }

        public static QueueJobs Instance => instance ??= new QueueJobs();

        public static void DequeueAndSave()
        {
            var transactionOut = new Transaction();
            while (Instance.queue.TryDequeue(out transactionOut))
            {
                var dataWritter = new ReadWriteData();
                dataWritter.WriteDataToFile(transactionOut);
            }
        }
    }
}
