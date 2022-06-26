using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CurrencyConverter.Controllers.Resources;
using CurrencyConverter.Models;
using CurrencyConverter.Models.CanadaBank;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CurrencyConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangesController : ControllerBase
    {
        private readonly CurrencyConfiguration currencyConfiguration;
        public ExchangesController(IOptions<CurrencyConfiguration> options)
        {
            currencyConfiguration = options.Value;
        }

        [HttpPost]
        public async Task<ActionResult> GetExchangeRate([FromBody]Request request)
        {
            string[] ValidCurrencies = { "CAD" ,"USD", "EUR", "JPY", "GBP", "AUD", "CHF", "CNY", "HKD", "MXN", "INR" };

            if (!ValidCurrencies.Contains(request.SourceCurrency.ToUpperInvariant()))
            {
                return BadRequest("Invalid source currency");
            }

            if (!ValidCurrencies.Contains(request.TargetCurrency.ToUpperInvariant()))
            {
                return BadRequest("Invalid target currency");
            }

            if (string.IsNullOrWhiteSpace(request.Date))
            {
                request.Date = DateTime.Now.ToString("yyyy-MM-dd");
            }

            var result = await GetConversionRateAsync(request.SourceCurrency, request.TargetCurrency, request.Date);

            return Ok(result);

        }

        [HttpGet("groups")]
        public async Task<ActionResult> GetGroupsAsync()
        {
            try
            {
                await GetConversionRateAsync("CAD", "USD", "2022-01-23");
                //GroupResult groupResult;
                //using (var httpClient = new HttpClient())
                //{
                //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //    //httpClient.BaseAddress = new Uri(currencyConfiguration.BaseUrl);
                //    string url = currencyConfiguration.BaseUrl + "/" + currencyConfiguration.GroupEndPoint;
                //    using (var response = await httpClient.GetAsync(url))
                //    {
                //        string apiResponse = await response.Content.ReadAsStringAsync();
                //        groupResult = JsonConvert.DeserializeObject<GroupResult>(apiResponse);
                //    }
                //}
                return Ok("Get data successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        private async Task<decimal?> GetConversionRateAsync(string sourceCurrency, string targetCurreny, string currencyDate)
        {
            int tryPreviousDate = 4;
            decimal? conversionRate = null;
            CurrencyRate currencyRate;
            string key = $"FX{sourceCurrency.ToUpperInvariant()}{targetCurreny.ToUpperInvariant()}";
            string url = $"https://www.bankofcanada.ca/valet/observations/{key}/json";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = apiResponse.Replace(key, "Currency");
                    currencyRate = JsonConvert.DeserializeObject<CurrencyRate>(apiResponse);
                    var observation = currencyRate.Observations.FirstOrDefault(q => q.d == currencyDate);

                    string previousDate = currencyDate;
                    while (observation == null && tryPreviousDate > 0)
                    {
                        string dateFormat = "yyyy-MM-dd";
                        previousDate = DateTime.ParseExact(previousDate, "yyyy-MM-dd", 
                                                    CultureInfo.InvariantCulture).AddDays(-1).ToString(dateFormat);
                        observation = currencyRate.Observations.FirstOrDefault(q => q.d == previousDate);
                        tryPreviousDate = tryPreviousDate - 1;

                    }

                    if (observation != null)
                    {
                        conversionRate = Convert.ToDecimal(observation.Currency.v);
                    }

                    //if (observation != null)
                    //{
                    //    conversionRate = Convert.ToDecimal(observation.Currency.v);
                    //}
                    //else
                    //{
                    //    string dateFormat = "yyyy-MM-dd";
                    //    string previousDate = DateTime.ParseExact(currencyDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString(dateFormat);
                    //    observation = currencyRate.Observations.FirstOrDefault(q => q.d == currencyDate);

                    //}
                }
            }
            return conversionRate;
        }
    }
}