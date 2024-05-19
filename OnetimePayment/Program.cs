// using System.Text.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OnetimePayment;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
    (options) =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
    }
);

var momoApiBaseUrl = builder.Configuration.GetSection("MomoApi").GetValue<string>("BaseUrl") ?? throw new Exception("Momo Base Url Not Found");

builder
    .Services
    .AddRefitClient<IMomoEndpoint>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(momoApiBaseUrl));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//==========================================================================================================================//

app.MapPost("/payment", async () =>
{
    const string secretKey = "at67qH6mk8w5Y1nAyMoYKMWACiEi2bsa";
    const string accessKey = "klm05TvNBzhg7h7j";

    var onetimePaymentRequest = new MomoOneTimePaymentRequest()
    {
        partnerCode = "MOMOBKUN20180529",
        requestId = Guid.NewGuid().ToString(),
        amount = 150000,
        orderId = $"MM-{Guid.NewGuid()}",
        orderInfo = $"Thanh Toan Subscription Phim Ngay ${DateTime.UtcNow}",
        redirectUrl = "http://localhost:3000/momo-return",
        ipnUrl = "http://localhost:3000/payment/api/momo-ipn",
        requestType = "captureWallet",
        extraData = string.Empty,
        lang = "vi"
    };

    onetimePaymentRequest.MakeSignature(accessKey, secretKey);

    var (isSuccess, response) = await onetimePaymentRequest.GetLink(momoApiBaseUrl);

    return response;

    // Using with Refit

    // var client = RestService.For<IMomoEndpoint>(momoApiBaseUrl, new RefitSettings(new NewtonsoftJsonContentSerializer(
    // new JsonSerializerSettings()
    // {
    //     ContractResolver = new CamelCasePropertyNamesContractResolver(),
    //     Converters = { new StringEnumConverter() }
    // })));

    // try
    // {
    //     await RestService.For<IMomoEndpoint>(momoApiBaseUrl).CreateOneTimePayment(onetimePaymentRequest);
    //     // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));

    //     return Results.Ok("OK");
    // }
    // catch (ApiException ex)
    // {
    //     // Extract the details of the error
    //     var errors = await ex.GetContentAsAsync<Dictionary<string, string>>();

    //     // Combine the errors into a string
    //     var message = string.Join("; ", errors!.Values);

    //     // Throw a normal exception
    //     throw new Exception(message);
    // }
})
.WithName("Payment")
.WithOpenApi();

app.MapGet("/momo-return", async ([AsParameters] MomoOneTimePaymentResultRequest response) =>
{
    // string returnUrl = string.Empty;
    // var returnModel = new PaymentReturnDtos();
    // var processResult = await mediator.Send(response.Adapt<ProcessMomoPaymentReturn>());

    // if (processResult.Success)
    // {
    //     returnModel = processResult.Data.Item1 as PaymentReturnDtos;
    //     returnUrl = processResult.Data.Item2 as string;
    // }

    // if (returnUrl.EndsWith("/"))
    //     returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
    // return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");

    var x = response;
    var y = response.signature;

    Console.WriteLine($"Momo-Return......: {System.Text.Json.JsonSerializer.Serialize(response)}");

    return "";
});

app.Run("http://localhost:3000");
