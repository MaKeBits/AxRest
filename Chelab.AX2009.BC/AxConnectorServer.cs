using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using Chelab.AX2009.Configuration;
using Microsoft.Dynamics.BusinessConnectorNet;
using System.Security.Principal;

namespace Chelab.AX2009.BC
{
    public class AxConnectorServer
    {
        #region Private
        private Axapta Ax;
        private IIdentity userIdentity = null;        
        #endregion

        //public AxConnectorServer(System.Security.Principal.IIdentity curUserIdentity)
        //{
        //    userIdentity = curUserIdentity;
        //}

        //public AxConnectorServer()
        //{
            
        //}

        //Metodo che legge lo schema restituito ad AX
        
        /// <summary>
        /// Queries Ax
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="paramList">Parameter list</param>
        /// <returns></returns>
        public Byte[] GetAxData(string className, string methodName, params object[] paramList)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                //this.AxLogin();
                this.AxLoginAs();
                AxaptaObject axObj = Ax.CreateAxaptaObject(className);
                string ret = (string)this.callMethod(className, methodName, paramList);

                Byte[] buf = Encoding.UTF8.GetBytes(ret);

                MemoryStream ms = new MemoryStream();
                System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
                zip.Write(buf, 0, buf.Length);
                zip.Close();
                ms.Close();
                axObj.Dispose();

                return ms.GetBuffer();
            }
            catch (Microsoft.Dynamics.AxaptaException ex)
            {
                this.WriteErrorToEventLog(ex);
                SoapException se = new SoapException(ex.Message, SoapException.ServerFaultCode, ex.InnerException);
                throw se;
            }
            catch (Exception ex)
            {
                this.WriteErrorToEventLog(ex);
                SoapException se = new SoapException(ex.Message, SoapException.ClientFaultCode, ex.InnerException);
                throw se;
            }
            finally
            {
                this.AxLogoff();
            }
        }
        
        /// <summary>
        /// Creates the XML schema
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="paramList">Parameters list</param>
        /// <returns></returns>
        public Byte[] GetAxDataCreateSchema(string className, string methodName, params object[] paramList)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                //this.AxLogin();
                this.AxLoginAs();
                AxaptaObject axObj = Ax.CreateAxaptaObject(className);
                string ret = (string)this.callMethod(className, methodName, paramList);

                //converto la stringa in XML document
                xmlDoc.LoadXml(ret);

                //creo un reader di dati XML per popolare il dataset
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDoc.DocumentElement);
                System.Data.DataSet ds = new System.Data.DataSet();

                //carico il dataset coi dati XML (Carico lo schema)
                ds.ReadXml(xmlReader, System.Data.XmlReadMode.InferSchema);

                //comprimo il dataset compreso di schema con GZIP e restituisco un byte array
                MemoryStream ms = new MemoryStream();
                System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
                ds.WriteXml(zip, System.Data.XmlWriteMode.WriteSchema);
                zip.Close();
                ms.Close();
                axObj.Dispose();

                return ms.GetBuffer();
            }
            catch (Microsoft.Dynamics.AxaptaException ex)
            {
                this.WriteErrorToEventLog(ex);
                SoapException se = new SoapException(ex.Message, SoapException.ServerFaultCode, ex.InnerException);
                throw se;
            }
            catch (Exception ex)
            {
                this.WriteErrorToEventLog(ex);
                SoapException se = new SoapException(ex.Message, SoapException.ClientFaultCode, ex.InnerException);
                throw se;
            }
            finally
            {
                this.AxLogoff();
            }
        }

        /// <summary>
        /// Calls an AX method
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="paramList">Parameters list</param>
        /// <returns></returns>
        public Object CallAxMethod(string className, string methodName, params object[] paramList)
        {
            try
            {
                //this.AxLogin();
                this.AxLoginAs();
                return this.callMethod(className, methodName, paramList);
            }
            catch (Microsoft.Dynamics.AxaptaException ex)
            {
                this.WriteErrorToEventLog(ex);
                SoapException se = new SoapException(ex.Message, SoapException.ServerFaultCode, ex.InnerException);
                throw se;
            }
            catch (Exception ex)
            {
                this.WriteErrorToEventLog(ex);
                SoapException se = new SoapException(ex.Message, SoapException.ClientFaultCode, ex.InnerException);
                throw se;
            }
            finally
            {
                this.AxLogoff();
            }
        }

        /// <summary>
        /// Calls an AX method
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="methodName">Method name</param>
        /// <param name="paramList">Parameters list</param>
        /// <returns></returns>
        private Object callMethod(string className, string methodName, params object[] paramList)
        {
            AxaptaObject axObj = Ax.CreateAxaptaObject(className);
            Object ret = null;
            if (paramList != null)
                ret = axObj.Call(methodName, paramList);
            else
                ret = axObj.Call(methodName);

            axObj.Dispose();

            return ret;
        }

        /// <summary>
        /// Login to AX using current User Identity
        /// </summary>
        private void AxLogin()
        {
            string company          = ConfigurationManager.AppSettings["Ax_Company"];
            string configuration    = ConfigurationManager.AppSettings["Ax_Configuration"];

            Ax = new Axapta();

            string[] array = userIdentity.Name.Split(new char[] { '\\' });
            Ax.LogonAs(array[1], array[0], null, company, "", "", configuration);

#if DEBUG
            Ax.Refresh();
#endif

        }

        /// <summary>
        /// Login to AX using credentials specified in the web.config
        /// </summary>
        private void AxLoginAs()
        {
            ConfigurationBase cb = ConfigurationProvider.getConfiguration();

            string Ax_UserName = cb.getStringValue("Ax_UserName");
            string Ax_UserDomain = cb.getStringValue("Ax_UserDomain");
            string company = cb.getStringValue("Ax_Company");
            string configuration = cb.getStringValue("Ax_Configuration");
            string Ax_ProxyUserName = cb.getStringValue("Ax_ProxyUserName");
            string Ax_ProxyUserPwd = cb.getStringValue("Ax_ProxyUserPwd");
            string Ax_ProxyUserDomain = cb.getStringValue("Ax_ProxyUserDomain");

            NetworkCredential nc = new NetworkCredential(Ax_ProxyUserName, Ax_ProxyUserPwd, Ax_ProxyUserDomain);

            Ax = new Axapta();

            Ax.LogonAs(Ax_UserName, Ax_UserDomain, nc, company, "", "", configuration);

#if DEBUG
            Ax.Refresh();
#endif
        }

        /// <summary>
        /// Writes to Windows Event Log
        /// </summary>
        /// <param name="ex"></param>
        private void WriteErrorToEventLog(Exception ex)
        {
            //An object of the EventLog class is created
            EventLog elog = new EventLog();
            elog.Log = "Application";
            elog.Source = "AxConnectorServer";
            string Message = "Message: \n" + ex.Message;
            Message += "\n\nSource: \n" + ex.Source;
            Message += "\n\nStack Trace: \n" + ex.StackTrace;
            elog.WriteEntry(Message, EventLogEntryType.Error);
            elog.Close();
        }

        /// <summary>
        /// Closes AX connection
        /// </summary>
        private void AxLogoff()
        {
            Ax.Logoff();
        }

        /// <summary>
        /// Returns a DataSet from XML.
        /// </summary>
        /// <param name="xml">The XML object to be converted in a Dataset.</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetFromXML(object xml)
        {
            StringReader readStr = null;
            DataSet ds = null;
            if (xml != null)
            {
                try
                {
                    readStr = new StringReader((string)xml);
                    ds = new DataSet();
                    ds.ReadXml(readStr);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a DataTable from XML
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromXML(object xml)
        {
            DataSet ds = new DataSet();
            ds = GetDataSetFromXML(xml);
            if (ds != null)
                if (ds.Tables.Count != 0)
                    return ds.Tables[0];
                else
                    return null;
            else
                return null;
        }
    }
}
