namespace Ontext.Server.Core.Utils
{
    public class IdentityUserTokenHelper
    {
        public static string GenerateTokenPurpose(TokenPurpose tokenPurpose, string credentials)
        {
            return string.Format("{0}:{1}", tokenPurpose, credentials);
        }

        public enum TokenPurpose
        {
            Loging,
            Editing
        }
    }
}