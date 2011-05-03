using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dynamics.BusinessConnectorNet;


class ReadResultTable
{
    static void Main(string[] args)
    {
        // Create the .NET Business Connector objects.
        Axapta ax;
        AxaptaRecord axRecord;
        string tableName = "LabX_ResultTable";

        // The Table field names for calls to
        // the AxRecord.get_field method.
        string strSampleField = "SampleID";
        string strDescriptionField = "Description";

        // The output variables for calls to the 
        // AxRecord.get_Field method.
        object fieldSampleId, fieldDescription;

        try
        {
            // Login to Microsoft Dynamics AX.
            ax = new Axapta();
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
                Console.WriteLine("{0}\t{1}", strSampleField, strDescriptionField);

                // Loop through the set of retrieved records.
                while (axRecord.Found)
                {
                    // Retrieve the record data for the specified fields.
                    fieldSampleId = axRecord.get_Field(strSampleField);
                    fieldDescription = axRecord.get_Field(strDescriptionField);

                    // Display the retrieved data.
                    Console.WriteLine(fieldSampleId + "\t" + fieldDescription);

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

        Console.Read();
    }
}

