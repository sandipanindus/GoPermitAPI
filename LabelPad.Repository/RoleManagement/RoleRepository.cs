using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LabelPad.Repository.RoleManagement
{
    public class RoleRepository : IRoleRepository
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public RoleRepository(LabelPadDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<dynamic> AddRole(AddRoleAc addRole)
        {

            await _dbContext.Roles.AddAsync(new Role
            {
                Name = addRole.Name,
                Description = addRole.Description,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                IsActive = true,
                IsDeleted = false,
                CreatedBy = addRole.LoginId,
                CreatedOn = DateTime.Now
            });
            await _dbContext.SaveChangesAsync();
            return new { Message = "Role saved successfully" };
        }

        public async Task<dynamic> DeleteRole(int Id)
        {
            var registerusers =  _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.RoleId == Id).ToList();

            //if (registerusers != null)
            //{

            //    return new { Message = "No data found" };

            //}

            //else
            //{


                Role role = await _dbContext.Roles.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id).FirstOrDefaultAsync();
                if (role != null)
                {
                    role.IsDeleted = true;
                    role.IsActive = false;
                    role.UpdatedOn = DateTime.Now;
                    _dbContext.Roles.Update(role);
                    await _dbContext.SaveChangesAsync();
                    return new { Message = "Role deleted successfully" };

                }
                return new { Message = "Role have users can't delete " };
            //}
        }

        public async Task<Role> GetRoleById(int Id)
        {
            Role role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id);
            return role;
        }

        public async Task<dynamic> GetRoles(int PageNo,int PageSize,int LoginId,int RoleId)
        {
           
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            if (RoleId == 1)
            {
                if (PageSize == 0)
                {
                    List<Role> roles1 = await _dbContext.Roles.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
                    return roles1;
                }
                else
                 {
                    int totalitems = _dbContext.Roles.Where(x => x.IsActive == true && x.IsDeleted == false && x.CreatedBy == LoginId).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var roles = (from r in _dbContext.Roles
                                 where r.IsActive == true && r.IsDeleted == false
                                 select new
                                 {
                                     Name = r.Name,
                                     Id = r.Id,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage
                                 }).OrderByDescending(x => x.Id).ToList();

                    roles = roles.Skip(count2).Take(count1).ToList();
                    return roles;
                }
            }
            else
            {
                if (PageSize == 0)
                {
                    List<Role> roles1 = await _dbContext.Roles.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == RoleId).ToListAsync();
                    return roles1;
                }
                else
                {
                    int totalitems = _dbContext.Roles.Where(x => x.IsActive == true && x.IsDeleted == false && x.CreatedBy == LoginId && x.Id == RoleId).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var roles = (from r in _dbContext.Roles
                                 where r.IsActive == true && r.IsDeleted == false
                                 select new
                                 {
                                     Name = r.Name,
                                     Id = r.Id,
                                     TotalItem = totalitems,

                                 }).OrderByDescending(x => x.Id).ToList();

                    roles = roles.Skip(count2).Take(count1).ToList();
                    return roles;
                }
            }
        }

        public async Task<dynamic> UpdateRole(AddRoleAc addRole)
        {
            Role role = await _dbContext.Roles.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == addRole.Id).FirstOrDefaultAsync();
            if (role != null)
            {
                role.Name = addRole.Name;
                role.Description = addRole.Description;
                role.UpdatedBy = addRole.LoginId;
                role.UpdatedOn = DateTime.Now;
                _dbContext.Roles.Update(role);
                await _dbContext.SaveChangesAsync();
                return new { Message = "Role updated successfully" };
            }
            else
            {
                return new { Message = "No data Found" };
            }
        }
        public bool GetExistsRole(AddRoleAc addRoleAc)
        {

            Role role = _dbContext.Roles.FirstOrDefault(x => x.Name.Trim() == addRoleAc.Name.Trim() && x.IsActive == true && x.IsDeleted == false && x.CreatedBy==addRoleAc.LoginId);
            return (role != null);

        }


    }
}
