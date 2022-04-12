using CSG.Models.Payment;
using ITServiceApp.Models.Services.Payment;

namespace ITServiceApp.Services.Payment
{
    public class IyzicoPaymentService : IPaymentService
    {
        public InstallmentModel CheckInstallments(string binNumber, decimal price)
        {
            throw new System.NotImplementedException();
        }

        public PaymentResponseModel Pay(PaymentModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
