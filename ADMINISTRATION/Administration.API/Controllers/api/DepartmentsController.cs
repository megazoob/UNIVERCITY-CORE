using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Administration.Services.Interfaces;
using Administration.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Filter.Authorization;

namespace Administration.API.Controllers.api
{
    //[Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class DepartmentsController : ControllerBase
    {
        IDepartmentsManagement _depManage;

        public DepartmentsController(IDepartmentsManagement depManage)
        {
            _depManage = depManage;
        }

        [HttpGet("api/createdepartment")]
        [AuthorizationWithRoles("Administrator")]
        public async Task<string> CreateDepartment(string name, int numberOfStaffUnits, string parentId="")
        {
            return await _depManage.CreateDepartment(name, numberOfStaffUnits, parentId);
        }


        [HttpGet("api/updatedepartment")]
        [AuthorizationWithRoles("Administrator")]
        public async Task<string> UpdateDepartment(string id, string parentId, int numberOfStaffUnits, string name)
        {
            return await _depManage.UpdateDepartment(id, parentId, numberOfStaffUnits, name);
        }


        [HttpGet("api/changeabolisheddepartment")]
        [AuthorizationWithRoles("Administrator")]
        public async Task<string> ChangeAbolishDepartment(string id, Boolean abolished)
        {
            return await _depManage.ChangeAbolishDepartment(id, abolished);
        }


        [HttpGet("api/getdepartmentslist")]
        [IsAuthorized]
        public async Task<IList<Department>> GetDepartmentsList(string parentId = "")
        {
            return await _depManage.GetDepartmentsList(parentId);
        }
    }
}
