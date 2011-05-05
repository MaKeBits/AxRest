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
            string strRecIdField = "RecId";

            // The output variables for calls to the 
            // AxRecord.get_Field method.
            string fieldName, fieldStateId, recId;

            try
            {
                AxLogon();

                // Create a query using the AxaptaRecord class
                // for the StateAddress table.
                using (axRecord = ax.CreateAxaptaRecord(tableName))
                {
                    // Execute the query on the table.
                    string stmt = "select * from %1";
                    axRecord.ExecuteStmt(stmt);

                    // Loop through the set of retrieved records.
                    while (axRecord.Found)
                    {
                        // Retrieve the record data for the specified fields.
                        fieldName = (String)axRecord.get_Field(strNameField);
                        fieldStateId = (String)axRecord.get_Field(strStateIdField);
                        recId = axRecord.get_Field(strRecIdField).ToString();

                        if (!String.IsNullOrEmpty(fieldName) && !String.IsNullOrEmpty(fieldStateId) && !String.IsNullOrEmpty(recId))
                        {
                            Address address = new Address();
                            address.Name = fieldName;
                            address.StateId = fieldStateId;
                            address.recId = recId;
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
                throw (e);
            }
            finally
            {
                ax.Logoff();
            }

            return addresses;
        }

        private void AxLogon()
        {
            // Login to Microsoft Dynamics AX.                
            ax.Logon(null, null, null, null);
            ax.Refresh();
        }

        public void createAddress(Address address)
        {
            try
            {
                AxLogon();

                // Create a new AddressState table record.
                using (axRecord = ax.CreateAxaptaRecord(tableName))
                {
                    createAxRecord(address);
                }
                ax.Logoff();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                ax.Logoff();
            }
        }

        private void createAxRecord(Address address)
        {
            // Provide values for each of the AddressState record fields.
            axRecord.set_Field("NAME", address.Name);
            axRecord.set_Field("STATEID", address.StateId);
            axRecord.set_Field("COUNTRYREGIONID", address.CountryRegionId);
            axRecord.set_Field("INTRASTATCODE", "");

            // Commit the record to the database.
            axRecord.Insert();
        }

        public void updateAddress(Address address)
        {
            try
            {
                AxLogon();

                using (axRecord = ax.CreateAxaptaRecord(tableName))
                {

                    // Execute a query to retrieve an editable record where the name is MyState.
                    string stmt = "select forupdate * from %1 where %1.RecId == " + address.recId + "";
                    axRecord.ExecuteStmt(stmt);
                    
                    updateAxRecord(address);
                }
                ax.Logoff();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                ax.Logoff();
            }
        }

        private void updateAxRecord(Address address)
        {
            // If the record is found then update the name.
            if (axRecord.Found)
            {
                // Start a transaction that can be committed.
                ax.TTSBegin();
                axRecord.set_Field("NAME", address.Name);
                axRecord.set_Field("STATEID", address.StateId);
                axRecord.set_Field("COUNTRYREGIONID", address.CountryRegionId);
                axRecord.set_Field("INTRASTATCODE", "");
                axRecord.Update();

                // Commit the transaction.
                ax.TTSCommit();
            }
        }

        public void deleteAddress(Address address)
        {
            try
            {

                AxLogon();

                using (axRecord = ax.CreateAxaptaRecord(tableName))
                {
                    // Execute a query to retrieve an editable record 
                    // where the name is MyStateUpdated.
                    string stmt = "select forupdate * from %1 where %1.RecId == " + address.recId + "";
                    axRecord.ExecuteStmt(stmt);

                    deleteAxRecord();
                }
                ax.Logoff();
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                ax.Logoff();
            }
        }

        private void deleteAxRecord()
        {
            // If the record is found then delete the record.
            if (axRecord.Found)
            {
                // Start a transaction that can be committed.
                ax.TTSBegin();
                axRecord.Delete();
                // Commit the transaction.
                ax.TTSCommit();
            }
        }
    }
}
