namespace CurrencyConverter.Models
{
    public class CurrencyConfiguration
    {
        public string BaseUrl { get; set; } //https://www.bankofcanada.ca/valet
        public string GroupEndPoint { get; set; } //groups/FX_RATES_DAILY/json 
        
        //https://www.bankofcanada.ca/valet/groups/FX_RATES_DAILY/json
        //public string MyProperty { get; set; } 
        //
    }
}
