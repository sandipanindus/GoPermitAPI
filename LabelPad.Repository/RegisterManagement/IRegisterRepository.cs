using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.RegisterManagement
{
    public interface IRegisterRepository
    {
        Task<dynamic> AddRegisterUser(AddRegisterUserAc addRegister);

        bool GetRegisterUser(AddRegisterUserAc addRegister);

       
    }
}
