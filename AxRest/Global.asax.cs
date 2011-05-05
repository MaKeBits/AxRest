using System;
using AxRest.AddressState.Axapta;
using AxRest.AddressState.ServiceInterface;
using AxRest.AddressState.ServiceModel;
using Funq;
using ServiceStack.WebHost.Endpoints;

namespace AxRest
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("AxRest Intro", typeof(AddressService).Assembly){}

        public override void Configure(Container container)
        {
            container.Register<DAL>(new DAL());

            //Register user-defined REST-ful routes         
        Routes
          .Add<Address>("/address")
          .Add<Address>("/address/{recId}");
        }
    }
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}