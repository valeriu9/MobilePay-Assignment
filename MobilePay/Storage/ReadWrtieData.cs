using MobilePay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MobilePay.Storage
{
    public class ReadWrtieData
    {
        public void WriteDataToFile(Transaction transaction)
        {
            var data = ReadDataFromFile();
            transaction.ID = data.Count != 0 ? data.Max(t => t.ID)+1 : 1;
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
