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
        // Create the .NET Business Connector objects.
        Microsoft.Dynamics.BusinessConnectorNet.Axapta ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
        AxaptaRecord axRecord;
        string tableName = "AddressState";

        public List<Address> getListOfAdresses()
        {
            Addresses addresses = new Addresses();

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
                ax.Logon(null, null, null, null);

                // Create a query using the AxaptaRecord class
                // for the StateAddress table.
                using (axRecord = ax.CreateAxaptaRecord(tableName))
                {

                    // Execute the query on the table.
                    string stmt = "select * from %1";
                    axRecord.ExecuteStmt(stmt);
                    // Create output with a title and column headings
                    // for the returned records.
                    //Console.WriteLine("List of selected records from {0}", tableName);
                    //Console.WriteLine("{0}\t{1}", strNameField, strStateIdField);

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
                            address.StateId = fieldStateId;
                            addresses.Add(address);
                        }

                        // Advance to the next row.
                        axRecord.Next();
                    }
                }
                ax.Logoff();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error encountered: {0}", e.Message);
                // Take other error action as needed.
            }

            return addresses;
        }

        public void createAddress(Address address)
        {
            try
            {
                // Login to Microsoft Dynamics AX.
                ax = new Microsoft.Dynamics.BusinessConnectorNet.Axapta();
                ax.Logon(null, null, null, null);

                // Create a new AddressState table record.
                using (axRecord = ax.CreateAxaptaRecord(tableName))
                {

                    // Provide values for each of the AddressState record fields.
                    axRecord.set_Field("NAME", address.Name);
                    axRecord.set_Field("STATEID", address.StateId);
                    axRecord.set_Field("COUNTRYREGIONID", address.CountryRegionId);
                    axRecord.set_Field("INTRASTATCODE", "");

                    // Commit the record to the database.
                    axRecord.Insert();
                }
                ax.Logoff();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered: {0}", e.Message);
                // Take other error action as needed.
            }
        }

        public void updateAddress(Address address)
        {
            // Create an Axapta record for the StateAddress table.
            axRecord = ax.CreateAxaptaRecord(tableName);

            // Execute a query to retrieve an editable record where the name is MyState.
            string stmt = "select forupdate * from %1 where %1.Name == '" + address.Name + "'";
            axRecord.ExecuteStmt(stmt);

            // If the record is found then update the name.
            if (axRecord.Found)
            {
                // Start a transaction that can be committed.
                ax.TTSBegin();
                axRecord.set_Field("Name", "MyStateUpdated");
                axRecord.Update();

                // Commit the transaction.
                ax.TTSCommit();
            }
            ax.Logoff();
        }

        public void deleteAddress(Address address)
        {
            // Execute a query to retrieve an editable record 
            // where the name is MyStateUpdated.
            string stmt = "select forupdate * from %1 where %1.Name == '" + address.Name + "'";
            axRecord.ExecuteStmt(stmt);

            // If the record is found then delete the record.
            if (axRecord.Found)
            {
                // Start a transaction that can be committed.
                ax.TTSBegin();
                axRecord.Delete();
                // Commit the transaction.
                ax.TTSCommit();
            }
            ax.Logoff();
        }
    }
}
