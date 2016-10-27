namespace Ontext.Core.OAuth
{
    /// <summary>
    /// oAuth error
    /// </summary>
    public class AuthError
    {
        /// <summary>
        /// Error code
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// Error desctiption
        /// </summary>
        public string ErrorDescription { get; set; }
    }
}