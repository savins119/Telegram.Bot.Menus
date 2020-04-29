using System;
using System.Collections.Generic;
using System.Linq;

namespace Telegram.Bot.Menus.Items
{
    public class MenuItemWithSubItems : MenuItemBase
    {
        public IEnumerable<MenuItemBase> SubItems
        {
            get
            {
                return this.subItems.ToArray();
            }
        }

        private List<MenuItemBase> subItems;
        
        public string MenuEnterMessage { get; private set; }
        
        public bool ShowAlways { get; private set; }

        public ushort ColumnsCount { get; private set; }
        
        public MenuItemWithSubItems(string commandText, string menuEnterMessage, bool showAlways = false, ushort columnsCount = 2) : base(commandText)
        {
            if (string.IsNullOrWhiteSpace(menuEnterMessage))
            {
                throw new ArgumentException("Can not be null or white space", nameof(menuEnterMessage));
            }
            this.MenuEnterMessage = menuEnterMessage;
            this.subItems = new List<MenuItemBase>();
            this.ShowAlways = showAlways;

            if (columnsCount < 1 || columnsCount > 8)
            {
                throw new ArgumentException("Value mast be between 1 and 8", nameof(columnsCount));
            }
            
            this.ColumnsCount = columnsCount;
        }

        public MenuItemWithSubItems AddSubItems(IEnumerable<MenuItemBase> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            
            foreach (MenuItemBase item in items)
            {
                this.AddSubItem(item);
            }

            return this;
        }
        
        public MenuItemWithSubItems AddSubItem(MenuItemBase subItem)
        {
            if (subItem == null)
            {
                throw new ArgumentNullException(nameof(subItems));
            }
            
            this.subItems.Add(subItem);
            subItem.Parent = this;
            return this;
        }
    }
}