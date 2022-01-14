namespace MobilePay.Storage
{
    using MobilePay.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ReadWriteData
    {
        public void WriteDataToFile(Transaction transaction)
        {
            var data = ReadDataFromFile();
            transaction.ID = Guid.NewGuid();
            data.Add(transaction);

            string path = Path.Combine(Environment.CurrentDirectory, @"Storage\dataHolder.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }

        public List<Transaction> ReadDataFromFile()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Storage\dataHolder.json");

            return JsonConvert.DeserializeObject<List<Transaction>>(File.ReadAllText(path));
        }

        public List<Discounts> ReadDiscountsFromFile()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Storage\discounts.json");

            return JsonConvert.DeserializeObject<List<Discounts>>(File.ReadAllText(path));
        }
    }
}
