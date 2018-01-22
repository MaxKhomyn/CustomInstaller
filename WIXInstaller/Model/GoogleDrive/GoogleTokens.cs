using System.IO;
using System.Web.Script.Serialization;

namespace WIXInstaller.Model.GoogleDrive
{
    public class JsonSerialize
    {
        public static void Serialize(Tokens obj)
        {
            var json = new JavaScriptSerializer().Serialize(obj);
            File.WriteAllText("Tokens", json);
        }
    }
    public class Tokens
    {
        public Installed installed { get; set; } = new Installed();
        public Tokens() { }
    }

    public class Installed
    {
        public string client_id
            = "514005526695-h1vmf9dq7skiaob0ntcpfmtt3b2v4sg1.apps.googleusercontent.com";

        public string project_id
            = "installerproject-188706";

        public string auth_uri
            = "https:/" + "/accounts.google.com/o/oauth2/auth";

        public string token_uri
            = "https:/" + "/accounts.google.com/o/oauth2/token";

        public string auth_provider_x509_cert_url
            = "https:/" + "/www.googleapis.com/oauth2/v1/certs";

        public string client_secret
            = "zRzWBPkyNtm0Ch74ngIUIXMq";

        public string[] redirect_uris
            = { "urn:ietf:wg:oauth:2.0:oob", "http:/" + "/localhost" };

        public Installed() { }
    }
}
