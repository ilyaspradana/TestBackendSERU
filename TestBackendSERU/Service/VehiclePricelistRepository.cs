using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestBackendSERU.Entity;
using TestBackendSERU.IService;
using TestBackendSERU.Models;
using static TestBackendSERU.Models.Response;

namespace TestBackendSERU.Service
{
    public class VehiclePricelistRepository : IVehiclePricelistRepository
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly VehicleModelRepository _vehicleModelRepository;
        private readonly VehicleYearRepository _vehicleYearRepository;

        public VehiclePricelistRepository(VehicleDbContext dbContext, IConfiguration configuration,
            VehicleModelRepository vehicleModelRepository, VehicleYearRepository vehicleYearRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _vehicleModelRepository = vehicleModelRepository;
            _vehicleYearRepository = vehicleYearRepository;
        }

        public async Task<GetAllPricelist> GetPricelist(int limit, int offset)
        {
            var totalCount = await _dbContext.Pricelist.CountAsync();
            var query = await _dbContext.Pricelist
            .FromSqlInterpolated($@"
                SELECT pl.ID, pl.Code, pl.Price, pl.Model_ID, pl.Year_ID, vy.Year AS Year, vm.Name AS Model
                FROM pricelist pl
                LEFT JOIN vehicle_year vy 
                ON pl.Year_ID = vy.ID 
                LEFT JOIN vehicle_model vm 
                ON pl.Model_ID = vm.ID 
                ORDER BY pl.ID 
                OFFSET {offset} ROWS 
                FETCH NEXT {limit} ROWS ONLY")
            .Select(pl => new
            {
                ID = pl.ID,
                Code = pl.Code,
                Price = pl.Price,
                Year = pl.Vehicle_Year.Year, 
                Model = pl.Vehicle_Model.Name
            })
            .ToListAsync();

            var result = query.Select(x => new DtoGetPricelist
            {
                ID = x.ID,
                Code = x.Code,
                Price = x.Price,
                Year = x.Year,
                Model = x.Model
            }).ToList();

            var metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = limit,
                CurrentPage = offset / limit + 1,
                TotalPages = (int)System.Math.Ceiling(totalCount / (double)limit)
            };

            var data = new GetAllPricelist()
            {
                Paginantion = metadata,
                Pricelist = result
            };

            return data;
        }

        public async Task<Default> GetPricelistById(int id)
        {
            var r = new Default();

            var type = await GetByIdDto(id);

            if (type == null)
            {
                r.Status = "Failed";
                r.Message = "Model tidak ditemukan";

                return r;
            }

            r.Status = "Success";
            r.Message = type;

            return r;
        }

        public async Task<Default> Add(Vehicle.Pricelist.AddPrice data)
        {
            var r = new Default();

            if (data == null)
            {
                r.Status = "Failed";
                r.Message = "Data yang dikirmkan salah";
                return r;
            }

            if (string.IsNullOrEmpty(data.Code))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Code Terlebih Dahulu";
                return r;
            }

            if (data.Price <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harga tidak bisa 0";
                return r;
            }

            if (data.Year_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Pilih Tahun Terlebih Dahulu";
                return r;
            }

            if ( data.Model_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Pilih Model Terlebih Dahulu";
                return r;
            }

            var model = await _vehicleModelRepository.GetById(data.Model_Id);
            if (model == null)
            {
                r.Status = "Failed";
                r.Message = "Model Tidak Ditemukan";

                return r;
            }

            var year = await _vehicleYearRepository.GetById(data.Year_Id);
            if (year == null)
            {
                r.Status = "Failed";
                r.Message = "Tahun Tidak Ditemukan";

                return r;
            }

            if (await IsPriceExist(data.Code, data.Model_Id,data.Year_Id))
            {
                r.Status = "Failed";
                r.Message = "Code sudah terdaftar";

                return r;
            }

            var price = new Pricelist()
            {
                Code = data.Code,
                Price = data.Price,
                Created_At = DateTime.Now.ToString(),
                Updated_At = DateTime.Now.ToString(),
                Model_ID = data.Model_Id,
                Year_ID = data.Year_Id
            };

            _dbContext.Add(price);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Pricelist " + data.Code + " berhasil disimpan";

            return r;
        }

        public async Task<Default> Edit(Vehicle.Pricelist.EditPrice data)
        {
            var r = new Default();

            if (data == null)
            {
                r.Status = "Failed";
                r.Message = "Data yang dikirmkan salah";
                return r;
            }

            if (string.IsNullOrEmpty(data.Code))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Code Terlebih Dahulu";
                return r;
            }

            if (data.Price <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harga tidak bisa 0";
                return r;
            }

            if (data.Year_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Pilih Tahun Terlebih Dahulu";
                return r;
            }

            if (data.Model_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Pilih Model Terlebih Dahulu";
                return r;
            }

            var model = await _vehicleModelRepository.GetById(data.Model_Id);
            if (model == null)
            {
                r.Status = "Failed";
                r.Message = "Model Tidak Ditemukan";

                return r;
            }

            var year = await _vehicleYearRepository.GetById(data.Year_Id);
            if (year == null)
            {
                r.Status = "Failed";
                r.Message = "Tahun Tidak Ditemukan";

                return r;
            }

            if (await IsPriceExist(data.Code, data.Model_Id, data.Year_Id))
            {
                r.Status = "Failed";
                r.Message = "Code sudah terdaftar";

                return r;
            }

            var price = await GetById(data.ID);

            if (price == null)
            {
                r.Status = "Failed";
                r.Message = "Pricelist tidak ditemukan";

                return r;
            }

            price.Code = data.Code;
            price.Updated_At = DateTime.Now.ToString();
            price.Price = data.Price;

            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Model berhasil diubah";

            return r;
        }

        public async Task<Default> Delete(Vehicle.Pricelist.DeletePrice data)
        {
            var r = new Default();
            var price = await GetById(data.ID);

            if (price == null)
            {
                r.Status = "Failed";
                r.Message = "Pricelist tidak ditemukan";

                return r;
            }

            _dbContext.Remove(price);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Pricelist berhasil dihapus";

            return r;
        }

        public async Task<bool> IsPriceExist(string name, int model_id,int year_id)
        {
            var query = await _dbContext.Pricelist
            .FromSqlInterpolated($@"
                SELECT pl.ID
                FROM pricelist pl
                LEFT JOIN vehicle_model vm ON pl.Model_ID = vm.ID
                LEFT JOIN vehicle_year vy ON pl.Year_ID = vy.ID
                WHERE vm.ID = {model_id} AND vy.ID = {year_id} AND pl.Code = {name}")
            .Select(pl => new
            {
                pl.ID
            })
            .SingleOrDefaultAsync();

            if (query == null)
                return false;
            else
                return true;
        }

        public async Task<Pricelist> GetById(int id)
        {
            return await _dbContext.Pricelist
                .FromSqlInterpolated($"SELECT * FROM dbo.[Pricelist] WHERE ID = {id}")
                .SingleOrDefaultAsync();
        }

        public async Task<DtoGetPricelist> GetByIdDto(int id)
        {
            var query = await _dbContext.Pricelist
          .FromSqlInterpolated($@"
                SELECT pl.ID, pl.Code, pl.Price, pl.Model_ID, pl.Year_ID, vy.Year AS Year, vm.Name AS Model
                FROM pricelist pl
                LEFT JOIN vehicle_year vy 
                ON pl.Year_ID = vy.ID 
                LEFT JOIN vehicle_model vm 
                ON pl.Model_ID = vm.ID 
                WHERE pl.ID = {id}")
           .Select(pl => new
           {
               ID = pl.ID,
               Code = pl.Code,
               Price = pl.Price,
               Year = pl.Vehicle_Year.Year,
               Model = pl.Vehicle_Model.Name
           })
          .SingleOrDefaultAsync();

            if (query == null)
                return null;

            return new DtoGetPricelist() { ID = query.ID, Code = query.Code, Price = query.Price, Model = query.Model, Year=query.Year };
        }
    }
}
