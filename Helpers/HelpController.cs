using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Prueba_Tecnica.Helpers
{
    public class HelpController : Microsoft.AspNetCore.Mvc.Controller
    {
        #region properties
        public IConfiguration Configuration { get; }
        protected static System.Net.ICredentials NetworkCredentials { get; }
        protected virtual string ReportServerUrl { get; }
        //protected virtual System.ServiceModel.HttpClientCredentialType ClientCredentialType
        //{
        //    get
        //    {
        //        return System.ServiceModel.HttpClientCredentialType.Basic;
        //        //return System.ServiceModel.HttpClientCredentialType.Windows;
        //    }
        //}
        protected virtual bool AjaxLoadInitialReport { get { return true; } }
        protected virtual System.Text.Encoding Encoding { get { return System.Text.Encoding.ASCII; } }
        

        #endregion

        #region constructor
        public HelpController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion


        public string GetConfigValue(string key)
        {
            try
            {
                return Configuration.GetSection(key).Value;
            }
            catch
            {
                return string.Empty;
            }
        }

        public int? UserId
        {
            get
            {
                var user = HttpContext.User;
                var firstOrDefault = user.Claims.FirstOrDefault(a => a.Type == "UserId");
                if (firstOrDefault != null)
                    return int.Parse(firstOrDefault.Value);
                return null;
            }
        }

      

        private static HttpRequestMessage CloneHttpRequestMessageAsync(HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            // Copy the request's content (via a MemoryStream) into the cloned object
            var ms = new System.IO.MemoryStream();
            if (req.Content != null)
            {
                clone.Content = req.Content;

                // Copy the content headers
                /*if (req.Content.Headers != null)
                    foreach (var h in req.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);*/
            }

            clone.Version = req.Version;

            foreach (System.Collections.Generic.KeyValuePair<string, object> prop in req.Properties)
                clone.Properties.Add(prop);

            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.IEnumerable<string>> header in req.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            clone.Headers.Remove("Authorization");
            return clone;
        }
     
    
    }

    public class RequestResponse
    {
        public string JsonResponseValue { get; set; }
        public HttpStatusCode StatusCode { get; set; }

    }
}

