using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Utilities.General
{
    /// <summary>
    /// Mail Util
    /// </summary>
    public abstract class MailUtil
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        private SmtpClient Client { get; set; }

        /// <summary>
        /// The status description
        /// </summary>
        private string _statusDescription;

        /// <summary>
        /// Gets the status description.
        /// </summary>
        public string StatusDescription => _statusDescription;

        /// <summary>
        /// The configuration
        /// </summary>
        private MailConfig _config;
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        /// <exception cref="ArgumentNullException">config - Parâmetros de envio de e-mail inválido.</exception>
        public MailConfig Config
        {
            get => _config;
            set
            {
                _config = value ?? throw new ArgumentNullException(nameof(_config), "Invalid send mail parameters.");
                Client = new SmtpClient(_config.SmtpHost, _config.Port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    EnableSsl = _config.EnableSsl,
                    Credentials = new System.Net.NetworkCredential(_config.User, _config.Password)
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailUtil"/> class.
        /// </summary>
        /// <param name="mailConfig">The mail configuration.</param>
        protected MailUtil(MailConfig mailConfig)
        {
            Config = mailConfig;
        }

        #region send

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <param name="priority">The priority.</param>
        /// <returns></returns>
        public bool Send(MailAddress from,
                         MailAddress to,
                         string subject,
                         string body,
                         bool isBodyHtml,
                         MailPriority priority)
        {
            using MailMessage mailMessage = MailPrepare(@from,
                new MailAddressCollection { to },
                subject,
                body,
                isBodyHtml,
                priority,
                null);
            try
            {
                return Send(mailMessage);
            }
            catch (Exception ex)
            {
                _statusDescription = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <param name="priority">The priority.</param>
        /// <param name="attachmentCollection">The attachment collection.</param>
        /// <returns></returns>
        public bool Send(MailAddress from,
                         MailAddress to,
                         string subject,
                         string body,
                         bool isBodyHtml,
                         MailPriority priority,
                         List<Attachment> attachmentCollection)
        {
            using MailMessage mailMessage = MailPrepare(@from,
                new MailAddressCollection { to },
                subject,
                body,
                isBodyHtml,
                priority,
                attachmentCollection);
            try
            {
                return Send(mailMessage);
            }
            catch (Exception ex)
            {
                _statusDescription = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <param name="priority">The priority.</param>
        /// <returns></returns>
        public bool Send(MailAddress from,
                         MailAddressCollection to,
                         string subject,
                         string body,
                         bool isBodyHtml,
                         MailPriority priority)
        {
            using MailMessage mailMessage = MailPrepare(@from,
                to,
                subject,
                body,
                isBodyHtml,
                priority,
                null);
            try
            {
                return Send(mailMessage);
            }
            catch (Exception ex)
            {
                _statusDescription = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Sends the specified from.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <param name="priority">The priority.</param>
        /// <param name="attachmentCollection">The attachment collection.</param>
        /// <returns></returns>
        public bool Send(MailAddress from,
                         MailAddressCollection to,
                         string subject,
                         string body,
                         bool isBodyHtml,
                         MailPriority priority,
                         List<Attachment> attachmentCollection)
        {
            using MailMessage mailMessage = MailPrepare(@from,
                to,
                subject,
                body,
                isBodyHtml,
                priority,
                attachmentCollection);
            try
            {
                return Send(mailMessage);
            }
            catch (Exception ex)
            {
                _statusDescription = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Mails the prepare.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="isBodyHtml">if set to <c>true</c> [is body HTML].</param>
        /// <param name="priority">The priority.</param>
        /// <param name="attachmentCollection">The attachment collection.</param>
        /// <returns></returns>
        private MailMessage MailPrepare(MailAddress from,
                                        MailAddressCollection to,
                                        string subject,
                                        string body,
                                        bool isBodyHtml,
                                        MailPriority priority,
                                        List<Attachment> attachmentCollection)
        {
            if (isBodyHtml)
                body = body.Replace(Environment.NewLine, "<br />");

            MailMessage mailMessage = new MailMessage
            {
                From = @from,
                Priority = priority,
                IsBodyHtml = isBodyHtml,
                Subject = subject,
                Body = body
            };

            foreach (MailAddress mailAddress in to)
                mailMessage.To.Add(mailAddress);

            if (attachmentCollection == null)
                return mailMessage;

            foreach (Attachment attachment in attachmentCollection)
                mailMessage.Attachments.Add(attachment);

            return mailMessage;
        }

        /// <summary>
        /// Sends the specified mail message.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        /// <returns></returns>
        private bool Send(MailMessage mailMessage)
        {
            if (Config == null || Client == null || mailMessage == null)
            {
                _statusDescription = "Config, client or mailMessage is necessary";
                return false;
            }
            try
            {
                Client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _statusDescription = ex.Message;
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public abstract class MailConfig
        {
            protected MailConfig(string smtpHost, int port, bool enableSsl, string user, string password)
            {
                SmtpHost = smtpHost;
                Port = port;
                EnableSsl = enableSsl;
                User = user;
                Password = password;
            }

            /// <summary>
            /// Gets or sets the SMTP host.
            /// </summary>
            /// <value>
            /// The SMTP host.
            /// </value>
            public string SmtpHost { get; }
            /// <summary>
            /// Gets or sets the port.
            /// </summary>
            /// <value>
            /// The port.
            /// </value>
            public int Port { get; }
            /// <summary>
            /// Gets or sets a value indicating whether [enable SSL].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [enable SSL]; otherwise, <c>false</c>.
            /// </value>
            public bool EnableSsl { get; }
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            /// <value>
            /// The user.
            /// </value>
            public string User { get; }
            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            /// <value>
            /// The password.
            /// </value>
            public string Password { get; }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
