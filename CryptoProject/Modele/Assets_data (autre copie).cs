using System.Collections.Generic;

/*
 * This is the object with are use to Deserialize the json
 * This object has been created by a converter Json to class
 * and i have delete some arguments that i didn't use
 */


namespace CryptoProject.Modele
{
    
    public class Config
    {
        public string data { get; set; }
        public int data_points { get; set; }
        public string symbol { get; set; }
        public string interval { get; set; }
    }

    public class Usage
    {
        public int day { get; set; }
        public int month { get; set; }
    }

    public class TimeSery
    {
        public int asset_id { get; set; }
        public int time { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double volume { get; set; }
        public object market_cap { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public double price { get; set; }
        public double price_btc { get; set; }
        public long market_cap { get; set; }
        public double percent_change_24h { get; set; }
        public double percent_change_7d { get; set; }
        public double percent_change_30d { get; set; }
        public double volume_24h { get; set; }
        public string max_supply { get; set; }
        public int asset_id { get; set; }
        public int time { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double volume { get; set; }
        public double price_score { get; set; }
        public double close { get; set; }
        public List<TimeSery> timeSeries { get; set; }
    }

    public class Root
    {
        public Config config { get; set; }
        public Usage usage { get; set; }
        public List<Datum> data { get; set; }
    }
}
