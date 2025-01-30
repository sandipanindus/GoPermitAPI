using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.RoleManagement
{
    public interface IRoleRepository
    {
        Task<dynamic> AddRole(AddRoleAc addRole);
        Task<dynamic> UpdateRole(AddRoleAc addRoleAc);
        Task<dynamic> GetRoles(int PageNo, int PageSize,int LoginId, int RoleId);
        Task<Role> GetRoleById(int Id);
        bool GetExistsRole(AddRoleAc addRoleAc);
        Task<dynamic> DeleteRole(int Id);
    }
}
