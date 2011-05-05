using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace AxRest.AddressState.ServiceModel
{
    [RestService("/address")]
    [RestService("/address/{recId}")]
    public class Address
    {
        public string StateId { get; set; }
        public string Name { get; set; }
        public string CountryRegionId { get; set; }
        public string recId { get; set; }
    }
}
