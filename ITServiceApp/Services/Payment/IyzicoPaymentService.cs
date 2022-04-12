using AutoMapper;
using CSG.Models.Payment;
using ITServiceApp.Models.Services.Payment;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using MUsefulMethods;



namespace ITServiceApp.Services.Payment
{
    public class IyzicoPaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IyzicoPaymentOptions _options;
        private readonly IMapper _mapper;

        public IyzicoPaymentService(IConfiguration configuration,IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
            var section = _configuration.GetSection(IyzicoPaymentOptions.Key);
            _options = new IyzicoPaymentOptions()
            {
                ApiKey = section["ApiKey"],
                SecretKey = section["SecretKey"],
                BaseUrl = section["BaseUrl"],
                ThreedsCallbackUrl = section["ThreedsCallbackUrl"],
            };

        }
        public InstallmentModel CheckInstallments(string binNumber, decimal price)
        {
            var conversationId = GenerateConversationId();
            var request = new RetrieveInstallmentInfoRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = conversationId,
                BinNumber = binNumber,
                Price = price.ToString(new CultureInfo("en-US")),
            };

            var result=InstallmentInfo.Retrieve(request,_options);
            if (result.Status == "failure")
            {
                throw new Exception(result.ErrorMessage);
            }

            if (result.ConversationId != conversationId)
            {
                throw new Exception("Hatalı istek oluturuldu");
            }
            InstallmentModel resultModel=_mapper.Map<InstallmentModel>(result.InstallmentDetails[0]);

            Console.WriteLine();
            return resultModel;

        }

        public PaymentResponseModel Pay(PaymentModel model)
        {
            return null;
        }
        private string GenerateConversationId()
        {
            return StringHelpers.GenerateUniqueCode();
        }
    }
}
