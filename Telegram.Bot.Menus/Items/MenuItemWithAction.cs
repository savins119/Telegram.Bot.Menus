using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Menus.Items
{
    public class MenuItemWithAction : MenuItemBase
    {
        public Func<Chat, Task> Action { get; private set; }
        
        public MenuItemWithAction(string commandText, Func<Chat, Task> action) : base(commandText)
        {
            this.Action = action ?? throw new ArgumentNullException(nameof(action));
        }
    }
}