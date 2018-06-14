namespace ShoppingCartProject.DataAccess.Model
{
    public interface IItem
    {
        int Id { get; set; }
        decimal ItemTotal { get; }
        string Name { get; set; }
        int Quantity { get; set; }
        decimal UnitPrice { get; set; }

        bool Equals(object obj);
        int GetHashCode();
        string ToString();
    }
}