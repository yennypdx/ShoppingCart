using Newtonsoft.Json;
using ShoppingCartProject.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShoppingCartProject.DataAccess
{
    public class ShoppingCart : IShoppingCart
    {
        private const string CartDatabase = "CartDatabase.json";

        public int Id { get; set; }
        public decimal SubTotal
        {
            get
            {
                var items = ReadFromDatabase;
                decimal tempTotal = 0m;
                foreach (var it in items)
                {
                    tempTotal += it.ItemTotal;
                }
                return tempTotal;
            }
        }

        public void AddItem(Item item)
        {
            if (item == null) throw new Exception("Item was null.");

            var items = ReadFromDatabase;
            var maxId = 0;
            if (items == null)
            {
                items = new List<Item>();
                item.Id = maxId + 1;
            }
            else
            {
                maxId = items.Max(it => it.Id);
            }

            items.Add(item);
            SaveToDatabase(items);
        }
        public void DeleteItem(Item item)
        {
            var items = ReadFromDatabase;
            var itemToDelete = items.ToList().SingleOrDefault(it => it.Id.Equals(item.Id));

            if (item == null) throw new Exception("Item not found.");
            items.Remove(itemToDelete);
            SaveToDatabase(items);
        }
        public void UpdateItem(Item item)
        {
            var items = ReadFromDatabase;
            foreach (var it in items)
            {
                if (it.Id.Equals(item.Id))
                {
                    it.Id = item.Id;
                    it.Name = item.Name;
                    it.Quantity = item.Quantity;
                    it.UnitPrice = item.UnitPrice;
                }
            }
            SaveToDatabase(items);
        }
        public IList<Item> ReadFromDatabase
        {
            get
            {
                if (!File.Exists(CartDatabase))
                {
                    throw new InvalidOperationException("File not found");
                }

                var json = File.ReadAllText(CartDatabase);
                return JsonConvert.DeserializeObject<List<Item>>(json);
            }
        }

        public void SaveToDatabase(IList<Item> items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(CartDatabase, json);
        }

        public Item GetItemById(int itemId)
        {
            var items = ReadFromDatabase;
            return items.SingleOrDefault(it => it.Id.Equals(itemId));
        }

        public IEnumerable<Item> GetAllItems()
        {
            return ReadFromDatabase;
        }

        public void Dispose()
        {
            File.WriteAllText(CartDatabase, string.Empty);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("\t\t\t------------------\n");
            builder.Append("\t\t     Sub Total: $" + SubTotal);

            return builder.ToString();
        }
    }
}
