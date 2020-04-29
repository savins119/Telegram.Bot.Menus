using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Menus.Items
{
    public class MenuItemWithAction : MenuItemBase
    {
        public Func<Chat, Task> ActionAsync { get; private set; }
        public Action<Chat> Action { get; private set; }
        
        public MenuItemWithAction(string commandText, Func<Chat, Task> action) : base(commandText)
        {
            this.ActionAsync = action ?? throw new ArgumentNullException(nameof(action));
        }

        public MenuItemWithAction(string commandText, Action<Chat> action) : base(commandText)
        {
            this.Action = action ?? throw new ArgumentNullException(nameof(action));
        }
    }
}