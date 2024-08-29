
using System.Collections.Generic;

namespace TestBackendSERU.Entity
{
    public class Vehicle_Brand : Audit
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Vehicle_Type> Vehicle_Types { get; set; }
    }

    public class Vehicle_Type : Audit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Brand_ID { get; set; }
        public Vehicle_Brand Vehicle_Brand { get; set; }
        public ICollection<Vehicle_Model> Vehicle_Model { get; set; }
    }

    public class Vehicle_Model : Audit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Type_ID { get; set; }

        public Vehicle_Type Vehicle_Type { get; set; }
        public ICollection<Pricelist> Pricelist { get; set; }
    }

    public class Vehicle_Year : Audit
    {
        public int ID { get; set; }
        public string Year { get; set; }

        public ICollection<Pricelist> Pricelist { get; set; }
    }

    public class Pricelist : Audit
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int Price { get; set; }
        public int? Year_ID { get; set; }
        public int? Model_ID { get; set; }

        public Vehicle_Model Vehicle_Model { get; set; }
        public Vehicle_Year Vehicle_Year { get; set; }
    }
}
