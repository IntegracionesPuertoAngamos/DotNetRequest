using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace DotNetRequest
{
    class Program
    {
        private const string authEndpoint = "https://navisq.puertoangamos.cl/APIWsPortAngTatc/Auth/authenticate";
        private const string sendTATCEndpoint = "https://navisq.puertoangamos.cl/APIWsPortAngTatc/PortAngTatc/EnviarTatc";
        private const string cancelTATCEndpoint = "https://navisq.puertoangamos.cl/APIWsPortAngTatc/PortAngTatc/AnularTatc";

        static void Main(string[] args)
        {
            var token = GetToken();

            Console.WriteLine("Enviando información de TATC");
            SendTATC(token);

            Console.WriteLine("Anulando TATC");
            CancelTATC(token);
        }

        static void SendTATC(Token token)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(sendTATCEndpoint);

                var obj = JsonSerializer.Serialize(new
                {
                    dcNumeroTatc = "100001127",
                    dcOperadorTatc = "C20",
                    dgOperadorTatc = "Operador Prueba",
                    dcCodigoAduana = "ADU01",
                    dgCodigoAduana = "Nombre Prueba Aduana",
                    dcIdCtr = "MRKU000000-1",
                    dcPuerto = "CLPAG",
                    dcIsocode = "2210",
                    dcNumeroBl = "SH1KJ6480400",
                    dcRutOperadorContenedor = "96653890-2",
                    dgOperadorContenedor = "HLC",
                    dcRutDeposito = "96789280-7",
                    dgNombreDeposito = "Deposito PANG",
                    dcNumeroLloyd = "9198575",
                    dgViaje = "138E",
                    dgNave = "NAVE TEST",
                    dcRutAgenteAduana = "15679146-6",
                    dgNombreAgenteAduana = "Agencia Aduana Prueba",
                    dcRutCliente = "15679146-6",
                    dgNombreCliente = "Cliente Prueba",
                    dgobservacion = "Datos de Prueba",
                    dgEmisorBl = "Navis",
                    dfLiberacion = "2022-02-01 08:00:00"
                });

                var objBytes = Encoding.ASCII.GetBytes(obj);

                request.Headers.Add("Authorization", $"Bearer {token.token}");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = obj.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(objBytes, 0, obj.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Console.WriteLine(responseString);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void CancelTATC(Token token)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(cancelTATCEndpoint);

                var obj = JsonSerializer.Serialize(new
                {
                    dcNumeroTatc = "100001127",
                    dcOperadorTatc = "C20",
                });

                var objBytes = Encoding.ASCII.GetBytes(obj);

                request.Headers.Add("Authorization", $"Bearer {token.token}");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = obj.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(objBytes, 0, obj.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                Console.WriteLine(responseString);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Token GetToken()
        {
            var token = new Token();

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(authEndpoint);

                // Las credenciales se encuentran en la Guía de Integración
                var obj = JsonSerializer.Serialize(new { username = "username", password = "password" });

                var objBytes = Encoding.ASCII.GetBytes(obj);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = obj.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(objBytes, 0, obj.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                token = JsonSerializer.Deserialize<Token>(responseString);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return token;
        }
    }

    class Token
    {
        public string token { get; set; }
    }
}
