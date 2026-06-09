using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kcsj.Models
{
    public class Observation
    {
        public string FromPoint { get; set; }
        public string ToPoint { get; set; }
        public double HeightDiff { get; set; }
        public double Distance { get; set; }

        public Observation(string fromPoint, string toPoint, double heightDiff, double distance)
        {
            FromPoint = fromPoint;
            ToPoint = toPoint;
            HeightDiff = heightDiff;
            Distance = distance;
        }
    }
}
