using Administration.Data.DataContext.SQL_SERVER;
using Administration.Data.Models;
using Administration.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Services.Services
{
    /// <summary>
    /// Релизация IDepartmentsManagement для SQL Server.
    /// </summary>
    public class DepartmentsManagementSQLServer : IDepartmentsManagement
    {
        DBContextSQLServer _db;

        /// <summary>Внедрение зависимости от контекста базы данных.</summary>
        /// <param name="db"></param>
        public DepartmentsManagementSQLServer(DBContextSQLServer db)
        {
            _db = db;
        }

        /// <summary>
        /// Удаление/восстановление отдела.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="abolished"></param>
        /// <returns>string - successful или Error:  </returns>
        public async Task<string> ChangeAbolishDepartment(string id, bool abolished)
        {
            string result = "successful";

            var item = _db.Departments.Where(p => p.Id.Equals(id)).FirstOrDefault();

            if (item!=null)
            {
                item.Abolished = abolished;
                _db.Update<Department>(item);
                try
                {
                    await _db.SaveChangesAsync();
                } catch (Exception e) {
                    result = "Error:" + e.Message;
                }
               
              
            }

            return result;
        }

        /// <summary>
        /// Создание отдела.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberOfStaffUnits"></param>
        /// <param name="parentId"></param>
        /// <returns>string - successful или Error:  </returns>
        public async Task<string> CreateDepartment(string name, int numberOfStaffUnits, string parentId)
        {
            string result = "successful";

            if (numberOfStaffUnits == 0)
            {
                numberOfStaffUnits = 1;
            }

            Department newdep = new Department
            {
                Abolished = false,
                CreatedDate = DateTime.Now,
                Name = name,
                NumberOfStaffUnits = numberOfStaffUnits,
                SubordinateToId = parentId
            };

            try
            {
               await _db.Departments.AddAsync(newdep);
               await _db.SaveChangesAsync();
            } catch (Exception e)
            {
                result = "Error: " + e.Message;
            }

            return result;
        }

        /// <summary>
        /// Список всех отделов или подчиненных отделу с id == parentid
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>IList Department</returns>
        public async Task<IList<Department>> GetDepartmentsList(string parentId = "")
        {

            return await _db.Departments.Include(e => e.ManagesDepartments)
                         .Where(p => string.IsNullOrEmpty(p.SubordinateToId).Equals(parentId))
                         .Where(p => p.Abolished == false).ToListAsync<Department>();

            
        }

        /// <summary>
        /// Редактирование отдела.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="numberOfStaffUnits"></param>
        /// <param name="name"></param>
        /// <returns>string - successful или Error:  </returns>
        public async Task<string> UpdateDepartment(string id, string parentId, int numberOfStaffUnits, string name)
        {
            string result = "successful";

            if (numberOfStaffUnits == 0)
            {
                numberOfStaffUnits = 1;
            }

            var item = _db.Departments.Where(p => p.Id.Equals(id)).FirstOrDefault();

            if (item != null)
            {
                item.NumberOfStaffUnits = numberOfStaffUnits;
                item.Name = name;
                item.SubordinateToId = parentId;

                try
                {

                    _db.Update<Department>(item);
                    await _db.SaveChangesAsync();

                } catch (Exception e)
                {
                    result = "Error: " + e.Message;
                }

            }

            return result;
        }
    }
}
