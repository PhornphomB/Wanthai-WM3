using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;
using System.Xml;
using System.Net;

namespace Prototype.Providers
{
    public static class DynamicWebService
    {
        public static DataTable GetDataWebMethod(string url, string webMethod, object[] parameters)
        {
            bool returnsResult = false;
            string ns = "";
            string[] paramNames = new string[0];
            try
            {
                if ((!url.StartsWith("http://")) && (!url.StartsWith("https://")))
                    url = "http://" + url;

                if (!url.EndsWith("?wsdl"))
                    url += "?wsdl";
                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                httpWebRequest.Timeout = 300000;
                httpWebRequest.Accept = "text/xml; charset=utf-8";

                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();

                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());

                string result = streamReader.ReadToEnd();
                streamReader.Close();

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(result);
                ns = xDoc.GetElementsByTagName("wsdl:definitions")[0].Attributes["targetNamespace"].Value;
                if (!ns.EndsWith("/")) ns += "/";
                foreach (XmlNode xNode in xDoc.GetElementsByTagName("s:element"))
                {
                    if (xNode.Attributes["name"].Value.CompareTo(webMethod) == 0)
                    {
                        XmlNode complexNode = xNode.FirstChild;
                        if (complexNode.HasChildNodes)
                        {
                            XmlNode seqNode = complexNode.FirstChild;
                            if (seqNode.HasChildNodes)
                            {
                                //parameters...
                                paramNames = new string[seqNode.ChildNodes.Count];
                                int i = 0;
                                foreach (XmlNode child in seqNode.ChildNodes)
                                {
                                    paramNames[i] = child.Attributes["name"].Value;
                                    i++;
                                }
                            }
                        }
                        break;
                    }
                }
                foreach (XmlNode xNode in xDoc.GetElementsByTagName("s:element"))
                {
                    if (xNode.Attributes["name"].Value.CompareTo(webMethod + "Response") == 0)
                    {
                        XmlNode complexNode = xNode.FirstChild;
                        if (complexNode.HasChildNodes)
                        {
                            returnsResult = true;
                        }
                        break;
                    }
                }
            }
            catch
            {
                return null;
            }

            string xml = "";
            xml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xml += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ";
            xml += "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ";
            xml += "xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
            xml += "<soap:Body>";

            xml += "<" + webMethod + " xmlns=\"" + ns + "\">";

            #region Add Parameters for Method

            if (parameters != null)
            {
                if (parameters.Length == paramNames.Length)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        xml += "<" + paramNames[i] + ">";

                        if (parameters[i].GetType().ToString().CompareTo("System.String") == 0)
                        {
                            xml += parameters[i].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                        }
                        else
                        {
                            xml += parameters[i].ToString();
                        }
                        xml += "</" + paramNames[i] + ">";
                    }
                }
            }

            #endregion

            xml += "</" + webMethod + ">";

            xml += "</soap:Body>";
            xml += "</soap:Envelope>";

            try
            {

                System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                httpWebRequest.Headers.Add("SOAPAction", "\"" + ns + webMethod + "\"");
                httpWebRequest.Timeout = 300000;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "text/xml; charset=utf-8";  // add the content type so we can handle form data
                httpWebRequest.Accept = "text/xml; charset=utf-8";

                Byte[] postData = Encoding.UTF8.GetBytes(xml); // we need to store the data into a byte array

                httpWebRequest.ContentLength = postData.Length;
                System.IO.Stream tempStream = httpWebRequest.GetRequestStream();
                tempStream.Write(postData, 0, postData.Length); // write the data to be posted to the Request Stream
                tempStream.Close();

                System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();

                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());

                string xmlresult = streamReader.ReadToEnd();
                streamReader.Close();

                if (returnsResult)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xmlresult);

                    string result = xDoc.GetElementsByTagName(webMethod + "Result")[0].InnerText;
                    result = result.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");

                    xDoc = new XmlDocument();
                    xDoc.LoadXml(result);

                    DataSet dsAPDP = new DataSet();
                    using (XmlReader reader = new XmlNodeReader(xDoc.DocumentElement))
                    {
                        dsAPDP.ReadXml(reader);
                        reader.Close();
                    }

                    return dsAPDP.Tables[0];
                }
                else
                {
                    return new DataTable();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}

