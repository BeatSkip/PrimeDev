using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazm.Hid;

namespace PrimeWeb.Types
{
	public class PrimeG2HidFilter : IHidFilter
    {
        public int? vendorId { get { return 1008; } }
        public int? productId { get { return 9281; } }
        public string? classCode { get { return null; } }
        public string? subclassCode { get { return null; } }
        public string? protocolCode { get { return null; } }
        public string? serialNumber { get { return null; } }
    }
}
