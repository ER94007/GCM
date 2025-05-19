using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.Entity
{
	public class MenuViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public string Icon { get; set; }
		public int? ParentId { get; set; }
		public int DisplayOrder { get; set; }
        public List<MenuViewModel> Children { get; set; } = new List<MenuViewModel>();

    }
}
