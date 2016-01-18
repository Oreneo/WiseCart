using System.Collections.Generic;
using WiseCartLogic.Entities;

namespace WiseCartLogic
{
    public class Provider
    {
        public string Name { get; private set; }

        public List<Product> Products { get; set; }

        public Provider(string name)
        {
            Name = name;
            Products = new List<Product>();
        }

        public Provider(string name, List<Product> products)
        {
            Name = name;
            Products = products;
        }
    }
}
