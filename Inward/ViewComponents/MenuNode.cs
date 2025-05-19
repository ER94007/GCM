using GCM.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.ViewComponents
{
    public class MenuNode
    {
        public MenuViewModel Menu { get; set; }
        public List<MenuNode> Children { get; set; } = new List<MenuNode>();
    }
}
