using Autofac;
using ShoppingCartConsole.Client.Startup;
using ShoppingCartProject.DataAccess;
using ShoppingCartProject.DataAccess.Model;
using System;
using System.Collections.Generic;

namespace ShoppingCartConsole.Client
{
    public class MainConsole
    {
        public static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var shoppingcartRepository = container.Resolve<IShoppingCart>();
            shoppingcartRepository.Dispose();

             var myCart = new List<Item>()
            {
                new Item {Id = 1, Name="Cereal", Quantity=2, UnitPrice=3.49m },
                new Item {Id = 2, Name="Milk", Quantity=1, UnitPrice=2.49m },
                new Item {Id = 3, Name="Bread", Quantity=2, UnitPrice=1.29m },
                new Item {Id = 4, Name="Jam", Quantity=1, UnitPrice=3.15m },
                new Item {Id = 5, Name="Coffee", Quantity=1, UnitPrice=6.49m }
            };
            //adding the above items into database
            Console.WriteLine("List of item in the shopping cart: \n");
            foreach (var item in myCart)
            {
                shoppingcartRepository.AddItem(item);
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(shoppingcartRepository.ToString());

            //ADDING NEW item into shopping cart
            var apples = new Item()
            {
                Id = 6,
                Name = "Apples",
                Quantity = 5,
                UnitPrice = 0.99m
            };
            shoppingcartRepository.AddItem(apples);

            Console.WriteLine("\nList of item in the shopping cart after adding Apples: \n");
            foreach (var item in shoppingcartRepository.GetAllItems())
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(shoppingcartRepository.ToString());

            //UPDATING item inside the cart
            var moreApples = new Item()
            {
                Id = 6,
                Name = "Apples",
                Quantity = 10,
                UnitPrice = 0.99m
            };
            shoppingcartRepository.UpdateItem(moreApples);

            Console.WriteLine("\nList of item in the shopping cart after updating Apples: \n");
            foreach (var item in shoppingcartRepository.GetAllItems())
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(shoppingcartRepository.ToString());

            //DELETING an existing item inside the cart
            shoppingcartRepository.DeleteItem(moreApples);

            Console.WriteLine("\nList of item in the shopping cart after deleting Apples: \n");
            foreach (var item in shoppingcartRepository.GetAllItems())
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine(shoppingcartRepository.ToString());

            Console.ReadKey();
        }
    }
}
