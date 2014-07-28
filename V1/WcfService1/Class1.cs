using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Xml.Linq;

namespace SSOAuthenticationService
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.

    [ServiceContract]
    public interface ISSOAuthentication
    {

        
        [OperationContract]
        [WebGet]
        Stream IssueToken(string realm, string wctx, string wct, string wreply);

        [OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare)]
        [WebGet(UriTemplate = "/FederationMetadata")]
        XElement FederationMetadata();

        [OperationContract]
        [WebGet]
        bool SignIn(string model, string returnUrl);

    }


    [DataContract]
    public class LogOn
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public string UserName
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        [DataMember]
        public string Password
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SSOAuthentication : ISSOAuthentication
    {
        public SSOAuthentication()
        { }

        public Stream IssueToken(string realm, string wctx, string wct, string wreply) { return null; }
        public XElement FederationMetadata()
        {
            return null;
        }

        public bool SignIn(string model, string returnUrl) { return false; }
    }
}