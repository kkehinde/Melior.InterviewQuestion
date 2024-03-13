using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Services
{
    public interface IPaymentService
    {
        Task<MakePaymentResult> MakePayment(MakePaymentRequest request);

    }

}
