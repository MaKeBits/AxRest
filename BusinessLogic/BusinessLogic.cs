using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dynamics.BusinessConnectorNet;

namespace AxQuery
{
    class BusinessLogic
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Create the .NET Business Connector objects.
            Axapta ax;
            string sID = "@SYS21669";
            object o;
            bool b;

            try
            {
                // Login to Microsoft Dynamics Ax.
                ax = new Axapta();
                ax.Logon(null, null, null, null);

            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred in object creation or Axapta logon: {0}", e.Message);
                return;
            }

            // Logon was successful.
            try
            {
                // Call a static class method.
                // In this example, call SysLabel::labelId2String2 
                // to determine the label string for a particular label ID.
                o = ax.CallStaticClassMethod("SysLabel", "labelId2String2", sID);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has been encountered during CallStaticClassMethod: {0}", e.Message);
                b = ax.Logoff();
                return;
            }

            // Display the returned string.
            Console.WriteLine("The label string for {0} is {1}.", sID, o.ToString());

            // Log off from Microsoft Dynamics AX.
            b = ax.Logoff();
        }
    }
}

