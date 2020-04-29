using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Menus.Items;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Menus
{
    public class TelegramMenu
    {
        private readonly TelegramBotClient BotClient;
        private readonly MenuItemWithSubItems MainMenu;

        private Dictionary<string, MenuItemBase> allItems = new Dictionary<string, MenuItemBase>();
        
        public TelegramMenu(TelegramBotClient botClient, MenuItemWithSubItems mainMenu)
        {
            this.BotClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            this.MainMenu = mainMenu ?? throw new ArgumentNullException(nameof(mainMenu));
            this.ValidateAndInit(mainMenu);
        }

        private void RegisterCommand(MenuItemBase item)
        {
            if (this.allItems.ContainsKey(item.CommandText))
            {
                throw new Exception($"Command '{item.CommandText}' already registered.");
            }
                
            this.allItems.Add(item.CommandText, item);
        }
        
        private void ValidateAndInit(MenuItemWithSubItems menu)
        {
            this.RegisterCommand(menu);
            
            foreach (MenuItemBase item in menu.SubItems)
            {
                if (item is MenuItemToMainMenu)
                {
                    continue;
                }

                if (item is MenuItemBack)
                {
                    continue;
                }

                if (item is MenuItemWithSubItems menuItemWithSubItems)
                {
                    this.ValidateAndInit(menuItemWithSubItems);
                }
                else
                {
                    this.RegisterCommand(item);
                }
            }
        }
        
        public async void ShowMainMenu(long chatId)
        {
            await this.BotClient.SendTextMessageAsync(chatId, this.MainMenu.MenuEnterMessage, ParseMode.Markdown, replyMarkup: this.FormatReplyKeyboardMarkup(this.MainMenu));
        }

        public async void HideMainMenu(long chatId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Can not be null or white space", nameof(message));
            }
            
            await this.BotClient.SendTextMessageAsync(chatId, message, ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
        }
        
        public async Task<bool> ProcessInputMessage(MessageEventArgs e)
        {
            if (e.Message.Type != MessageType.Text)
            {
                return false;
            }

            string command = e.Message.Text;

            if (!this.allItems.ContainsKey(command))
            {
                return false;
            }

            MenuItemBase currentMenuItemPressed = this.allItems[command];

            if (currentMenuItemPressed is MenuItemWithAction menuItemWithAction)
            {
                await menuItemWithAction.Action(e.Message.Chat);
            }
            else if (currentMenuItemPressed is MenuItemWithSubItems menuItemWithSubItems)
            {
                await this.BotClient.SendTextMessageAsync(e.Message.Chat.Id, menuItemWithSubItems.MenuEnterMessage, ParseMode.Markdown, replyMarkup: this.FormatReplyKeyboardMarkup(menuItemWithSubItems));
            }
            
            return true;
        }

        private ReplyKeyboardMarkup FormatReplyKeyboardMarkup(MenuItemWithSubItems menu)
        {
            List<List<KeyboardButton>> lines = new List<List<KeyboardButton>>();
            List<KeyboardButton> currentLine = new List<KeyboardButton>();
            int number = 0;
            foreach (MenuItemBase item in menu.SubItems.Where(c => !(c is MenuItemToMainMenu) && !(c is MenuItemBack) ))
            {
                currentLine.Add(item.ToKeyboardButton());
                number++;
                if (number % 2 == 0)
                {
                    lines.Add(currentLine);
                    currentLine = new List<KeyboardButton>();
                }
            }

            if (currentLine.Count != 0)
            {
                lines.Add(currentLine);
                currentLine = new List<KeyboardButton>();
            }

            bool containsGoToMain = menu.SubItems.Any(c => c is MenuItemToMainMenu); 
            bool containsGoBack = menu.SubItems.Any(c => c is MenuItemBack); 
                
            
            if (containsGoToMain && containsGoBack)
            {
                lines.Add(new List<KeyboardButton> {new KeyboardButton(menu.Parent.CommandText), new KeyboardButton(this.MainMenu.CommandText)});
            }
            else
            {
                if (containsGoBack)
                {
                    lines.Add(new List<KeyboardButton> {new KeyboardButton(menu.Parent.CommandText)});
                }

                if (containsGoToMain)
                {
                    lines.Add(new List<KeyboardButton> {new KeyboardButton(this.MainMenu.CommandText)});
                }
            }
            
            ReplyKeyboardMarkup keyboardMarkup = new ReplyKeyboardMarkup();
            keyboardMarkup.OneTimeKeyboard = !menu.ShowAlways;
            keyboardMarkup.ResizeKeyboard = true;
            keyboardMarkup.Selective = true;
            keyboardMarkup.Keyboard = lines;

            return keyboardMarkup;
        }
    }
}