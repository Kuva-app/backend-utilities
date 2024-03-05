using System.Net.Http;

namespace Utilities.General.Handlers
{
    /// <summary>
    /// HttpClientCustomHandler
    /// </summary>
    /// <seealso cref="HttpClientHandler" />
    public class HttpClientCustomHandler : HttpClientHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientCustomHandler"/> class.
        /// </summary>
        public HttpClientCustomHandler() => ServerCertificateCustomValidationCallback = delegate
                                                     {
                                                         //bool validationStatus = true;
                                                         //if ((errors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
                                                         //{
                                                         //    Console.WriteLine("SslPolicyErrors.RemoteCertificateChainErrors");
                                                         //    validationStatus = false;
                                                         //}

                                                         //if ((errors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
                                                         //{
                                                         //    Console.WriteLine("SslPolicyErrors.RemoteCertificateNameMismatch");
                                                         //    validationStatus = false;
                                                         //}
                                                         //else if ((errors & SslPolicyErrors.None) != 0)
                                                         //{
                                                         //    Console.WriteLine("SslPolicyErrors.None");
                                                         //    validationStatus = true;
                                                         //}

                                                         //Console.WriteLine(x509Cer2.ToString(true));

                                                         return true;
                                                     };
    }
}
