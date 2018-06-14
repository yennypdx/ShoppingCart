using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShoppingCartProject.DataAccess;
using ShoppingCartProject.DataAccess.Model;

namespace ShoppingCartProject.UnitTests
{
    [TestClass]
    public class ShoppingCartProjectTests
    {
        private Mock<IShoppingCart> _mockCartDataService;
        private IShoppingCart _cartDataService;
        private IList<Item> _shoppingCartDb;
        private IList<Item> _shoppingCartJson;

        [TestInitialize]
        public void Init()
        {
            _cartDataService = new ShoppingCart();
            _shoppingCartJson = new List<Item>();
            _shoppingCartJson = _cartDataService.ReadFromDatabase;

            _shoppingCartDb = new List<Item>()
            {
                new Item {Id = 1, Name="Cereal", Quantity=2, UnitPrice=3.49m },
                new Item {Id = 2, Name="Milk", Quantity=1, UnitPrice=2.49m },
                new Item {Id = 3, Name="Bread", Quantity=2, UnitPrice=1.29m },
                new Item {Id = 4, Name="Jam", Quantity=1, UnitPrice=3.15m },
                new Item {Id = 5, Name="Coffee", Quantity=1, UnitPrice=6.49m }
            };

            _cartDataService.SaveToDatabase(_shoppingCartDb);

            _mockCartDataService = new Mock<IShoppingCart>();

            _mockCartDataService.Setup(m => m.AddItem(It.IsAny<Item>())).Callback(
                (Item newItem) =>
                {
                    if (newItem == null) throw new Exception("Item is null.");

                    if (newItem.Id <= 0)
                    {
                        newItem.Id = _shoppingCartDb.Max(f => f.Id) + 1;
                    }
                    _shoppingCartDb.Add(newItem);

                });

            _mockCartDataService.Setup(m => m.DeleteItem(It.IsAny<Item>())).Callback(
                (Item itemToDel) =>
                {
                    var tobeDeleted = _shoppingCartDb.SingleOrDefault(item => item.Id.Equals(itemToDel.Id));
                    _shoppingCartDb.Remove(tobeDeleted);

                }).Verifiable();

            _mockCartDataService.Setup(m => m.UpdateItem(It.IsAny<Item>())).Callback(
                (Item newItem) =>
                {
                    if (newItem == null) throw new Exception("Item is null.");
                    foreach (var it in _shoppingCartDb)
                    {
                        if (it.Id.Equals(newItem.Id))
                        {
                            it.Id = newItem.Id;
                            it.Name = newItem.Name;
                            it.Quantity = newItem.Quantity;
                            it.UnitPrice = newItem.UnitPrice;
                        }
                    }
                });

            _mockCartDataService.Setup(m => m.GetItemById(It.IsAny<int>())).Returns(
                (int itemId) =>
                {
                    return _shoppingCartDb.SingleOrDefault(f => f.Id.Equals(itemId));
                });

            _mockCartDataService.Setup(m => m.GetAllItems()).Returns(
                () =>
                {
                    var items = new List<Item>();
                    _shoppingCartDb.ToList().ForEach(item =>
                    {
                        items.Add(new Item()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice
                        });
                    });
                    return items;
                });
        }

        //*********************************************** TEST STARTS HERE************************************************//

        [TestMethod()]
        public void AddItemTest_AddItemNumSixToCart()
        {
            Item itemTest = new Item
            {
                Id = 6,
                Name = "Candy",
                Quantity = 10,
                UnitPrice = 0.50m
            };

            _mockCartDataService.Object.AddItem(itemTest);

            Assert.AreEqual("Candy", _mockCartDataService.Object.GetItemById(6).Name);
        }

        [TestMethod()]
        public void AddItemTest_TotalItemInCartShouldReturnSix()
        {
            var cart = new ShoppingCart();
            Item itemTest = new Item
            {
                Id = 6,
                Name = "Candy",
                Quantity = 10,
                UnitPrice = 0.50m
            };

            _mockCartDataService.Object.AddItem(itemTest);

            Assert.AreEqual(6, _mockCartDataService.Object.GetAllItems().Count());
        }

        [TestMethod()]
        public void AddItemTest_TotalItemInCartShouldReturnSeven()
        {
            Item itemOne = new Item
            {
                Id = 9,
                Name = "Sugar",
                Quantity = 1,
                UnitPrice = 2.49m
            };

            Item itemTwo = new Item
            {
                Id = 10,
                Name = "Butter",
                Quantity = 1,
                UnitPrice = 1.49m
            };

            _mockCartDataService.Object.AddItem(itemOne);
            _mockCartDataService.Object.AddItem(itemTwo);

            Assert.AreEqual(7, _mockCartDataService.Object.GetAllItems().Count());
        }

        [TestMethod]
        public void AddItemTest_ShouldNotEqualWhenCountIsOff()
        {
            Assert.AreNotEqual(10, _mockCartDataService.Object.GetAllItems().Count());
        }

        [TestMethod()]
        public void DeleteItemTest_DeleteItemNumSixFromcart()
        {
            Item itemTest = new Item
            {
                Id = 6,
                Name = "Candy",
                Quantity = 10,
                UnitPrice = 0.50m
            };

            _mockCartDataService.Object.DeleteItem(itemTest);

            Assert.AreEqual(5, _mockCartDataService.Object.GetAllItems().Count());
        }

        [TestMethod]
        public void DeleteItemTest_DeletingItemNotInTheCart()
        {
            try
            {
                Item itemTest = new Item();

                _mockCartDataService.Object.DeleteItem(itemTest);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        public void UpdateItemTest_UpdatingItemIdNumOne()
        {
            Item itemTest = new Item
            {
                Id = 1,
                Name = "Shampoo",
                Quantity = 1,
                UnitPrice = 4.25m
            };

            _mockCartDataService.Object.UpdateItem(itemTest);

            Assert.AreEqual("Shampoo", _mockCartDataService.Object.GetItemById(1).Name);
        }

        [TestMethod()]
        public void GetAllItemsTest_TempDbCountShouldBeFive()
        {
            Assert.AreEqual(5, _mockCartDataService.Object.GetAllItems().Count());
        }
    }
}
