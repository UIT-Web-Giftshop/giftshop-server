using Braintree;

namespace API.Config
{
    public class BraintreeConfig : IBraintreeConfig
    {
        public string Environment { get; set; } = "sandbox";
        public string MerchantId { get; set; } = "h35cvz8tt7j4xbpk";
        public string PublicKey { get; set; } = "b5ysqyjjb64qhttt";
        public string PrivateKey { get; set; } = "fda29958ef5436de70607069fac6baf9";
        private IBraintreeGateway BraintreeGateway { get; set; }

        public IBraintreeGateway GetGateway()
        {
            return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
        }

        public IBraintreeGateway CreateGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }
    }
}