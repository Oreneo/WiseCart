using System;
using System.Collections.Generic;
using WiseCartLogic.Entities;

namespace WiseCartLogic
{
    [Serializable]
    public class ShoppingCart
    {
        public List<Product> Products { get; private set; }

        public ShoppingCart()
        {
            Products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public void BuildCart(List<Product> allProducts)
        {
            Products.Clear();

            foreach (Product product in allProducts)
            {
                if (product.Amount > 0)
                {
                    Products.Add(product);
                }
            }
        }
    }
}
