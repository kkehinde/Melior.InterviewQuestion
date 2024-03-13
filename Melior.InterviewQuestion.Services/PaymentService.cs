using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Types;
using System.Collections.Specialized;
using System.Configuration;

namespace Melior.InterviewQuestion.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountService _accountService;
        private readonly NameValueCollection _nameValueCollection;
        private string _dataStoreType;

        public PaymentService(IAccountService accountService, NameValueCollection nameValueCollection)
        {
            _accountService = accountService;
            _nameValueCollection = nameValueCollection;
            _dataStoreType = _nameValueCollection["DataStoreType"]!;

        }
        /// <summary>
        /// MakePayment method with parameter MakePaymentRequest request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<MakePaymentResult> MakePayment(MakePaymentRequest request)
        {
            if (request == null)
                throw new NullReferenceException(nameof(MakePaymentRequest));

            //lookup account using config data
            Account account = await _accountService.LookupAccount(request.DebtorAccountNumber, _dataStoreType);

            if (account == null)
                throw new NullReferenceException(nameof(Account));

            //check if the account is in the valid state
            var result = await _accountService.CheckAccountStateIsValid(request, account);

            if (result.Success)
            {
                //deduct payment from the account 
                await _accountService.DeductPaymentFromAccount(request.Amount, account, _dataStoreType);
            }

            return await Task.FromResult(result);
        }


    }
}
