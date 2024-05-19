using System.Text.Json;

public class MomoOneTimePaymentRequest
{
    // public MomoOneTimePaymentRequest(string partnerCode, string requestId,
    //     long amount, string orderId, string orderInfo, string redirectUrl,
    //     string ipnUrl, string requestType, string extraData, string lang = "vi")
    // {
    //     this.partnerCode = partnerCode;
    //     this.requestId = requestId;
    //     this.amount = amount;
    //     this.orderId = orderId;
    //     this.orderInfo = orderInfo;
    //     this.redirectUrl = redirectUrl;
    //     this.ipnUrl = ipnUrl;
    //     this.requestType = requestType;
    //     this.extraData = extraData;
    //     this.lang = lang;
    // }
    public string partnerCode { get; set; } = string.Empty;
    public string requestId { get; set; } = string.Empty;
    public long amount { get; set; }
    public string orderId { get; set; } = string.Empty;
    public string orderInfo { get; set; } = string.Empty;
    public string redirectUrl { get; set; } = string.Empty;
    public string ipnUrl { get; set; } = string.Empty;
    public string requestType { get; set; } = string.Empty;
    public string extraData { get; set; } = string.Empty;
    public string lang { get; set; } = string.Empty;
    public string signature { get; set; } = string.Empty;

    public void MakeSignature(string accessKey, string secretKey)
    {
        var rawHash = "accessKey=" + accessKey +
            "&amount=" + amount +
            "&extraData=" + extraData +
            "&ipnUrl=" + ipnUrl +
            "&orderId=" + orderId +
            "&orderInfo=" + orderInfo +
            "&partnerCode=" + partnerCode +
            "&redirectUrl=" + redirectUrl +
            "&requestId=" + requestId +
            "&requestType=" + requestType;

        signature = HashHelper.HmacSHA256(rawHash, secretKey);
    }

    public async Task<(bool, MomoResponse?)> GetLink(string paymentUrl)
    {
        using HttpClient client = new();

        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        StringContent httpContent = new(
            JsonSerializer.Serialize(this, options: options),
            System.Text.Encoding.UTF8,
            "application/json");

        var quickPayResponse = await client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);

        if (quickPayResponse.IsSuccessStatusCode)
        {
            var contents = quickPayResponse.Content;

            var results = contents.ReadAsStringAsync().Result;

            // Parse response to MomoResponse class
            var responseData = JsonSerializer
                .Deserialize<MomoResponse>(results)
                ?? throw new Exception("Deserialize payment link response failed");

            if (responseData.resultCode == 0)
            {
                return (true, responseData);
            }
            else
            {
                return (false, responseData);
            }
        }
        else
        {
            return (false, null);
        }
    }
}