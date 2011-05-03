using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dynamics.BusinessConnectorNet;

namespace CallAxMethod
{
    class Program
    {     
        static void Main(string[] args)
        {
            Axapta Ax = new Axapta();
            Ax.Logon(null, null, null, null);
            string className = "ZFS_TestXML";
            string methodName = "getResultTable";
            string paramList = null;

            AxaptaObject axObj = Ax.CreateAxaptaObject(className);
            AxaptaRecord ret = null;
            if (paramList != null)
                ret = (AxaptaRecord)axObj.Call(methodName, paramList);
            else
                ret = (AxaptaRecord)axObj.Call(methodName);

            while (ret.Found)
            {
                ret.get_Field(2);
            }

            axObj.Dispose();
 
        }
    }
}
