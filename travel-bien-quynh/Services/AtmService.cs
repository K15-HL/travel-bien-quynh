using travel_bien_quynh.Repositories;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Interface;

namespace travel_bien_quynh.Services
{
    public class AtmService : IAtmService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IAtmHistory _atmHistory;
        private readonly IAtmCheck _atmCheck;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly ILogWallet _logWallet;

        public AtmService(IAtmHistory atmHistory, IAtmCheck atmCheck, IUserRepository userRepository, IConfiguration configuration, ILogWallet logWallet)
        {
            _atmHistory = atmHistory;
            _atmCheck = atmCheck;
            _userRepository = userRepository;
            _config = configuration;
            _logWallet = logWallet;
        }
        public async void xuLyGiaoDich(AtmHistory history)
        {
            string username = string.Empty;
            var regex = new Regex(@"NAPTIEN\s+([a-zA-Z0-9]+)");
            var match = regex.Match(history.Username);
            if (match.Success)
            {
                username = match.Groups[1].Value;
            }

            if (history.Username.Contains("NAPTIEN", StringComparison.OrdinalIgnoreCase))
            {
                var atmcheck = await _atmCheck.GetAsyncByField("reference_number", history.referenceNumber);
                var user = await _userRepository.GetAsyncByField("username", username);
                if (atmcheck == null)
                {
                    AtmCheck atmCheck = new AtmCheck
                    {
                        referenceNumber = history.referenceNumber,
                    };

                    if (user != null)
                    {
                        user.Money += history.amountIn;
                        history.Username = username;
                        LogWallet logWallet = new LogWallet
                        {
                            UserName = user.Username,
                            Amount = history.amountIn,
                            Money = user.Money,
                            Description = "Nap Tien",
                            TransactionType = TransactionType.Credit,
                        };
                        try
                        {
                            await _userRepository.UpdateAsync(user.Id, user);
                            await _atmHistory.CreateAsync(history);
                            await _atmCheck.CreateAsync(atmCheck);
                            await _logWallet.CreateAsync(logWallet);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

        }
        public async Task<AtmHistory> CallBack()
        {
            var url = "https://my.sepay.vn/userapi/transactions/list";
            AtmHistory history = new AtmHistory();
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var apikey = _config["Atm:Key"];
                request.Headers.Add("Authorization", $"Bearer {apikey}");

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                if (apiResponse.Status == 200 && apiResponse.Messages.Success)
                {
                    foreach (var transaction in apiResponse.Transactions)
                    {
                        history = new AtmHistory
                        {

                            accountNumber = transaction.AccountNumber,
                            referenceNumber = transaction.ReferenceNumber,
                            amountIn = Double.Parse(transaction.AmountIn),
                            bankBrandName = transaction.BankBrandName,
                            Username = transaction.TransactionContent,
                            transactionDate = DateTime.Parse(transaction.TransactionDate)

                        };
                        xuLyGiaoDich(history);
                    }
                }
                else
                {
                    Console.WriteLine("API returned an error or unsuccessful message.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            return history;
        }
        public async Task<List<AtmHistory>> GetHistoryAtm()
        {
            var url = "https://my.sepay.vn/userapi/transactions/list";
            var atmHistories = new List<AtmHistory>();
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var apikey = _config["Atm:Key"];
                request.Headers.Add("Authorization", $"Bearer {apikey}");

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);

                if (apiResponse.Status == 200 && apiResponse.Messages.Success)
                {
                    foreach (var transaction in apiResponse.Transactions)
                    {
                        var history = new AtmHistory
                        {

                            accountNumber = transaction.AccountNumber,
                            referenceNumber = transaction.ReferenceNumber,
                            amountIn = Double.Parse(transaction.AmountIn),
                            bankBrandName = transaction.BankBrandName,
                            Username = transaction.TransactionContent,
                            transactionDate = DateTime.Parse(transaction.TransactionDate)

                        };
                        atmHistories.Add(history);


                    }
                }
                else
                {
                    Console.WriteLine("API returned an error or unsuccessful message.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            return atmHistories;
        }
    }
}


public interface IAtmService
{
    Task<AtmHistory> CallBack();
    Task<List<AtmHistory>> GetHistoryAtm();
    void xuLyGiaoDich(AtmHistory history);
}

public class Messages
{
    public bool Success { get; set; }
}

public class Transaction
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("bank_brand_name")]
    public string BankBrandName { get; set; }

    [JsonProperty("account_number")]
    public string AccountNumber { get; set; }

    [JsonProperty("transaction_date")]
    public string TransactionDate { get; set; }

    [JsonProperty("amount_out")]
    public string AmountOut { get; set; }

    [JsonProperty("amount_in")]
    public string AmountIn { get; set; }

    [JsonProperty("accumulated")]
    public string Accumulated { get; set; }

    [JsonProperty("transaction_content")]
    public string TransactionContent { get; set; }

    [JsonProperty("reference_number")]
    public string ReferenceNumber { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("sub_account")]
    public string SubAccount { get; set; }

    [JsonProperty("bank_account_id")]
    public string BankAccountId { get; set; }
}

public class ApiResponse
{
    public int Status { get; set; }
    public string Error { get; set; }
    public Messages Messages { get; set; }
    public List<Transaction> Transactions { get; set; }
}


