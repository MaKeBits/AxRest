using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Chelab.AX2009.BC;

namespace Chelab.AX2009.WS
{
    /// <summary>
    /// Descrizione di riepilogo per LabX
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // Per consentire la chiamata di questo servizio Web dallo script utilizzando ASP.NET AJAX, rimuovere il commento dalla riga seguente. 
    // [System.Web.Script.Services.ScriptService]
    public class LabX : System.Web.Services.WebService
    {      
        private AxConnectorServer _acs = new AxConnectorServer();

        [WebMethod]
        public DataTable GetTable()
        {
            //DataTable dt = new DataTable();
            //DataSet ds = new DataSet();
            object xml = null;

            try
            {
                xml = _acs.CallAxMethod("ZFS_TestXML", "getSamples");              
            }
            catch (Exception ex)
            {
                return null;
            }
            return _acs.GetDataTableFromXML(xml);
        }       
    }
}
