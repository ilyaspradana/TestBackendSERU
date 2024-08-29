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
    public class VehicleModelRepository : IVehicleModelRepository
    {
        private readonly VehicleDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly VehicleTypeRepository _vehicleTypeRepository;

        public VehicleModelRepository(VehicleDbContext dbContext, IConfiguration configuration, VehicleTypeRepository vehicleTypeRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public async Task<GetAllModel> GetModel(int limit, int offset)
        {
            var totalCount = await _dbContext.Vehicle_Model.CountAsync();
            var query = await _dbContext.Vehicle_Model
            .FromSqlInterpolated($@"
                SELECT vm.ID, vm.Name, vm.Type_ID, vt.Name AS Type_Name
                FROM vehicle_model vm
                LEFT JOIN vehicle_type vt 
                ON vm.Type_ID = vt.ID 
                ORDER BY vm.ID 
                OFFSET {offset} ROWS 
                FETCH NEXT {limit} ROWS ONLY")
            .Select(vm => new
            {
                vm.ID,
                vm.Name,
                Type_Name = vm.Vehicle_Type.Name
            })
            .ToListAsync();

            var result = query.Select(x => new DtoGetModel
            {
                ID = x.ID,
                Name = x.Name,
                Type_Name = x.Type_Name
            }).ToList();

            var metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = limit,
                CurrentPage = offset / limit + 1,
                TotalPages = (int)System.Math.Ceiling(totalCount / (double)limit)
            };

            var data = new GetAllModel()
            {
                Paginantion = metadata,
                Model = result
            };

            return data;
        }

        public async Task<Default> GetModelById(int id)
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

        public async Task<Default> Add(Vehicle.Model.AddModel data)
        {
            var r = new Default();

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Nama Model Terlebih Dahulu";

                return r;
            }

            if (data.Type_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harap Pilih Tipe Terlebih Dahulu";

                return r;
            }

            var type = await _vehicleTypeRepository.GetById(data.Type_Id);
            if (type == null)
            {
                r.Status = "Failed";
                r.Message = "Tipe Tidak Ditemukan";

                return r;
            }

            if (IsModelExist(data.Name, data.Type_Id))
            {
                r.Status = "Failed";
                r.Message = "Model sudah terdaftar";

                return r;
            }

            var brand = new Vehicle_Model()
            {
                Name = data.Name,
                Created_At = DateTime.Now.ToString(),
                Updated_At = DateTime.Now.ToString(),
                Type_ID = data.Type_Id
            };

            _dbContext.Add(brand);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Model " + data.Name + " berhasil disimpan";

            return r;
        }

        public async Task<Default> Edit(Vehicle.Model.EditModel data)
        {
            var r = new Default();

            if (data == null || string.IsNullOrEmpty(data.Name))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Nama Model Terlebih Dahulu";

                return r;
            }

            if (data.Type_Id <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harap Pilih Tipe Terlebih Dahulu";

                return r;
            }

            var type = await _vehicleTypeRepository.GetById(data.Type_Id);
            if (type == null)
            {
                r.Status = "Failed";
                r.Message = "Tipe Tidak Ditemukan";

                return r;
            }

            if (IsModelExist(data.Name, data.Type_Id))
            {
                r.Status = "Failed";
                r.Message = "Model sudah terdaftar";

                return r;
            }

            var model = await GetById(data.ID);

            if (model == null)
            {
                r.Status = "Failed";
                r.Message = "Model tidak ditemukan";

                return r;
            }

            model.Name = data.Name;
            model.Updated_At = DateTime.Now.ToString();
            model.Type_ID = data.Type_Id;

            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Model berhasil diubah";

            return r;
        }

        public async Task<Default> Delete(Vehicle.Model.DeleteModel data)
        {
            var r = new Default();

            var model = await GetById(data.ID);

            if (model == null)
            {
                r.Status = "Failed";
                r.Message = "Model tidak ditemukan";

                return r;
            }

            _dbContext.Remove(model);
            await _dbContext.SaveChangesAsync();

            r.Status = "Success";
            r.Message = "Model berhasil dihapus";

            return r;
        }

        public async Task<Vehicle_Model> GetById(int id)
        {
            return await _dbContext.Vehicle_Model
                .FromSqlInterpolated($"SELECT * FROM dbo.[Vehicle_Model] WHERE ID = {id}")
                .SingleOrDefaultAsync();
        }

        public bool IsModelExist(string name, int type_id)
        {
            var a = _dbContext.Vehicle_Model
            .FromSqlInterpolated($"SELECT Name FROM dbo.[Vehicle_Model] WHERE Name = {name} AND Type_ID = {type_id}")
            .Select(u => new DtoCheckName { Name = u.Name })
            .ToList();

            if (a.Count > 0)
                return true;
            else
                return false;
        }

        public async Task<DtoGetModel> GetByIdDto(int id)
        {
            var query = await _dbContext.Vehicle_Model
           .FromSqlInterpolated($@"
                SELECT vm.ID, vm.Name, vm.Type_ID, vt.Name AS Type_Name
                FROM vehicle_model vm
                LEFT JOIN vehicle_type vt 
                ON vm.Type_ID = vt.ID 
                WHERE vm.ID = {id}")
           .Select(vt => new
           {
               vt.ID,
               vt.Name,
               Type_Name = vt.Vehicle_Type.Name
           })
           .SingleOrDefaultAsync();

            if (query == null)
                return null;

            return new DtoGetModel() { ID = query.ID, Name = query.Name, Type_Name = query.Type_Name };
        }
    }
}
