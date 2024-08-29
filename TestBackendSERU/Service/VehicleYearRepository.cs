using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Linq;
using TestBackendSERU.Entity;
using TestBackendSERU.IService;
using TestBackendSERU.Models;
using static TestBackendSERU.Models.Response;

namespace TestBackendSERU.Service
{
    public class VehicleYearRepository : IVehicleYearRepository
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public VehicleYearRepository(VehicleDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<GetAllYear> GetYear(int limit, int offset)
        {
            var totalCount = await _dbContext.Vehicle_Year.CountAsync();
            var query = await _dbContext.Vehicle_Year
            .FromSqlInterpolated($@"
                SELECT * FROM dbo.[Vehicle_Year]
                ORDER BY ID 
                OFFSET {offset} ROWS 
                FETCH NEXT {limit} ROWS ONLY")
            .Select(vy => new DtoGetYear
            {
                 ID = vy.ID,
                 Year = vy.Year
            })
            .ToListAsync();

            var metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = limit,
                CurrentPage = offset / limit + 1,
                TotalPages = (int)System.Math.Ceiling(totalCount / (double)limit)
            };

            var data = new GetAllYear()
            {
                Paginantion = metadata,
                Year = query
            };

            return data;
        }

        public async Task<Default> GetYearById(int id)
        {
            var r = new Default();

            var type = await GetByIdDto(id);

            if (type == null)
            {
                r.Status = "Failed";
                r.Message = "Tahun tidak ditemukan";

                return r;
            }

            r.Status = "Success";
            r.Message = type;

            return r;
        }

        public async Task<Default> Add(Vehicle.Year.AddYear data)
        {
            var r = new Default();

            if (data == null || string.IsNullOrEmpty(data.Year))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Tahun Terlebih Dahulu";

                return r;
            }

            if (!IsNumeric(data.Year))
            {
                r.Status = "Failed";
                r.Message = "Tahun harus berupa angka";

                return r;
            }

            if (IsYearExist(data.Year))
            {
                r.Status = "Failed";
                r.Message = "Tahun sudah terdaftar";

                return r;
            }

            var year = new Vehicle_Year()
            {
                Year = data.Year,
                Created_At = DateTime.Now.ToString(),
                Updated_At = DateTime.Now.ToString()
            };

            _dbContext.Add(year);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Tahun " + data.Year + " berhasil disimpan";

            return r;
        }

        public async Task<Default> Edit(Vehicle.Year.EditYear data)
        {
            var r = new Default();

            if (data == null || string.IsNullOrEmpty(data.Year))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Tahun Terlebih Dahulu";

                return r;
            }


            if (!IsNumeric(data.Year))
            {
                r.Status = "Failed";
                r.Message = "Tahun harus berupa angka";

                return r;
            }

            if (IsYearExist(data.Year))
            {
                r.Status = "Failed";
                r.Message = "Tahun sudah terdaftar";

                return r;
            }

            var year = await GetById(data.ID);

            if (year == null)
            {
                r.Status = "Failed";
                r.Message = "Tahun tidak ditemukan";

                return r;
            }

            year.Year = data.Year;
            year.Updated_At = DateTime.Now.ToString();

            _dbContext.Update(year);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Tahun berhasil diubah";

            return r;
        }

        public async Task<Default> Delete(Vehicle.Year.DeleteYear data)
        {
            var r = new Default();

            var model = await GetById(data.ID);

            if (model == null)
            {
                r.Status = "Failed";
                r.Message = "Tahun tidak ditemukan";

                return r;
            }

            _dbContext.Remove(model);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Tahun berhasil dihapus";

            return r;
        }

        public async Task<Vehicle_Year> GetById(int id)
        {
            return await _dbContext.Vehicle_Year
                .FromSqlInterpolated($"SELECT * FROM dbo.[Vehicle_Year] WHERE ID = {id}")
                .SingleOrDefaultAsync();
        }

        public async Task<DtoGetYear> GetByIdDto(int id)
        {
            var query = await _dbContext.Vehicle_Year
            .FromSqlInterpolated($"SELECT * FROM dbo.[Vehicle_Year] WHERE ID = {id}")
            .Select(vy => new DtoGetYear
                {
                    ID = vy.ID,
                    Year = vy.Year
                })
            .SingleOrDefaultAsync();

            if (query == null)
                return null;

            return query;
        }

        public bool IsYearExist(string tahun)
        {
            var a = _dbContext.Vehicle_Year
            .FromSqlInterpolated($"SELECT Year FROM dbo.[Vehicle_Year] WHERE Year = {tahun}")
            .Select(u => new DtoCheckName { Name = u.Year })
            .ToList();

            if (a.Count > 0)
                return true;
            else
                return false;
        }

        public bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
