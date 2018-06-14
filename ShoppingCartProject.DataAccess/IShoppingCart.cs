using System.Collections.Generic;
using ShoppingCartProject.DataAccess.Model;

namespace ShoppingCartProject.DataAccess
{
    public interface IShoppingCart
    {
        int Id { get; set; }
        IList<Item> ReadFromDatabase { get; }
        decimal SubTotal { get; }
        void AddItem(Item item);
        void DeleteItem(Item item);
        IEnumerable<Item> GetAllItems();
        Item GetItemById(int itemId);
        void SaveToDatabase(IList<Item> items);
        void Dispose();
        void UpdateItem(Item item);

    }
}