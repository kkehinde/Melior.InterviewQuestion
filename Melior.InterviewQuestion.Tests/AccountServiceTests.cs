using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;

namespace Melior.InterviewQuestion.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;
        private readonly NameValueCollection _nameValueCollection;
        public AccountServiceTests()
        {
            _nameValueCollection = new NameValueCollection();
            _nameValueCollection.Add("DataStoreType", "Backup");
            _accountService = new AccountService();
        }

        [TestMethod]
        [DataRow("12345678", DisplayName = "DebtorAccountNumber is not Null")]
        public async Task Lookup_DebtorAccountNumber_WhenNotNull(string debtorAccountNumber)
        {
            string dataStoreType = _nameValueCollection["DataStoreType"]!;
            var result = await _accountService.LookupAccount(debtorAccountNumber, dataStoreType);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "DebtorAccountNumber is null or empty")]
        [DataRow(null, DisplayName = "Null check")]
        [DataRow("", DisplayName = "Empty check")]
        public async Task Lookup_DebtorAccountNumber_When_Is_Null(string debtorAccountNumber)
        {
            string dataStoreType = _nameValueCollection["DataStoreType"]!;
            var result = await _accountService.LookupAccount(debtorAccountNumber, dataStoreType);

        }

        [TestMethod]
        public async Task Deduct_Amount_From_Account_When_Amount_Greater_Than_0()
        {
            //this is valid only if there's no exception
            decimal amount = 2.0m;
            string dataStoreType = _nameValueCollection["DataStoreType"]!;
            Account account = new Account
            {
                AccountNumber = "44445555",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = 20.0m,
                Status = AccountStatus.InboundPaymentsOnly
            };
            var result = await _accountService.DeductPaymentFromAccount(amount, account, dataStoreType);
            Assert.IsTrue(result);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Account is null or empty")]
        public async Task Deduct_Amount_From_Account_When_Account_Is_Null()
        {
            //this is valid only if there's no exception
            decimal amount = 2.0m;
            string dataStoreType = _nameValueCollection["DataStoreType"]!;
            Account account = null!;
            var result = await _accountService.DeductPaymentFromAccount(amount, account, dataStoreType);
        }

        [TestMethod]
        [DataRow(PaymentScheme.Bacs, DisplayName = "Bacs check")]
        public async Task  Check_Bacs_Account_State_Is_Valid(PaymentScheme scheme)
        {
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 5.0m,
                CreditorAccountNumber = "12345678",
                DebtorAccountNumber = "87654321",
                PaymentDate = DateTime.UtcNow,
                PaymentScheme = scheme
            };
            Account account = new Account
            {
                AccountNumber = "44445555",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = 20.0m,
                Status = AccountStatus.InboundPaymentsOnly
            };
            var result = await _accountService.CheckAccountStateIsValid(request, account);
            Assert.IsTrue(result.Success);

        }

        [TestMethod]
        [DataRow(PaymentScheme.FasterPayments, DisplayName = "FasterPayments check")]
        public async Task Check_FasterPayments_Account_State_Is_Valid(PaymentScheme scheme)
        {
            //this is valid only if there's no exception
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 5.0m,
                CreditorAccountNumber = "12345678",
                DebtorAccountNumber = "12345678",
                PaymentDate = DateTime.UtcNow,
                PaymentScheme = scheme
            };
            Account account = new Account
            {
                AccountNumber = "12345678",
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 10.0m,
                Status = AccountStatus.Live
            };
            var result = await _accountService.CheckAccountStateIsValid(request, account);
            Assert.IsTrue(result.Success);

        }

        [TestMethod]
        [DataRow(PaymentScheme.Chaps, DisplayName = "Chaps check")]
        public async Task Check_Chaps_Account_State_Is_Valid(PaymentScheme scheme)
        {
            //this is valid only if there's no exception
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 5.0m,
                CreditorAccountNumber = "44445555",
                DebtorAccountNumber = "44445555",
                PaymentDate = DateTime.UtcNow,
                PaymentScheme = scheme
            };
            Account account = new Account
            {
                AccountNumber = "33335555",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 20.0m,
                Status = AccountStatus.Live
            };
            var result = await _accountService.CheckAccountStateIsValid(request, account);
            Assert.IsTrue(result.Success);

        }

    }
}