using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.OperatorManagement
{
    public interface IOperatorRepository
    {
        Task<IEnumerable<OperatorDetails>> GetAllOperators();
        Task<OperatorDetails> GetOperatorById(int id);
        Task<OperatorDetails> CreateOperator(OperatorDetails operatorDetails);
        Task<OperatorDetails> UpdateOperator(OperatorDetails operatorDetails);
        Task<dynamic> DeleteOperator(int id);
    }
}
