using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer.WeListenApiHandler
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
        public Task<string> GetServiceResponse(string requestUrl, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Timeout = 60000;
            request.ContentType = contentType;
            request.Method = WebRequestMethods.Http.Get;
            request.Proxy = null;

            try
            {
                Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

                return task.ContinueWith(s => ReadStreamFromResponse(s.Result));
            }
            catch
            {
                return null;
            }   
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }
    }
}
