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

        private string _StateId { get; set; }
        private string _Name { get; set; }
        private string _CountryRegionId { get; set; }
        private string _recId { get; set; }

        public Address() { }

        public Address(String stateId, String name, String countryRegionId, String recId)
        {
            this._StateId = stateId;
            this._Name = name;
            this._CountryRegionId = countryRegionId;
            this._recId = recId;
        }
    }
}
