using System;
using System.Collections.Generic;
using System.Linq;
using WiseCartLogic.Entities;

namespace WiseCartLogic
{
    public class CartComparer
    {
        public List<Tuple<string, double>> GetCartsTotalPrices(ShoppingCart shoppingCart, List<Provider> providers)
        {
            return (from provider in providers 
                    let sum = (from product in shoppingCart.Products 
                                let productPrice = provider.Products.Find(p => p.ItemCode == product.ItemCode).Price 
                                select productPrice*product.Amount).Sum()

                    select new Tuple<string, double>(provider.Name, sum)).ToList();
        }

        public List<Tuple<string, List<Product>>> GetCheapestProductsPerProvider(ShoppingCart shoppingCart, List<Provider> providers, int numOfItemsToTake)
        {
            return (from provider in providers
                    let cheapestProducts = provider.Products.Intersect(shoppingCart.Products)
                                                            .OrderBy(p => p.Price)
                                                            .Take(numOfItemsToTake).ToList()
                    select new Tuple<string, List<Product>>(provider.Name, cheapestProducts)).ToList();
        }

        public List<Tuple<string, List<Product>>> GetMostExpensivetProductsPerProvider(ShoppingCart shoppingCart, List<Provider> providers, int numOfItemsToTake)
        {
            return (from provider in providers
                    let mostExpensieveProducts = provider.Products.Intersect(shoppingCart.Products)
                                                                  .OrderByDescending(p => p.Price)
                                                                  .Take(3).ToList()
                    select new Tuple<string, List<Product>>(provider.Name, mostExpensieveProducts)).ToList();
        }
    }
}
