using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Shapes;

namespace CF.Models.DataModels
{
    public class mShape : mBase
    {


        public float Left { get; set; } = 0;
        public float Top { get; set; } = 0;
        public float Width { get; set; } = 0;
        public float Height { get; set; } = 0;
    }
}
