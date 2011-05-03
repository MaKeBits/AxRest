using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using AxRest.AddressState.ServiceModel;
using AxRest.AddressState.Axapta;

namespace AxRest.AddressState.ServiceInterface
{  
    public class AddressService : RestServiceBase<Address>
    {
        public DAL _dal { get; set; } //Injected by IOC

        public override object OnGet(Address request)
        {
            object addresses = _dal.getListOfAdresses();
            return addresses;
        }

        public override object OnPost(Address request)
        {
            return base.OnPost(request);
        }

        public override object OnPut(Address request)
        {
            return base.OnPut(request);
        }

        public override object OnDelete(Address request)
        {
            return base.OnDelete(request);
        }
    }
}
