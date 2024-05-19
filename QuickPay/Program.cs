using System;
using System.Threading.Tasks;
using System.Net.Http;
using QuickPay.model;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace QuickPay
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();

            string accessKey = "F8BBA842ECF85";
            string secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";

            QuickPayResquest request = new QuickPayResquest();
            request.orderInfo = "pay with MoMo";
            request.partnerCode = "MOMO";
            request.ipnUrl = "https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b";
            request.redirectUrl = "https://webhook.site/b3088a6a-2d17-4f8d-a383-71389a6c600b";
            request.paymentCode = "T8Qii53fAXyUftPV3m9ysyRhEanUs9KlOPfHgpMR0ON50U10Bh+vZdpJU7VY4z+Z2y77fJHkoDc69scwwzLuW5MzeUKTwPo3ZMaB29imm6YulqnWfTkgzqRaion+EuD7FN9wZ4aXE1+mRt0gHsU193y+yxtRgpmY7SDMU9hCKoQtYyHsfFR5FUAOAKMdw2fzQqpToei3rnaYvZuYaxolprm9+/+WIETnPUDlxCYOiw7vPeaaYQQH0BF0TxyU3zu36ODx980rJvPAgtJzH1gUrlxcSS1HQeQ9ZaVM1eOK/jl8KJm6ijOwErHGbgf/hVymUQG65rHU2MWz9U8QUjvDWA==";
            request.amount = 50000;
            request.orderId = request.partnerCode;
            request.requestId = request.orderId;
            request.extraData = "";
            request.partnerName = "Test";
            request.orderGroupId = "";
            request.autoCapture = true;
            request.lang = "vi";

            //before sign HMAC SHA256 with format
            //accessKey=$accessKey&amount=$amount&extraData=$extraData&ipnUrl=$ipnUrl&orderId=$orderId&orderInfo=$orderInfo&partnerCode=$partnerCode&redirectUrl=$redirectUrl&requestId=$requestId&requestType=$requestType
            var rawSignature = "accessKey=" + accessKey + "&amount=" + request.amount + "&extraData=" + request.extraData + "&orderId=" + request.orderId + "&orderInfo=" + request.orderInfo + "&partnerCode=" + request.partnerCode + "&paymentCode=" + request.paymentCode + "&requestId=" + request.requestId;
            //puts raw signature

            // var rawSignature = "accessKey=" + accessKey + "&amount=" + request.amount + "&extraData=" + request.extraData + "&orderId=" + request.orderId + "&orderInfo=" + request.orderInfo + "&partnerCode=" + request.partnerCode + "&paymentCode=" + request.paymentCode + "&requestId=" + request.requestId;
            request.signature = getSignature(rawSignature, secretKey);

            StringContent httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var quickPayResponse = await client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/pos", httpContent);
            var contents = quickPayResponse.Content.ReadAsStringAsync().Result;
            System.Console.WriteLine(contents + "");
        }

        private static String getSignature(String text, String key)
        {
            // change according to your needs, an UTF8Encoding
            // could be more suitable in certain situations
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

    }
}
