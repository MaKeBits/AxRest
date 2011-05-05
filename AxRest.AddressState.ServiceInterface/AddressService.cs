using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using AxRest.AddressState.ServiceModel;
using AxRest.AddressState.Axapta;
using ServiceStack.Common.Web;
using System.Net;
using ServiceStack.Text;

namespace AxRest.AddressState.ServiceInterface
{
    public class AddressService : RestServiceBase<Address>
    {
        public DAL _dal { get; set; } //Injected by IOC

        public override object OnGet(Address request)
        {
            Addresses addresses = (Addresses)_dal.getListOfAdresses();

            if (!String.IsNullOrEmpty(request.recId))
            {
                Address address = addresses.Find(delegate(Address a) { return a.recId == request.recId; });
                return address;
            }

            return addresses;
        }

        public override object OnPost(Address request)
        {
            _dal.createAddress(request);

            var pathToNewResource = base.RequestContext.AbsoluteUri.WithTrailingSlash() + request.StateId;
            return new HttpResult(request)
            {
                StatusCode = HttpStatusCode.Created,
                Headers = {
					{ HttpHeaders.Location, pathToNewResource },
				}
            };
        }

        public override object OnPut(Address request)
        {
            _dal.updateAddress(request);
            return new Address();
        }

        public override object OnDelete(Address request)
        {
            _dal.deleteAddress(request);
            return new Address();
        }
    }
}
