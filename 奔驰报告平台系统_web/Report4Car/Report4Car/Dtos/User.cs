using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Report4Car.Dtos
{
    public class User
    {
        public string ID { get; set; }
        public string Role { get; set; }
        public string ProjectCode { get; set; }
        public string Year { get; set; }
        public string Remark { get; set; }
        public string Brand { get; set; }
    }
}