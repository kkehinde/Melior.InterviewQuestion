using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Melior.InterviewQuestion.Services
{
    public class AccountService : IAccountService
    {
        public async Task<MakePaymentResult> CheckAccountStateIsValid(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult();
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }
                    else
                    {
                        result.Success = true;
                    }
                    break;
            }

            return await Task.FromResult(result);
        }
        public async Task<bool> DeductPaymentFromAccount(decimal amount, Account account, string dataStoreType)
        {
            if (account == null)
                throw new NullReferenceException(nameof(Account));

            if (amount > 0)
            {
                account.Balance -= amount;

                if (dataStoreType == "Backup")
                {
                    var accountDataStore = new BackupAccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
                else
                {
                    var accountDataStore = new AccountDataStore();
                    accountDataStore.UpdateAccount(account);
                }
                return await Task.FromResult(true);

            }
            else 
            {
                return await Task.FromResult(false);
            }
        }
        public async Task<Account> LookupAccount(string? debtorAccountNumber, string dataStoreType)
        {
            Account account = null!;

            if (string.IsNullOrEmpty(debtorAccountNumber))
                throw new NullReferenceException(nameof(debtorAccountNumber));

            if (dataStoreType == "Backup")
            {
                var accountDataStore = new BackupAccountDataStore();
                account = accountDataStore.GetAccount(debtorAccountNumber);
            }
            else
            {
                var accountDataStore = new AccountDataStore();
                account = accountDataStore.GetAccount(debtorAccountNumber);
            }

            return await Task.FromResult(account);
        }
    }
}
