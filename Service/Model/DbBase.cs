using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Service.Model
{
    public class DbBase
    {
        [Key]
        public int id { get; set; }
    }
}
