using CSG.Models.Payment;

namespace ITServiceApp.Models.Services.Payment
{
    public interface IPaymentService
    {
         InstallmentModel CheckInstallments(string binNumber, decimal price);
         PaymentResponseModel Pay(PaymentModel model);
    }
}
