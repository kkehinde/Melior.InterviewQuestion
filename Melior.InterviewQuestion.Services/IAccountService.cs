using Melior.InterviewQuestion.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melior.InterviewQuestion.Services
{
    public  interface  IAccountService
    {
        Task<Account> LookupAccount(string? debtorAccountNumber, string dataStoreType);
        Task<MakePaymentResult> CheckAccountStateIsValid(MakePaymentRequest request, Account account);
        Task<bool> DeductPaymentFromAccount(decimal amount, Account account, string dataStoreType);
    }
}
