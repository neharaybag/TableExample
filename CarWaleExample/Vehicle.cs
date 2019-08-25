using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWaleExample
{
    class Vehicle
    {
        string carName;
        string price;
        List<Vehicle> variants;
        

        public Vehicle(string carName,string price)
        {
            this.CarName = carName;
            this.Price = price;
        }

        public Vehicle(string carName, string price, List<Vehicle> variants)
        {
            this.CarName = carName;
            this.Price = price;
            this.Variants = variants;
        }

        public string Price { get => price; set => price = value; }
        public string CarName { get => carName; set => carName = value; }
        internal List<Vehicle> Variants { get => variants; set => variants = value; }
    }
}
