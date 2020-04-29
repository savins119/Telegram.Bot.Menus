using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Menus;
using Telegram.Bot.Menus.Items;
using Telegram.Bot.Types.Enums;

namespace TestApp
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("You bot token");

        private static TelegramMenu menu;
        
        static void Main(string[] args)
        {
            MenuItemWithSubItems mainMenu = new MenuItemWithSubItems("/showMainMenu", "Welcome to main menu!", true, 3);
            mainMenu.AddSubItem(new MenuItemWithSubItems("/menuWithSubItems1", "Welcome to 'menuWithSubItems1' menu!")
                .AddSubItems(new MenuItemBase[]
                {
                    new MenuItemWithAction("/sub1Action1",
                        async (chat) => { await bot.SendTextMessageAsync(chat.Id, "sub1Action1 pressed"); }),
                    new MenuItemWithAction("/sub1Action2",
                        async (chat) => { await bot.SendTextMessageAsync(chat.Id, "sub1Action2 pressed"); }),
                    new MenuItemWithSubItems("/subMenuWithSubItems1", "Welcome to 'subMenuWithSubItems1' menu!")
                        .AddSubItem(new MenuItemWithAction("/subSub1Action1",
                            async (chat) => { await bot.SendTextMessageAsync(chat.Id, "subSub1Action1 pressed"); }))
                        .AddSubItem(new MenuItemBack())
                        .AddSubItem(new MenuItemToMainMenu()),
                    new MenuItemToMainMenu(),
                }));
            
            mainMenu.AddSubItem(new MenuItemWithAction("/action2", async(chat) =>
            {
                await bot.SendTextMessageAsync(chat.Id, "action2 pressed");
            }));
            mainMenu.AddSubItem(new MenuItemWithAction("/action3", async(chat) =>
            {
                await bot.SendTextMessageAsync(chat.Id, "action3 pressed");
            }));
            mainMenu.AddSubItem(new MenuItemWithAction("/action4", async (chat) =>
            {
                await bot.SendTextMessageAsync(chat.Id, "action4 pressed");
            }));
            mainMenu.AddSubItem(new MenuItemWithAction("/HideMainMenu",
                (chat) =>
                {
                    menu.HideMainMenu(chat.Id, "Main menu closed. To show main menu send '/start'"); 
                }));
            
            
            menu = new TelegramMenu(bot, mainMenu);
            
            bot.OnMessage += BotOnOnMessage; 
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private static void BotOnOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
            {
                if (e.Message.Text == "/start")
                {
                    menu.ShowMainMenu(e.Message.Chat.Id);
                }
                else
                {
                    menu.ProcessInputMessage(e);
                }
            }
        }
    }
}
