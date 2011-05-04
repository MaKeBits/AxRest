using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxRest.AddressState.ServiceModel
{
    public class Addresses : List<Address>
    {
        public Addresses() { }
        public Addresses(IEnumerable<Address> collection) : base(collection) { }
    }
}
