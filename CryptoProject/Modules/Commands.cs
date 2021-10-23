using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Net.Http;
using Newtonsoft.Json;
using CryptoProject.Modele;
using System.IO;

/*
 * This file contains all the different command make by the bot
 */


namespace CryptoProject.Modules
{
    public class Commands : ModuleBase
    {
        private static string key;
        private static HttpClient client;

        private const string url_main = "https://api.lunarcrush.com/v2?";

        //This commands is just a test to see if the bot is in the discord server

        [Command("ping")]
        public async Task Hello()
        {
            await Context.Channel.SendMessageAsync("Pong");
        }

        //This command allowing to inform the Api lunarcrush key

        [Command("key")]
        public async Task Key(string pkey)
        {
            client = new HttpClient();
            key = pkey;
            await Context.Channel.SendMessageAsync("Votre key à été enregistrée :" + key);
        }

        //This command display the price of a crypto

        [Command("price")]
        public async Task AskPrice(string crypto)
        {
            Root data = Connection_api(key, crypto);
            await Context.Channel.SendMessageAsync(data.data[0].price.ToString());
        }

        //This commande compare 2 cryptocurrencies

        [Command("compare")]
        public async Task Compare([Remainder] string cryptogene)
        {
            string[] crypto = cryptogene.Split(" ");
            Root data1 = Connection_api(key, crypto[0]);
            Root data2 = Connection_api(key, crypto[1]);

            if(data1.data[0].price > data2.data[0].price)
            {
                await Context.Channel.SendMessageAsync(crypto[0] + " est la plus grande");
            }
            else
            {
                await Context.Channel.SendMessageAsync(crypto[1] + " est la plus grande");
            }
        }

        //This command allowing to calculate a new price of the 1st crypto with the market cap of the other crypto

        [Command("changemarketcap")]
        public async Task Changemarketcap([Remainder] string cryptogene)
        {
            string[] crypto = cryptogene.Split(" ");
            Root data1 = Connection_api(key, crypto[0]);
            Root data2 = Connection_api(key, crypto[1]);


            var marketcap_data1 = data1.data[0].market_cap;
            var marketcap_data2 = data2.data[0].market_cap;

            var coefficient = marketcap_data2 / (double)marketcap_data1;
            var new_price = data1.data[0].price * coefficient;

            await Context.Channel.SendMessageAsync("prie of " + crypto[0] + "with the market value of " + crypto[1] + ": \n new price :" + new_price.ToString() + "coeff:" + coefficient.ToString());
        }

        //This command allowing to calculate the mean of a crypto on 1 years

        [Command("meanprice1Y")]
        public async Task Mean(string crypto)
        {
            Root data = Connection_api_Condition(key, crypto);
            double total = 0;


            for (var i = 0; i < 365; i++)
            {
                var opendata = data.data[0].timeSeries[i].open;
                total += opendata;

            }
            double mean = (double)total / 365;

            await Context.Channel.SendMessageAsync("The mean of " + crypto + " is : " + mean.ToString());

        }

        //This command allowing to calculate the pourcentage of evolution of a crypto on 1 years

        [Command("evolutionprice1Y")]
        public async Task Evolution(string crypto)
        {
            Root data = Connection_api_Condition(key, crypto);

            double first_open = data.data[0].timeSeries[0].open;
            double last_open = data.data[0].timeSeries[364].open;

            var pourcentage = ((last_open - first_open) / first_open) * 100;
            await Context.Channel.SendMessageAsync("The pourcentage evolution of " + crypto + " is : " + pourcentage.ToString() +"%");

        }

        // This command export the price of a crypto in txt file

        [Command("exporttxtprice")]
        public async Task Export(string crypto)
        {
            Root data = Connection_api_Condition(key, crypto);
            string fileName = "C:\\"+crypto + "price.txt";
            // Vérifiez si le fichier existe déjà. Si oui, supprimez-le.    
            StreamWriter sw = new StreamWriter(fileName);
            //Write a line of text

            for (var i = 0; i < 365; i++)
            {
                var pricedata = data.data[0].timeSeries[i].open;
                sw.WriteLine(pricedata.ToString());


            }
            sw.Close();
            await Context.Channel.SendFileAsync(fileName);
        }

        //Function allowing the connection to the api  

        public Root Connection_api(string key, string crypto)
        {

            string url = url_main + "data=assets" + "&key=" + key + "&symbol=" + crypto;
            var json = GetGlobalDataAsync(url).GetAwaiter().GetResult();
            Console.WriteLine(json);
            Root data = JsonConvert.DeserializeObject<Root>(json);
            return data;
        }

        //Function allowing the connection to the api with condition (here one datat per days)

        public Root Connection_api_Condition(string key, string crypto)
        {

            string url = url_main + "data=assets" + "&key=" + key + "&symbol=" + crypto+ "&data_points=365&interval=day";
            var json = GetGlobalDataAsync(url).GetAwaiter().GetResult();
            Console.WriteLine(json);
            Root data = JsonConvert.DeserializeObject<Root>(json);
            return data;
        }

        //Realize the connection and check if the connection is etablished

        public async Task<string> GetGlobalDataAsync(string url)
           {
                var data = string.Empty;
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                }
                return data;
            }



        

    }

}
