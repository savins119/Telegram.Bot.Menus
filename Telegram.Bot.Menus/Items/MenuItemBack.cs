using System;

namespace Telegram.Bot.Menus.Items
{
    public class MenuItemBack : MenuItemBase
    {
        public MenuItemBack() : base(Guid.NewGuid().ToString("D"))
        {
        }
    }
}