using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.UserManagement
{
   public interface IUserRepository
    {
        Task<dynamic> AddTenantUser(AddTenantUser addUser);
        Task<dynamic> AddUser(AddUserAc addUser);
        Task<dynamic> UpdateTenantUser(AddTenantUser addUserAc);
        Task<dynamic> UpdateUser(AddUserAc addUserAc);
        Task<dynamic> GetTenantUsers(int PageNo,int PageSize,int LoginId,int RoleId,int SiteId);
        Task<dynamic> GetUsers(int PageNo, int PageSize,int LoginId,int RoleId,int SiteId);
        Task<dynamic> GetTenantUserById(int Id);
        Task<RegisterUser> GetUserById(int Id);
        bool GetExistsTenantUser(AddTenantUser addUserAc);
        bool GetExistsUser(AddUserAc addUserAc);
        Task<dynamic> DeleteUser(int Id);
        Task<dynamic> UpdateProfile(UpdateRegisterUserAc objinput);
        Task<dynamic> UpdateProfileUploads(UpdateRegisterUserAc objinput); 
         Task<dynamic> UpdateUserProfile(UpdateRegisterUser objinput);
        Task<dynamic> AddTenant(UpdateRegisterUserAc model);
        //void Cancelwhitelistvehicle(int siteId, string vrm);
        void whitelistvehicle(int siteId, string vrm);
        bool GetTenant(UpdateRegisterUserAc addRegister);
       Task<dynamic> GetSearchUsers(int PageNo, int PageSize, string FirstName, string LastName, string Email,string SiteName, int LoginId, int RoleId, int SiteId);
        Task<dynamic> GetSearchTenants(int PageNo, int PageSize, string FirstName, string LastName, string Email, string MobileNumber,string SiteName, int SiteId, string VRM);
    }
}
