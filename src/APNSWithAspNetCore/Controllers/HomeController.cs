using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APNSWithAspNetCore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;

namespace APNSWithAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _env;

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PushNotification()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PushNotification(Notification model)
        {
            int port = 2195;
            string hostname = "gateway.sandbox.push.apple.com";

            //I have removed certificate. Keep your certificate in wwwroot/certificate location. This location is not mandatory
            string certificatePath = $@"{_env.WebRootPath}\Certificate\YOUR_CERTIFICATE.pfx";
            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), "YOUR_PASSWORD");
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(AddressFamily.InterNetwork);
            await client.ConnectAsync(hostname, port);

            SslStream sslStream = new SslStream(
                client.GetStream(), false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null);

            try
            {
                await sslStream.AuthenticateAsClientAsync(hostname, certificatesCollection, SslProtocols.Tls, false);
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)32);

                writer.Write(HexStringToByteArray(model.DeviceId.ToUpper()));
                string payload = "{\"aps\":{\"alert\":\"" + model.Message + "\",\"badge\":0,\"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Dispose();
            }
            catch (AuthenticationException ex)
            {
                client.Dispose();
            }
            catch (Exception e)
            {
                client.Dispose();
            }

            return Content("Notification sent. Check your device.");
        }

        #region Helper methods
        private static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        private static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
        #endregion


    }
}
