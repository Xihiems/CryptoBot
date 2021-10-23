using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

/*
 * This file allowing to launch the discord bot
 */


namespace CryptoProject
{
    class Program
    {
        private DiscordSocketClient clientdiscord;
        private CommandService commands;

        private const string url_main = "https://api.lunarcrush.com/v2?";

        //This function allowing the display of the command, the login with the discord token, and the launch

        public async Task RunBotAsync()
        {
            clientdiscord = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel=LogSeverity.Debug
            });

            commands = new CommandService();
            clientdiscord.Log += Log;

            clientdiscord.Ready += () =>
            {
                Console.WriteLine("je suis prêt");
                return Task.CompletedTask;
            };
            await InstallCommandAsync();
            await clientdiscord.LoginAsync(TokenType.Bot, "Insérer le bon token");
            await clientdiscord.StartAsync();
            await Task.Delay(-1);
            
        }

        //Print a message

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        //Allowing to use the different commande created

        public async Task InstallCommandAsync()
        {
            clientdiscord.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        //Check if the message send by the discord user is a command

        private async Task HandleCommandAsync(SocketMessage pmessage)
        {
            var message = (SocketUserMessage)pmessage;

            if (message == null) return;

            int argPos = 0;

            if (!message.HasCharPrefix('!', ref argPos)) return;

            var context = new SocketCommandContext(clientdiscord, message);
            var result = await commands.ExecuteAsync(context, argPos, null);

            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        //Main function

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();    
          
    }
}
