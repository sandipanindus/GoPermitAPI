using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.RegisterManagement
{
    public class RegisterRepository:IRegisterRepository
    {
        private readonly LabelPadDbContext _dbContext;
        public RegisterRepository(LabelPadDbContext dbContext)
        {
            _dbContext = dbContext;
        }
      
        public async Task<dynamic> AddRegisterUser(AddRegisterUserAc addRegister)
        {
            //Role role = new Role();
            //role.IsActive = true;
            //role.IsDeleted = false;
            //role.Name = "Admin";
            //role.Description = "";
            //role.CreatedBy = 1;
            //role.CreatedOn = DateTime.Now;
            //role.ConcurrencyStamp = Guid.NewGuid().ToString();
            //await _dbContext.Roles.AddAsync(role);
            //await _dbContext.SaveChangesAsync();
           // string domainname = _dbContext.DomainNames.FirstOrDefault(x => x.IsActive == true && x.IsDeleted == false).Name;
            RegisterUser register = new RegisterUser();
            register.FirstName = addRegister.FirstName;
            register.LastName = addRegister.LastName;
           // register.OrganisationName = addRegister.OrganisationName;
            register.Email = addRegister.Email;
           // register.ContactNumber = addRegister.ContactNumber;
            register.Password = Utilities.Encrypt(addRegister.Password);
          //  register.Subdomain = addRegister.Subdomain.Trim() + domainname;
            register.EmailCode = addRegister.EmailCode;
            register.State = addRegister.State;
            register.City = addRegister.City;
            register.Address = addRegister.Address;
            register.ZipCode = addRegister.Zipcode;
            register.MobileNumber = addRegister.MobileNumber;
            register.ParkingBay = addRegister.ParkingBay;
            register.SiteId = Convert.ToInt32(addRegister.SiteId);
            register.RoleId = 3;
            register.ParentId =0;
            register.IsActive = true;
            register.IsDeleted = false;
            register.CreatedOn = DateTime.Now;
          
           await _dbContext.RegisterUsers.AddAsync(register);
            await _dbContext.SaveChangesAsync();
            RegisterUser user = _dbContext.RegisterUsers.Where(x => x.Id == register.Id).FirstOrDefault();
            user.CreatedBy = register.Id;
            user.ClientId = register.Id;
             _dbContext.RegisterUsers.Update(user);
            await _dbContext.SaveChangesAsync();

          //  Role userrole = _dbContext.Roles.Where(x => x.Id == role.Id).FirstOrDefault();
          //  userrole.CreatedBy = register.Id;
          //  _dbContext.Roles.Update(userrole);
         //   await _dbContext.SaveChangesAsync();
            return new { Message = "User added successfully" };
        }
        
        public bool GetRegisterUser(AddRegisterUserAc addRegister)
        {
          
                RegisterUser user = _dbContext.RegisterUsers.FirstOrDefault(x => x.Email == addRegister.Email || x.MobileNumber == addRegister.MobileNumber && x.IsActive == true && x.IsDeleted == false);
                return (user != null);
          
        }
    }
}
