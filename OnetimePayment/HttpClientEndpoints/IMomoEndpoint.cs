using Refit;

namespace OnetimePayment;

public interface IMomoEndpoint
{
    [Post("/v2/gateway/api/create")]
    Task CreateOneTimePayment([Body] MomoOneTimePaymentRequest request);
}
