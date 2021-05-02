using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationUtility
{
    public class OAuthHelper
    {
        /// <summary>
        /// The header to use for OAuth authentication.
        /// </summary>
        public const string OAuthHeader = "Authorization";

        /// <summary>
        /// Retrieves an authentication header from the service.
        /// </summary>
        /// <returns>The authentication header for the Web API call.</returns>
        public static string GetAuthenticationHeader(bool useWebAppAuthentication = true)
        {
            string aadTenant = ClientConfiguration.Default.ActiveDirectoryTenant;
            string aadClientAppId = "c76654db-522a-45fc-8a36-4ec4ad085cdc";//ClientConfiguration.Default.ActiveDirectoryClientAppId;
            string aadClientAppSecret = "ah_n7.S.C2ZcV_-a~.Y3gGO32-f23.n7p5";// ClientConfiguration.Default.ActiveDirectoryClientAppSecret;
            string aadResource = "https://kidc-sandbox.sandbox.operations.dynamics.com";// ClientConfiguration.Default.ActiveDirectoryResource;
           // string Authority = String.Format(CultureInfo.InvariantCulture, aadTenant);

            //AuthenticationContext authenticationContext = new AuthenticationContext("https://login.windows.net/kidcsa.onmicrosoft.com", false);
            AuthenticationContext authenticationContext = new AuthenticationContext("https://sts.windows.net/KIDCSA.onmicrosoft.com", false);
            AuthenticationResult authenticationResult;

            if (useWebAppAuthentication)
            {
                if (string.IsNullOrEmpty(aadClientAppSecret))
                {
                    Console.WriteLine("Please fill AAD application secret in ClientConfiguration if you choose authentication by the application.");
                    throw new Exception("Failed OAuth by empty application secret.");
                }

                try
                {
                    // OAuth through application by application id and application secret.
                    var creadential = new ClientCredential(aadClientAppId,aadClientAppSecret);
                    authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, creadential).Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Failed to authenticate with AAD by application with exception {0} and the stack trace {1}", ex.ToString(), ex.StackTrace));
                    throw new Exception("Failed to authenticate with AAD by application.");
                }
            }
            else
            {
                // OAuth through username and password.
                string username = ClientConfiguration.Default.UserName;
                string password = ClientConfiguration.Default.Password;

                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Please fill user password in ClientConfiguration if you choose authentication by the credential.");
                    throw new Exception("Failed OAuth by empty password.");
                }

                try
                {
                    // Get token object
                    var userCredential = new UserPasswordCredential(username, password); ;
                    authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, aadClientAppId, userCredential).Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Failed to authenticate with AAD by the credential with exception {0} and the stack trace {1}", ex.ToString(), ex.StackTrace));
                    throw new Exception("Failed to authenticate with AAD by the credential.");
                }
            }

            // Create and get JWT token
            return authenticationResult.CreateAuthorizationHeader();
        }
    }
}
