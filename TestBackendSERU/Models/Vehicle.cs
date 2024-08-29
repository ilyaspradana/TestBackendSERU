namespace TestBackendSERU.Models
{
    public class Vehicle
    {
        public class Brand
        {
            public class Get
            {
                public int ID { get; set; }
                public string Name { get; set; }
            }

            public class Add
            {
                public string Name { get; set; }
            }

            public class Edit
            {
                public int ID { get; set; }
                public string Name { get; set; }
            }

            public class Delete
            {
                public int ID { get; set; }
            }
        }

        public class Model
        {
            public class AddModel
            {
                public string Name { get; set; }
                public int Type_Id { get; set; }
            }

            public class EditModel
            {
                public int ID { get; set; }
                public string Name { get; set; }
                public int Type_Id { get; set; }
            }

            public class DeleteModel
            {
                public int ID { get; set; }
            }
        }

        public class Pricelist
        {
            public class AddPrice
            {
                public string Code { get; set; }
                public int Price { get; set; }
                public int Year_Id { get; set; }
                public int Model_Id { get; set; }
            }

            public class EditPrice
            {
                public int ID { get; set; }
                public string Code { get; set; }
                public int Price { get; set; }
                public int Year_Id { get; set; }
                public int Model_Id { get; set; }
            }

            public class DeletePrice
            {
                public int ID { get; set; }
            }
        }

        public class Type
        {
            public class AddType
            {
                public string Name { get; set; }
                public int Brand_Id { get; set; }
            }

            public class EditType
            {
                public int ID { get; set; }
                public string Name { get; set; }
                public int Brand_Id { get; set; }
            }

            public class DeleteType
            {
                public int ID { get; set; }
            }
        }

        public class Year
        {
            public class AddYear
            {
                public string Year { get; set; }
            }

            public class EditYear
            {
                public int ID { get; set; }
                public string Year { get; set; }
            }

            public class DeleteYear
            {
                public int ID { get; set; }
            }
        }
    }
}
