using System;
using System.Diagnostics;

namespace WiseCartLogic.Entities
{
    [Serializable]
    public class Product : IEquatable<Product>
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UnitQty { get; set; }
        public bool IsWeighted { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }

        public bool Equals(Product other)
        {
            return ItemCode == other.ItemCode;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            try
            {
                hashCode = ItemCode.GetHashCode();
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.Message);
            }

            return hashCode;
        }
    }
}
