namespace StockApp.ViewModel
{
    public class FinhubResponseViewModel
    {
        public string StockName { get; set; }
        public string StockSymbol { get; set; }
        public double CurrentPrice { get; set; }
        public double HighPriceOfTheDay { get; set; }
        public double LowPriceOfTheDay { get;  set; }
        public double OpenPriceOfTheDay { get; set; }
        public double PreviousClosePrice { get; set; }
    }
}
