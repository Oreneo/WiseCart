using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WiseCartLogic
{
    public class CartSerializer
    {
        public string CartXmlPath { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        public CartSerializer(ShoppingCart shoppingCart)
        {
            CartXmlPath = Environment.CurrentDirectory + @"\..\..\..\Resources\UserCart.xml";
            ShoppingCart = shoppingCart;
        }

        public void SaveCart()
        {
            File.Delete(CartXmlPath);

            XmlSerializer serializer = new XmlSerializer(typeof(ShoppingCart));

            using (StreamWriter file = new StreamWriter(CartXmlPath))
            {
                serializer.Serialize(file, ShoppingCart);
            }
        }

        public void LoadCart()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(ShoppingCart));

            if (File.Exists(CartXmlPath))
            {
                using (TextReader reader = new StreamReader(CartXmlPath))
                {
                    ShoppingCart = (ShoppingCart)deserializer.Deserialize(reader);
                }
            }
            else
            {
                Debug.WriteLine("File missing");
            }
        }
    }
}
