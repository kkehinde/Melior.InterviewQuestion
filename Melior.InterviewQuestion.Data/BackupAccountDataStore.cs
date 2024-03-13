using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Data
{
    public class BackupAccountDataStore
    {
        public List<Account> accounts = new List<Account>()
        {
            new Account
            {
                 AccountNumber = "12345678",
                  AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                   Balance = 10.0m,
                    Status = AccountStatus.Live
            },
            new Account
            {
                 AccountNumber = "44445678",
                  AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                   Balance = 20.0m,
                    Status = AccountStatus.Live
            },
            new Account
            {
                 AccountNumber = "33335555",
                  AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                   Balance = 20.0m,
                    Status = AccountStatus.Live
            },
        };


        public Account GetAccount(string? accountNumber)
        {
            // Access backup data base to retrieve account, code removed for brevity 
            var account  = accounts.FirstOrDefault(c=>c.AccountNumber == accountNumber);
            return account!;
        }

        public void UpdateAccount(Account account)
        {
            // Update account in backup database, code removed for brevity
        }



    }
}
