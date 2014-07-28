using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace AuthenticationAPI.Controllers
{
    public class Constants
    {
        public const string Port = "49183";
        public const string SigningCertificate = "LocalSTS.pfx";
        public const string SigningCertificatePassword = "LocalSTS";
        public const string IssuerName = "AuthenticationAPI";
        public const string HttpLocalhost = "http://localhost:49183";
        public const string WSFedSts = "/Values/";
        public const string WSFedStsIssue = WSFedSts + "Issue/";
        public const string LocalStsExeConfig = "WSFederationSecurityTokenService.exe.config";
        public const string FederationMetadataAddress = "FederationMetadata/2007-06/FederationMetadata.xml";
        public const string FederationMetadataEndpoint = WSFedSts + FederationMetadataAddress;
        public const string WSTrustSTS = "/wsTrustSTS/";
    }

    public class ValuesController : ApiController
    {
        CustomSecurityTokenServiceConfiguration stsConfiguration;
        SecurityTokenService securityTokenService;

        public ValuesController()
        {
            stsConfiguration = new CustomSecurityTokenServiceConfiguration();
            securityTokenService = new CustomSecurityTokenService(this.stsConfiguration);
        }

        [HttpGet]
        public System.Xml.Linq.XElement FederationMetadata()
        {

            return stsConfiguration.GetFederationMetadata();
        }
       
    }

    internal class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        public CustomSecurityTokenServiceConfiguration()
        {
            this.TokenIssuerName = Constants.IssuerName;
            string signingCertificatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.SigningCertificate);
            X509Certificate2 signignCert = new X509Certificate2(signingCertificatePath, Constants.SigningCertificatePassword, X509KeyStorageFlags.PersistKeySet);
            this.SigningCredentials = new X509SigningCredentials(signignCert);
            this.ServiceCertificate = signignCert;
            var x = this.SecurityTokenHandlers.TokenTypes;
            this.SecurityTokenService = typeof(CustomSecurityTokenService);
            //this.SecurityTokenHandlers.AddOrReplace(new CustomTokenHandler());
        }

        public XElement GetFederationMetadata()
        {
            // hostname
            EndpointReference passiveEndpoint = new EndpointReference(Constants.HttpLocalhost + Constants.WSFedStsIssue);
            EndpointReference activeEndpoint = new EndpointReference(Constants.HttpLocalhost + Constants.WSTrustSTS);

            // metadata document 
            EntityDescriptor entity = new EntityDescriptor(new EntityId(Constants.IssuerName));
            SecurityTokenServiceDescriptor sts = new SecurityTokenServiceDescriptor();
            entity.RoleDescriptors.Add(sts);

            // signing key
            KeyDescriptor signingKey = new KeyDescriptor(this.SigningCredentials.SigningKeyIdentifier);
            signingKey.Use = KeyType.Signing;
            sts.Keys.Add(signingKey);

            // claim types
            sts.ClaimTypesOffered.Add(new DisplayClaim(ClaimTypes.Email, "Email Address", "User email address"));
            sts.ClaimTypesOffered.Add(new DisplayClaim(ClaimTypes.Surname, "Surname", "User last name"));
            sts.ClaimTypesOffered.Add(new DisplayClaim(ClaimTypes.Name, "Name", "User name"));
            sts.ClaimTypesOffered.Add(new DisplayClaim(ClaimTypes.Role, "Role", "Roles user are in"));
            sts.ClaimTypesOffered.Add(new DisplayClaim("http://schemas.xmlsoap.org/claims/Group", "Group", "Groups users are in"));

            // passive federation endpoint
            sts.PassiveRequestorEndpoints.Add(passiveEndpoint);

            // supported protocols

            //Inaccessable due to protection level
            //sts.ProtocolsSupported.Add(new Uri(WSFederationConstants.Namespace));
            sts.ProtocolsSupported.Add(new Uri("http://docs.oasis-open.org/wsfed/federation/200706"));

            // add passive STS endpoint
            sts.SecurityTokenServiceEndpoints.Add(activeEndpoint);

            // metadata signing
            entity.SigningCredentials = this.SigningCredentials;

            // serialize 
            var serializer = new MetadataSerializer();
            XElement federationMetadata = null;

            using (var stream = new MemoryStream())
            {
                serializer.WriteMetadata(stream, entity);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                XmlReaderSettings readerSettings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Prohibit, // prohibit DTD processing
                    XmlResolver = null, // disallow opening any external resources
                    // no need to do anything to limit the size of the input, given the input is crafted internally and it is of small size
                };

                XmlReader xmlReader = XmlTextReader.Create(stream, readerSettings);
                federationMetadata = XElement.Load(xmlReader);
            }

            return federationMetadata;
        }
    }


    internal class CustomSecurityTokenService : SecurityTokenService
    {
        public CustomSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            this.ValidateAppliesTo(request.AppliesTo);
            Scope scope = new Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials);

            scope.TokenEncryptionRequired = false;
            scope.SymmetricKeyEncryptionRequired = false;

            if (string.IsNullOrEmpty(request.ReplyTo))
            {
                scope.ReplyToAddress = scope.AppliesToAddress;
            }
            else
            {
                scope.ReplyToAddress = request.ReplyTo;
            }

            return scope;
            ////var scope = new Scope(request.AppliesTo.Uri.AbsoluteUri, this.SecurityTokenServiceConfiguration.SigningCredentials);

            ////string encryptingCertificateName = WebConfigurationManager.AppSettings[ApplicationSettingsNames.EncryptingCertificateName];
            ////if (!string.IsNullOrEmpty(encryptingCertificateName))
            ////{
            ////    scope.EncryptingCredentials = new X509EncryptingCredentials(CertificateUtilities.GetCertificate(StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName));
            ////}
            ////else
            ////{
            ////    scope.TokenEncryptionRequired = false;
            ////}

            ////scope.ReplyToAddress = scope.AppliesToAddress;

            //return scope;
        }

        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            if (principal == null)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }

            ClaimsIdentity outputIdentity = new ClaimsIdentity();
            foreach (var clm in principal.Claims)
            {
                outputIdentity.AddClaim(clm);
                //                outputIdentity.AddClaim(new Claim(ClaimTypes.Email, "terry@contoso.com"));
                //outputIdentity.AddClaim(new Claim(ClaimTypes.Surname, "Adams"));
                //outputIdentity.AddClaim(new Claim(ClaimTypes.Name, "Terry"));
                //outputIdentity.AddClaim(new Claim(ClaimTypes.Role, "developer"));
                //outputIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/claims/Group", "Sales"));
                //outputIdentity.AddClaim(new Claim("http://schemas.xmlsoap.org/claims/Group", "Marketing"));
            }

            return outputIdentity;
        }
        public override RequestSecurityTokenResponse Validate(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            return base.Validate(principal, request);
        }
        protected override void ValidateRequest(RequestSecurityToken request)
        {
            base.ValidateRequest(request);
        }
        private void ValidateAppliesTo(EndpointReference appliesTo)
        {
            if (appliesTo == null)
            {
                throw new InvalidRequestException("The AppliesTo is null.");
            }
        }
    }
}