using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Menus.Items
{
    public class MenuItemToMainMenu : MenuItemBase
    {
        public MenuItemToMainMenu() : base(Guid.NewGuid().ToString("D"))
        {
        }
    }
}