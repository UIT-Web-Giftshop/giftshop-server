using Braintree;

namespace API.Config
{
    public interface IBraintreeConfig
    {
        IBraintreeGateway GetGateway();
        IBraintreeGateway CreateGateway();
    }
}