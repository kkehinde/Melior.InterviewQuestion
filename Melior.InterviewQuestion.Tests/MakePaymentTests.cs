using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Configuration;

namespace Melior.InterviewQuestion.Tests
{
    [TestClass]
    public class MakePaymentTests
    {
        private readonly IAccountService _accountService;
        private readonly NameValueCollection _nameValueCollection;
        public MakePaymentTests() {
            _nameValueCollection = new NameValueCollection();
            _nameValueCollection.Add("DataStoreType", "Backup");
            _accountService = new AccountService();
        }
       
        [TestMethod]
        public async Task   MakePayment_WhenRequestNotNull()
        {
            MakePaymentRequest request = new MakePaymentRequest
            {
                Amount = 5.0m,
                CreditorAccountNumber = "12345678",
                DebtorAccountNumber = "12345678",
                PaymentDate = DateTime.UtcNow,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var paymentService = new PaymentService (_accountService, _nameValueCollection);

            var  result = await paymentService.MakePayment(request);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "MakePaymentRequest object is null")]
        public async Task MakePayment_WhenRequestIsNull()
        {
            MakePaymentRequest request = null!;
            var paymentService = new PaymentService(_accountService, _nameValueCollection);
            var result = await paymentService.MakePayment(request);
        }

    }
}