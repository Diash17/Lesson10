using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson10
{
    public class Store
    {
        public string Name { get; set; }
        public int StorageSize { get; set; }
        public int ApplePrice { get; set; }
        public int OrangePrice { get; set; }
        public int AppleStock { get; set; }

        public int OrangeStock { get; set; }

        public int AppleSold { get; set; }

        public int OrangeSold { get; set; }

        public Store(string name, int storageSize, int applePrice, int orangePrice, int appleStock, int orangeStock, int appleSold, int orangeSold)
        {
            Name = name;
            StorageSize = storageSize;
            ApplePrice = applePrice;
            OrangePrice = orangePrice;
            AppleStock = appleStock;
            OrangeStock = orangeStock;
            AppleSold = appleSold;
            OrangeSold = orangeSold;
        }
    }
}

