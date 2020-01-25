using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Extensions;
using Vega.Persistance;

namespace vega.Persistance
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;
        public VehicleRepository(VegaDbContext context)
        {
            _context = context;
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            _context.Remove(vehicle);
        }

        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true){

            if(includeRelated)
                return await _context.Vehicles
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .FirstOrDefaultAsync(v => v.Id == id);
            
            else
                return await _context.Vehicles.FindAsync(id);
            
        }

        public async Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery queryObj){
            var result = new QueryResult<Vehicle>();
            var query =  _context.Vehicles
            .Include(v => v.Features)
                .ThenInclude(vf => vf.Feature)
            .Include (v => v.Model)
                .ThenInclude(m => m.Make)
            .AsQueryable();
            
            if(queryObj.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);
            
            if(queryObj.ModelId.HasValue)
                query = query.Where(v => v.ModelId == queryObj.ModelId.Value);

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>(){
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,

            };
           query = query.ApplyOrdering(queryObj,columnsMap);

           result.TotalItems = await query.CountAsync();

           query = query.ApplyPaging(queryObj);

            #region  "Order logic without Dictionary"
            // if(queryObj.SortBy == "make")
            //     query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);
            
            // if(queryObj.SortBy == "model")
            //     query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);

            // if(queryObj.SortBy == "contactName")
            //     query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);
            
            // if(queryObj.SortBy == "id")
            //     query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Id) : query.OrderByDescending(v => v.Id);
            #endregion

            result.Items = await query.ToListAsync();

            return result;

        }

       
    }
}