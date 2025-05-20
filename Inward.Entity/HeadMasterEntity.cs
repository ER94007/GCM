using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.Entity
{
	public class HeadMasterEntity
	{
		public long HeadMasterId { get; set; }

		[Required(ErrorMessage = "Please select a Head Name")]

		public string HeadName { get; set; }


		public bool IsStudentRelated { get; set; }
		public bool IsCombined { get; set; }
		public string UserId{ get; set; }
	}
}
