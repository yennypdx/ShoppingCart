using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCartProject.DataAccess.Model
{
    public class Item : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ItemTotal
        {
            get { return Quantity * UnitPrice; }
        }

        public override bool Equals(object obj)
        {
            var item = obj as Item;
            return item != null &&
                   Id == item.Id &&
                   Name == item.Name &&
                   Quantity == item.Quantity &&
                   UnitPrice == item.UnitPrice &&
                   ItemTotal == item.ItemTotal;
        }

        public override int GetHashCode()
        {
            var hashCode = 758833526;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            hashCode = hashCode * -1521134295 + UnitPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + ItemTotal.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("ID\tName\tQty\tPrice\tItem Total\n");
            builder.Append(Id + "\t" + Name + "\t" + Quantity + "\t$" + UnitPrice + "\t$" + ItemTotal);

            return builder.ToString();
        }
    }
}
