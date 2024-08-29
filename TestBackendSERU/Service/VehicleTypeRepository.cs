using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;
using TestBackendSERU.Entity;
using TestBackendSERU.IService;
using TestBackendSERU.Models;
using Microsoft.EntityFrameworkCore;
using static TestBackendSERU.Models.Response;

namespace TestBackendSERU.Service
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly VehicleBrandRepository _vehicleBrandRepository;

        public VehicleTypeRepository(VehicleDbContext dbContext, IConfiguration configuration, VehicleBrandRepository vehicleBrandRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _vehicleBrandRepository = vehicleBrandRepository;
        }

        public async Task<GetAllType> GetType(int limit, int offset)
        {
            var totalCount = await _dbContext.Vehicle_Type.CountAsync();
            var query = await _dbContext.Vehicle_Type
            .FromSqlInterpolated($@"
                SELECT vt.ID, vt.Name, vt.Brand_ID, vb.Name AS Brand_Name
                FROM vehicle_type vt
                LEFT JOIN vehicle_brand vb 
                ON vt.Brand_ID = vb.ID 
                ORDER BY vt.ID 
                OFFSET {offset} ROWS 
                FETCH NEXT {limit} ROWS ONLY")
            .Select(vt => new
            {
                vt.ID,
                vt.Name,
                Brand_Name = vt.Vehicle_Brand.Name
            })
            .ToListAsync();

            var result = query.Select(x => new DtoGetType
            {
                ID = x.ID,
                Name = x.Name,
                Brand_Name = x.Brand_Name
            }).ToList();

            var metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = limit,
                CurrentPage = offset / limit + 1,
                TotalPages = (int)System.Math.Ceiling(totalCount / (double)limit)
            };

            var data = new GetAllType()
            {
                Paginantion = metadata,
                Type = result
            };

            return data;
        }

        public async Task<Default> GetTypeById(int id)
        {
            var r = new Default();

            var type = await GetByIdDto(id);

            if (type == null)
            {
                r.Status = "Failed";
                r.Message = "Tipe tidak ditemukan";

                return r;
            }

            r.Status = "Success";
            r.Message = type;

            return r;
        }

        public async Task<Default> Add(Vehicle.Type.AddType data)
        {
            var r = new Default();

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Nama Type Terlebih Dahulu";

                return r;
            }

            if (data.Brand_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harap Pilih Brand Terlebih Dahulu";

                return r;
            }

            if (!_vehicleBrandRepository.IsBrandExistById(data.Brand_Id)) 
            {
                r.Status = "Failed";
                r.Message = "Brand Tidak Ditemukan";

                return r;
            }

            if (IsTypeExist(data.Name, data.Brand_Id))
            {
                r.Status = "Failed";
                r.Message = "Tipe sudah terdaftar";

                return r;
            }

            var brand = new Vehicle_Type()
            {
                Name = data.Name,
                Created_At = DateTime.Now.ToString(),
                Updated_At = DateTime.Now.ToString(), 
                Brand_ID = data.Brand_Id
            };

            _dbContext.Add(brand);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Tipe "+ data.Name + " berhasil disimpan";

            return r;
        }

        public async Task<Default> Edit(Vehicle.Type.EditType data)
        {
            var r = new Default();

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Nama Type Terlebih Dahulu";

                return r;
            }

            if (data.Brand_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harap Pilih Brand Terlebih Dahulu";

                return r;
            }

            if (!_vehicleBrandRepository.IsBrandExistById(data.Brand_Id))
            {
                r.Status = "Failed";
                r.Message = "Brand Tidak Ditemukan";

                return r;
            }

            if (IsTypeExist(data.Name,data.Brand_Id))
            {
                r.Status = "Failed";
                r.Message = "Tipe sudah terdaftar";

                return r;
            }

            var type = await GetById(data.ID);

            if (type == null)
            {
                r.Status = "Failed";
                r.Message = "Tipe tidak ditemukan";

                return r;
            }


            type.Name = data.Name;
            type.Updated_At = DateTime.Now.ToString();
            type.Brand_ID = data.Brand_Id;

            _dbContext.Update(type);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Tipe berhasil diubah";

            return r;
        }

        public async Task<Default> Delete(Vehicle.Type.DeleteType data)
        {
            var r = new Default();

            var type = await GetById(data.ID);

            if (type == null)
            {
                r.Status = "Failed";
                r.Message = "Tipe tidak ditemukan";

                return r;
            }

            _dbContext.Remove(type);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Tipe berhasil dihapus";

            return r;
        }

        public bool IsTypeExist(string name,int brand_id)
        {
            var a = _dbContext.Vehicle_Brand
            .FromSqlInterpolated($"SELECT Name FROM dbo.[Vehicle_Type] WHERE Name = {name} AND Brand_ID = {brand_id}")
            .Select(u => new DtoCheckName { Name = u.Name })
            .ToList();

            if (a.Count > 0)
                return true;
            else
                return false;
        }

        public async Task<Vehicle_Type> GetById(int id)
        {
            return await _dbContext.Vehicle_Type
                .FromSqlInterpolated($"SELECT * FROM dbo.[Vehicle_Type] WHERE ID = {id}")
                .SingleOrDefaultAsync();
        }

        public async Task<DtoGetType> GetByIdDto(int id)
        {
            var query = await _dbContext.Vehicle_Type
           .FromSqlInterpolated($@"
                SELECT vt.ID, vt.Name, vt.Brand_ID, vb.Name AS Brand_Name
                FROM vehicle_type vt
                LEFT JOIN vehicle_brand vb 
                ON vt.Brand_ID = vb.ID
                WHERE vt.ID = {id}")
           .Select(vt => new
           {
               vt.ID,
               vt.Name,
               Brand_Name = vt.Vehicle_Brand.Name
           })
           .SingleOrDefaultAsync();

            if (query == null)
                return null;

            return new DtoGetType() { ID = query.ID, Name = query.Name, Brand_Name = query.Brand_Name };
        }
    }
}
