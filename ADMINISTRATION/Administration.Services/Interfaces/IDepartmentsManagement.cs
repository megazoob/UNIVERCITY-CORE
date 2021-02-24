using Administration.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Services.Interfaces
{
    /// <summary>
    /// Создание, редактирование, отображение отделов.
    /// </summary>
    public interface IDepartmentsManagement
    {
        /// <summary>
        /// Создание отдела.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberOfStaffUnits"></param>
        /// <param name="parentId"></param>
        /// <returns>string - successful или Error:  </returns>
        Task<string> CreateDepartment(string name, int numberOfStaffUnits, string parentId);

        /// <summary>
        /// Редактирование отдела.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <param name="numberOfStaffUnits"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> UpdateDepartment(string id, string parentId, int numberOfStaffUnits, string name);

        /// <summary>
        /// abolished = true = удаление.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="abolished"></param>
        /// <returns>string - successful или Error:  </returns>
        Task<string> ChangeAbolishDepartment(string id, Boolean abolished);

        /// <summary>
        /// Список всех отделов или подчиненных отделу с id == parentid
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>Task<IList<Department>></returns>
        Task<IList<Department>> GetDepartmentsList(string parentId = "");
    }
}
