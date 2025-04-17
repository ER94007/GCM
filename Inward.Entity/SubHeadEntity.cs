using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCM.Entity
{
    public class SubHeadEntity
    {
        public long subheadid {  get; set; }

        [Required(ErrorMessage = "Please select a SubHead Type")]

        public string subheadtype { get; set; }

        [Required(ErrorMessage = "SubHead Name is required")]

        public string subheadname { get; set; }

    }
}
