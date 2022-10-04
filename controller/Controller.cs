using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace BumpsData
{
    class AccountsImpl: AccountService.AccountServiceBase
    {
        // Server side handler of the GetEmployeeName RPCpublic override Task<EmployeeName> GetEmployeeName(EmployeeNameRequest request, ServerCallContext context)
        public AccountsImpl()
        {
            EmployeeData empData = new EmployeeData();
            return Task.FromResult( empData.GetEmployeeName(request) );
        }
    }
}