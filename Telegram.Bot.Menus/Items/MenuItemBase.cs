using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Menus.Items
{
    public abstract class MenuItemBase
    {
        public string CommandText { get; private set; }

        internal MenuItemWithSubItems Parent { get; set; } 
        
        protected internal MenuItemBase(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText))
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            this.CommandText = commandText;
        }

        protected internal virtual KeyboardButton ToKeyboardButton()
        {
            return new KeyboardButton(this.CommandText);
        }
    }
}