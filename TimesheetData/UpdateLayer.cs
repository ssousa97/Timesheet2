using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TimesheetCore;

namespace TimesheetData {
    public class UpdateLayer {

        public static bool TemNovaAtualização(string proxAtualização) {

            var request = (FtpWebRequest)WebRequest.Create($@"ftp://{ConfigurationLayer.GetConfig("FtpServer")}/{proxAtualização}");
            request.Credentials = new NetworkCredential(ConfigurationLayer.GetConfig("FtpUser"), ConfigurationLayer.GetConfig("FtpPass"));
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            FtpWebResponse response = null;

            try {

                response = (FtpWebResponse) request.GetResponse();
                return true;

            }
            catch (WebException e) {

                response = (FtpWebResponse) e.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable) return false;

            }
            finally { response.Close();}
            return false;

        }

        public static void BaixarAtualização(string proxAtualização) {

            var request = (FtpWebRequest)WebRequest.Create($@"ftp://{ConfigurationLayer.GetConfig("FtpServer")}/{proxAtualização}");
            request.Credentials = new NetworkCredential(ConfigurationLayer.GetConfig("FtpUser"), ConfigurationLayer.GetConfig("FtpPass"));
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            try {
                using (var response = (FtpWebResponse) request.GetResponse()) {
                    using (var stream = response.GetResponseStream()) {
                        using (var file = new FileStream($@"C:\Users\{Environment.UserName}\Downloads\{proxAtualização}", FileMode.Create)) {
                            stream.CopyTo(file);
                        }
                    }
                }
            } catch (WebException e) {
                throw new Exception(e.Message);
            }
                   
        }
    }
}
