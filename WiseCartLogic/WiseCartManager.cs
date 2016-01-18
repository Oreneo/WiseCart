using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WiseCartLogic.Entities;

namespace WiseCartLogic
{
    public class WiseCartManager
    {
        public string XmlFilesPath { get; set; }
        public List<string> XmlFilesPaths { get; set; }
        public List<Provider> Providers { get; private set; }
        public Provider AllProvidersIntersected { get; set; }
        public ShoppingCart ShoppingCart { get; private set; }
        public Dictionary<string, XDocument> ProvidersXDocuments { get; private set; }

        public WiseCartManager()
        {
            ShoppingCart = new ShoppingCart();
            Providers = new List<Provider>();
            AllProvidersIntersected = new Provider("Intersected");
            ProvidersXDocuments = new Dictionary<string, XDocument>();

            XmlFilesPath = @"..\..\..\Resources\ProvidersXml";
            XmlFilesPaths = Directory.GetFiles(XmlFilesPath).ToList();
        }

        public void InitializeData()
        {
            foreach (string xmlFilePath in XmlFilesPaths)
            {
                string providerName = Path.GetFileNameWithoutExtension(xmlFilePath);

                Providers.Add(new Provider(providerName));
                ProvidersXDocuments.Add(providerName, XDocument.Load(xmlFilePath));
            }

            Parallel.ForEach(XmlFilesPaths, xmlFilePath =>
            {
                string providerName = Path.GetFileNameWithoutExtension(xmlFilePath);

                InitializeProductsList(ProvidersXDocuments[providerName], Providers.Find(p => p.Name == providerName));
            });

            AllProvidersIntersected.Products = Providers[0].Products.Intersect(Providers[1].Products).ToList();

            for (int i = 2; i < Providers.Count; i++)
            {
                AllProvidersIntersected.Products = AllProvidersIntersected.Products.Intersect(Providers[i].Products).ToList();
            }

            IndexListItems(AllProvidersIntersected.Products);
        }

        private void InitializeProductsList(XDocument providerXDocument, Provider provider)
        {
            var allProducts = from item in providerXDocument.Descendants("Item")
                              select new
                              {
                                  ItemCode = item.Element("ItemCode").Value,
                                  ItemName = item.Element("ItemName").Value,
                                  UnitQty = item.Element("UnitQty").Value,
                                  IsWeighted = item.Element("bIsWeighted").Value,
                                  ItemPrice = item.Element("ItemPrice").Value
                              };

            foreach (var item in allProducts)
            {
                provider.Products.Add(new Product
                {
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    UnitQty = item.UnitQty,
                    IsWeighted = item.IsWeighted == "1",
                    Price = Convert.ToDouble(item.ItemPrice)
                });
            }
        }

        private void IndexListItems(List<Product> products)
        {
            int index = 1;

            foreach (Product product in products)
            {
                product.ItemId = index++;
            }
        }
    }
}
