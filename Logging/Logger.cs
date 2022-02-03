using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Teos.Logging
{
    public static class Logger
    {
        /// <summary>
        /// Логирование ошибки
        /// </summary>
        public static void LogError(Exception ex)
        {
            try
            {
                SendError(ex);
            }
            catch
            {
                // ignored
            }
        }

        private static void SendError(Exception ex)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(ErrorUrl);
                request.Method = "POST";
                request.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var errorData = new ErrorData
                    {
                        Message = ex.ToString(),
                        PathToExecutable = Global.ExecutableLocation,
                        OSVersion = ""
                    };
                    var json = JsonConvert.SerializeObject(errorData);
                    streamWriter.Write(json);
                }

                request.GetResponse();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Url to error logging.
        /// </summary>
        private static string ErrorUrl => Global.SiteUrl + "error";
    }
}
