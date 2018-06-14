using System;
using Autofac;
using ShoppingCartProject.DataAccess;
using ShoppingCartProject.DataAccess.Model;

namespace ShoppingCartConsole.Client.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Item>().As<IItem>();
            builder.RegisterType<ShoppingCart>().As<IShoppingCart>();

            return builder.Build();
        }
    }
}
