using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer1._1_WPF.XmlHandler
{
    class XmlAccesser
    {

        ///////////////////////////////////////////////////////
        // Request XML URL Handler
        // - Pulls XML page data for parsing
        //
        // - Uses       string serviceResponse = await new XmlAccesser().GetServiceResponse({string:path})
        // - Output     Returns httpResponse, Raw XML page for parsing
        ///////////////////////////////////////////////////////
        public Task<string> GetServiceResponse(string requestUrl)
        {
            return Task.Run(() =>
            {
                string httpResponse = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Timeout = 60000;
                HttpWebResponse response = null;
                StreamReader reader = null;

                try
                {
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (WebException ex)
                    {
                        response = (HttpWebResponse)ex.Response;
                    }

                    reader = new StreamReader(response.GetResponseStream());
                    httpResponse = reader.ReadToEnd();
                }
                catch
                {
                    //MessageBox.Show("Script has stopped unexpectedly!");
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (response != null)
                    {
                        response.Close();
                    }
                }

                return httpResponse;
            });
        }
    }
}
