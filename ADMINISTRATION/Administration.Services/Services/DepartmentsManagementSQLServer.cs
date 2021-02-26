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
        private readonly DBContextSQLServer _db;
        private readonly int _minId;

        /// <summary>Внедрение зависимости от контекста базы данных.</summary>
        /// <param name="db"></param>
        public DepartmentsManagementSQLServer(DBContextSQLServer db)
        {
            _db = db;

            _minId = _db.Departments.Min(p => p.Id);
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

            Department newdep = new Department();
            newdep.Abolished = false;
            newdep.CreatedDate = DateTime.Now;
            newdep.Name = name;
            newdep.NumberOfStaffUnits = numberOfStaffUnits;

            int pid = 0;
            if (!string.IsNullOrEmpty(parentId))
            {
                try
                {
                    pid = Int32.Parse(parentId);
                } catch (Exception e)
                {
                    pid = 0;
                }
            }
            if (pid==0)
            {
                pid = _minId;
            }

            newdep.SubordinateToId = pid;

            try
            {
                await _db.Departments.AddAsync(newdep);
               await _db.SaveChangesAsync();
            }
            catch (Exception e)
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
            int pid = 0;
            if (!string.IsNullOrEmpty(parentId))
            {
                try
                {
                    pid = Int32.Parse(parentId);
                }
                catch (Exception e)
                {
                    pid = 0;
                }
            }

            if (pid==0)
            {
                pid = _minId;
            }

            return await _db.Departments.Include(e => e.ManagesDepartments)
                         .Where(p => p.SubordinateToId == pid && p.Abolished == false).ToListAsync<Department>();

            
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

                int pid = 0;
                if (!string.IsNullOrEmpty(parentId))
                {
                    try
                    {
                        pid = Int32.Parse(parentId);
                    }
                    catch (Exception e)
                    {
                        pid = 0;
                    }
                }

                if (pid==0)
                {
                    pid = _minId;
                }

                item.SubordinateToId = pid;
              

                
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
