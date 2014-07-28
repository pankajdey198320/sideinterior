using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Services;
using System.Linq;
using System.ServiceModel.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;

namespace Client1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //start();
        }
        private static readonly string ConfigAddress = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Web.config";
        private void start()
        {
            string stsMetadataAddress = ComputeStsMetadataAddress();

            XmlReader updatedConfigReader = null;

            try
            {
                System.IO.Stream resStream = null;
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(stsMetadataAddress);
                request.Method = "GET";
                request.ContentType = "application/xml";

                XDocument document = new XDocument();
                using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        resStream = response.GetResponseStream();
                        using (XmlReader metadataReader = XmlReader.Create(resStream))
                        {
                            var s = metadataReader.ReadInnerXml();
                            using (XmlReader configReader = XmlReader.Create(ConfigAddress))
                            {
                                // Creates the xml reader pointing to the updated web.config contents
                                // Don't validate the cert signing the federation metadata
                                MetadataSerializer serializer = new MetadataSerializer()
                                {
                                    CertificateValidationMode = X509CertificateValidationMode.None,
                                };

                                updatedConfigReader = FederationManagement.UpdateIdentityProviderTrustInfo(metadataReader, configReader, false, serializer);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }


        }
        private static string ComputeStsMetadataAddress()
        {
            return "http://localhost:49183/api/Values/FederationMetadata/";
        }
    }
}
