using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kcsj.Models
{
    public class KnownPoint
    {
        public string Name { get; set; }
        public double Elevation { get; set; }

        public KnownPoint(string name, double elevation)
        {
            Name = name;
            Elevation = elevation;
        }
    }
}
