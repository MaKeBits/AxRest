using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dynamics.BusinessConnectorNet;
using AxRest.AddressState.ServiceModel;


namespace AxRest.AddressState.Axapta
{
    public class DAL
    {
        public List<Address> getListOfAdresses()
        {
            List<Address> addresses = new List<Address>();

            // Create the .NET Business Connector objects.
            Microsoft.Dynamics.BusinessConnectorNet.Axapta ax;
            AxaptaRecord axRecord;
            string tableName = "AddressState";

            // The AddressState field names for calls to
            // the AxRecord.get_field method.
            string strNameField = "NAME";
            string strStateIdField = "STATEID";

            // The output variables for calls to the 
            // AxRecord.get_Field method.
            string fieldName, fieldStateId;

            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
                ax.Logon(null, null, null, null);

                // Create a query using the AxaptaRecord class
                // for the StateAddress table.
                using (axRecord = ax.CreateAxaptaRecord(tableName))
                {

                    // Execute the query on the table.
                    axRecord.ExecuteStmt("select * from %1");
                    // Create output with a title and column headings
                    // for the returned records.
                    Console.WriteLine("List of selected records from {0}",
                        tableName);
                    Console.WriteLine("{0}\t{1}", strNameField, strStateIdField);

                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {
                        // Retrieve the record data for the specified fields.
                        fieldName = (String)axRecord.get_Field(strNameField);
                        fieldStateId = (String)axRecord.get_Field(strStateIdField);

                        // Display the retrieved data.
                        //Console.WriteLine(fieldName + "\t" + fieldStateId);

                        if (!String.IsNullOrEmpty(fieldName) && !String.IsNullOrEmpty(fieldStateId))
                        {
                            Address address = new Address();
                            address.Name = fieldName;
                            Int16 id;
                            bool result = Int16.TryParse(fieldStateId, out id);
                            address.Id = id;
                            addresses.Add(address);
                        }

                        // Advance to the next row.
                        axRecord.Next();
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Error encountered: {0}", e.Message);
                // Take other error action as needed.
            }

            return addresses;
        }
    }
}
