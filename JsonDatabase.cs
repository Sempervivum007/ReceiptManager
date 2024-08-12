using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ReceiptManager
{
    public class JsonDatabase
    {
        private readonly string _filePath;

        public JsonDatabase(string filePath)
        {
            _filePath = filePath;
        }

        public List<Receipt> ReadReceipts()
        {
            if (!File.Exists(_filePath))
                return new List<Receipt>();

            var json = File.ReadAllText(_filePath);

            return JsonConvert.DeserializeObject<List<Receipt>>(json) ?? new List<Receipt>();
        }

        public void SaveReceipts(List<Receipt> receipts)
        {
            var json = JsonConvert.SerializeObject(receipts);
            File.WriteAllText(_filePath, json);
        }

        public void AddReceipt(Receipt newReceipt)
        {
            var receipts = ReadReceipts();
            receipts.Add(newReceipt);
            SaveReceipts(receipts);
        }
    }
}