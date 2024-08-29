using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using TestBackendSERU.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static TestBackendSERU.Models.Response;
using TestBackendSERU.Entity;
using System.Collections.Generic;
using static TestBackendSERU.Models.Vehicle;

namespace TestBackendSERU.Service
{
    public class VehicleBrandRepository : IVehicleBrandRepository
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public VehicleBrandRepository(VehicleDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<GetAllBrand> GetBrand(int limit, int offset)
        {
            var totalCount = await _dbContext.Vehicle_Brand.CountAsync();
            var brands = await _dbContext.Vehicle_Brand
                 .FromSqlInterpolated($"SELECT * FROM Vehicle_Brand ORDER BY ID OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY")
                 .ToListAsync();

            List<Brand.Get> list = new List<Brand.Get>();

            foreach(Vehicle_Brand brand in brands)
            {
                list.Add(new Brand.Get()
                {
                    ID = brand.ID,
                    Name = brand.Name
                });
            }

            var metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = limit,
                CurrentPage = offset / limit + 1,
                TotalPages = (int)System.Math.Ceiling(totalCount / (double)limit)
            };

            var data = new GetAllBrand()
            {
                Paginantion = metadata,
                Brand = list
            };

            return data;
        }

        public async Task<GetBrandById> GetBrandById(int id)
        {
            var brand = await GetById(id);

             if (brand == null)
                return null;

            var data = new GetBrandById() 
            {
                ID = brand.ID,
                Name = brand.Name
            };

            return data;
        }

        public async Task<Brand.Add> Add(Brand.Add data)
        {
            if (IsBrandExist(data.Name))
                return null;

            var brand = new Vehicle_Brand()
            {
                Name = data.Name,
                Created_At = DateTime.Now.ToString(),
                Updated_At = DateTime.Now.ToString()
            };

            _dbContext.Add(brand);
            await _dbContext.SaveChangesAsync();

            return data;
        }

        public async Task<Brand.Edit> Edit(Brand.Edit data)
        {
            if (IsBrandExist(data.Name))
                return null;

            var brand = await GetById(data.ID);

            brand.Name = data.Name;
            brand.Updated_At = DateTime.Now.ToString();

            _dbContext.Update(brand);
            await _dbContext.SaveChangesAsync();

            return data;
        }

        public async Task<Brand.Delete> Delete(Brand.Delete data)
        {
            var brand = await GetById(data.ID);

            if (brand == null)
                return null;

            _dbContext.Remove(brand);
            await _dbContext.SaveChangesAsync();

            return data;
        }

        public bool IsBrandExist(string brand)
        {
            var a = _dbContext.Vehicle_Brand
            .FromSqlInterpolated($"SELECT Name FROM dbo.[Vehicle_Brand] WHERE Name = {brand}")
            .Select(u => new DtoCheckName { Name = u.Name })
            .ToList();

            if (a.Count > 0)
                return true;
            else
                return false;
        }

        public bool IsBrandExistById(int id)
        {
            var a = _dbContext.Vehicle_Brand
            .FromSqlInterpolated($"SELECT Name FROM dbo.[Vehicle_Brand] WHERE ID = {id}")
            .Select(u => new DtoCheckName { Name = u.Name })
            .ToList();

            if (a.Count > 0)
                return true;
            else
                return false;
        }

        public async Task<Vehicle_Brand> GetById(int id)
        {
            return await _dbContext.Vehicle_Brand
                .FromSqlInterpolated($"SELECT * FROM dbo.[Vehicle_Brand] WHERE ID = {id}")
                .SingleOrDefaultAsync();
        }
    }
}
