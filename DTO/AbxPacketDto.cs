using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abc_Exchange_Client.DTO
{
    public class AbxPacketDto
    {
        public string? Symbol { get; set; }
        public string? Side { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Sequence { get; set; }
    }
}
