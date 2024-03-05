using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using BigInteger = Org.BouncyCastle.Math.BigInteger;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;


namespace Utilities.Security.Cryptography
{
    /// <summary>
    /// RSA Cryptography
    /// </summary>
    public class RsaCryptography
    {
        // ReSharper disable CommentTypo
        /*
            RSA Cryptography 
        
            OpenSsl
            openssl genrsa -des -out private-filename.pem 2048
            openssl rsa -in private-filename.pem -outform PEM -pubout -out public-filename.pem
            openssl req -new -x509 -key private-filename.pem -out public-filename.cer -days 7300
            openssl pkcs12 -export -out private-filename.pfx -inkey private-filename.pem -in public-filename.cer

            Instalar certificano no repositório (Máquina Local)
            Marcar chave como exportável
            No console de certificados, mover o certificado para o repositório de autoridades de certificação raiz 
            confiáveis/Certificados
        */
        // ReSharper restore CommentTypo

        /// <summary>
        /// The has error
        /// </summary>
        private bool _hasError;
        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError => _hasError;

        /// <summary>
        /// The message
        /// </summary>
        private string _message;
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message => _message;

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">data
        /// or
        /// publicKey</exception>
        /// <remarks>
        /// Use full text
        /// -----BEGIN PUBLIC KEY-----
        /// [KEY]
        /// -----END PUBLIC KEY-----
        /// </remarks>
        public byte[] Encrypt(byte[] data, string publicKey)
        {
            Initialize();
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException(nameof(publicKey));
            try
            {
                var encryptEngine = new Pkcs1Encoding(new RsaEngine());
                using (var sReader = new StringReader(publicKey))
                {
                    var keyParameters = (AsymmetricKeyParameter)new PemReader(sReader).ReadObject();
                    encryptEngine.Init(true, keyParameters);
                }
                return ProcessBlock(encryptEngine, data);
            }
            catch (Exception e)
            {
                Error(e);
                return null;
            }
        }

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">certificate</exception>
        public byte[] Encrypt(byte[] data, System.Security.Cryptography.X509Certificates.X509Certificate certificate)
        {
            Initialize();
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));
            try
            {
                var encryptEngine = new Pkcs1Encoding(new RsaEngine());
                var parser = new X509CertificateParser();
                var keyParameters = parser.ReadCertificate(certificate.GetRawCertData()).GetPublicKey();
                encryptEngine.Init(true, keyParameters);
                return ProcessBlock(encryptEngine, data);
            }
            catch (Exception e)
            {
                Error(e);
                return null;
            }
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">data
        /// or
        /// privateKey
        /// or
        /// password</exception>
        public byte[] Decrypt(byte[] data, string privateKey, string password = null)
        {
            Initialize();
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(privateKey))
                throw new ArgumentNullException(nameof(privateKey));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            try
            {
                var decryptEngine = new Pkcs1Encoding(new RsaEngine());
                using (var sReader = new StringReader(privateKey))
                {
                    var keyParameters = (AsymmetricCipherKeyPair)new PemReader(sReader, new PasswordFinder(password)).ReadObject();
                    decryptEngine.Init(false, keyParameters.Private);
                }
                return ProcessBlock(decryptEngine, data);
            }
            catch (Exception e)
            {
                Error(e);
                return null;
            }
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">certificate</exception>
        public byte[] Decrypt(byte[] data, System.Security.Cryptography.X509Certificates.X509Certificate certificate)
        {
            Initialize();
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));
            try
            {
                using var cert = new X509Certificate2(certificate);
                var encryptEngine = new Pkcs1Encoding(new RsaEngine());
                var parser = new X509CertificateParser();
                parser.ReadCertificate(certificate.GetRawCertData());
                var keyParameter = TransformRsaPrivateKey(cert.PrivateKey);
                encryptEngine.Init(false, keyParameter);
                return ProcessBlock(encryptEngine, data);
            }
            catch (Exception e)
            {
                Error(e);
                return null;
            }
        }

        /// <summary>
        /// Reads the key file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">fileName</exception>
        public string ReadKeyFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                var info = new FileInfo(path);
                if (!info.Exists || info.Extension != ".pem")
                    return string.Empty;
                return File.ReadAllText(info.FullName);
            }
            catch (Exception e)
            {
                Error(e);
                return null;
            }
        }

        /// <summary>
        /// Gets the private key cert by.
        /// </summary>
        /// <param name="thumbPrint">The thumb print.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">thumbPrint</exception>
        public System.Security.Cryptography.X509Certificates.X509Certificate GetPrivateKeyCertBy(string thumbPrint)
        {
            if (string.IsNullOrEmpty(thumbPrint))
                throw new ArgumentNullException(nameof(thumbPrint));
            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var collection = store.Certificates;
            var fcollection = collection.Find(X509FindType.FindByThumbprint, thumbPrint, false);
            return fcollection.Count > 0 ? fcollection[0] : null;
        }

        /// <summary>
        /// Opens the public certificate from.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">path</exception>
        public System.Security.Cryptography.X509Certificates.X509Certificate OpenPublicCertificateFrom(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate(path);
            return certificate;
        }

        /// <summary>
        /// Transforms the RSA private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">privateKey</exception>
        private AsymmetricKeyParameter TransformRsaPrivateKey(AsymmetricAlgorithm privateKey)
        {
            if (privateKey == null)
                throw new ArgumentNullException(nameof(privateKey));
            RSA rsa = privateKey as RSACng;
            var parameters = rsa?.ExportParameters(true);
            if (parameters == null)
                return null;
            return new RsaPrivateCrtKeyParameters(
                new BigInteger(1, parameters.Value.Modulus),
                new BigInteger(1, parameters.Value.Exponent),
                new BigInteger(1, parameters.Value.D),
                new BigInteger(1, parameters.Value.P),
                new BigInteger(1, parameters.Value.Q),
                new BigInteger(1, parameters.Value.DP),
                new BigInteger(1, parameters.Value.DQ),
                new BigInteger(1, parameters.Value.InverseQ));
        }

        /// <summary>
        /// Password Finder
        /// </summary>
        /// <seealso cref="IPasswordFinder" />
        private class PasswordFinder : IPasswordFinder
        {
            /// <summary>
            /// The password
            /// </summary>
            private readonly string _password;
            /// <summary>
            /// Initializes a new instance of the <see cref="PasswordFinder" /> class.
            /// </summary>
            /// <param name="password">The password.</param>
            public PasswordFinder(string password)
            {
                _password = password;
            }
            /// <summary>
            /// Gets the password.
            /// </summary>
            /// <returns></returns>
            public char[] GetPassword()
            {
                return _password.ToCharArray();
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            _hasError = false;
            _message = string.Empty;
        }

        /// <summary>
        /// Errors the specified e.
        /// </summary>
        /// <param name="e">The e.</param>
        private void Error(Exception e)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(e.Message);
            System.Diagnostics.Debug.WriteLine(e.StackTrace);
#endif
            _message = e.Message;
            _hasError = true;
        }

        /// <summary>
        /// Processes the block.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// engine
        /// or
        /// data
        /// </exception>
        private byte[] ProcessBlock(IAsymmetricBlockCipher engine, byte[] data)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException(nameof(data));
            var buffer = new List<byte>();
            var outputBlockSize = engine.GetInputBlockSize();
            var count = Convert.ToInt32(Math.Ceiling((decimal)data.Length / outputBlockSize));
            for (var i = 0; i < count; i++)
            {
                var block = data
                    .Skip(i * outputBlockSize)
                    .Take(outputBlockSize)
                    .ToArray();
                buffer.AddRange(engine.ProcessBlock(block, 0, block.Length));
            }
            return buffer.ToArray();
        }
    }
}
