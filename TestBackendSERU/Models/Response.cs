using System.Collections;

namespace TestBackendSERU.Models
{
    public class Response
    {
        public class Default
        {
            public string Status { get; set; }
            public object? Message { get; set; }
        }

        public class DtoLogin
        {
            public string Name { get; set; }
            public bool Is_Admin { get; set; }
            public string Token { get; set; }
        }

        public class DtoCheckName
        {
            public string Name { get; set; }
        }

        public class DtoGetType
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Brand_Name { get; set; }
        }

        public class DtoGetModel
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Type_Name { get; set; }
        }

        public class DtoGetYear
        {
            public int ID { get; set; }
            public string Year { get; set; }
        }

        public class DtoGetPricelist
        {
            public int ID { get; set; }
            public string Code { get; set; }
            public int Price { get; set; }
            public string Year { get; set; }
            public string Model { get; set; }
        }

        public class GetAllBrand
        {
            public IEnumerable Brand { get; set; }

            public PaginationMetadata Paginantion { get; set; }
        }

        public class GetBrandById
        {
            public int ID { get; set; }

            public string Name { get; set; }
        }

        public class GetAllType
        {
            public IEnumerable Type { get; set; }

            public PaginationMetadata Paginantion { get; set; }
        }

        public class GetAllModel
        {
            public IEnumerable Model { get; set; }

            public PaginationMetadata Paginantion { get; set; }
        }

        public class GetAllPricelist
        {
            public IEnumerable Pricelist { get; set; }

            public PaginationMetadata Paginantion { get; set; }
        }

        public class GetAllYear
        {
            public IEnumerable Year { get; set; }

            public PaginationMetadata Paginantion { get; set; }
        }

        public class PaginationMetadata
        {
            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public bool HasNext => CurrentPage < TotalPages;
            public bool HasPrevious => CurrentPage > 1;
        }
    }
}
