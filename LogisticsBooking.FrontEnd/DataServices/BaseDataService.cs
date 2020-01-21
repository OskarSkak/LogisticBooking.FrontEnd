using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class BaseDataService
    {
        protected  IHttpContextAccessor _httpContextAccessor;
        protected readonly IOptions<BackendServerUrlConfiguration> _config;
        protected string _APIServerURL;

        public BaseDataService(IHttpContextAccessor httpContextAccessor , IOptions<BackendServerUrlConfiguration> config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _APIServerURL = _config.Value.BackendServerUrl;
        }

        

        /// <summary>
        /// Post with body
        /// </summary>
        /// <typeparam name="T">Entity or value</typeparam>
        /// <param name="baseurl">Backend server end-point</param>
        /// <param name="Entity">Entity (what is to be posted)</param>
        /// <returns>HttpResponse message (Either succesfull or null)</returns>
        protected async Task<HttpResponseMessage> PostAsync<T>(string baseurl, T Entity)
        {
            HttpClient Client = await GetClient();

            //Set the client to accept json in body
            Client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var Content = JsonConvert.SerializeObject(Entity, Formatting.Indented);
            var HttpContent = new StringContent(JsonConvert.SerializeObject(Entity, Formatting.Indented), Encoding.UTF8, "application/json");

            HttpResponseMessage HttpResponse = null;

            try
            {
                HttpResponse = await Client.PostAsync(baseurl, HttpContent);
            }
            catch (WebException ex)
            {
                //Logs the exception (Hopefully) in the txt doc in app data
              
            }

            return HttpResponse;
        }

        /// <summary>
        /// PostAsync without any entity or value
        /// </summary>
        /// <param name="baseurl">Backend Server endpoint</param>
        /// <returns>Either succesfull/unsuccessfull HttpResponse or null</returns>
        protected async Task<HttpResponseMessage> PostAsync(string baseurl)
        {
            HttpClient client = await GetClient();
            //Set the client to accept json in body
            client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var httpContent = new StringContent(JsonConvert.SerializeObject(null, Formatting.Indented), System.Text.Encoding.UTF8, "application/json");

            return await client.PostAsync(baseurl, httpContent);
        }

        protected async Task<HttpResponseMessage> PostManyAsync<T>(string baseurl, T entity)
        {
            HttpClient client = await GetClient();
            
            //Set the client to accept json in body
            client.DefaultRequestHeaders
                .Accept
                .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            
            
            var cont = JsonConvert.SerializeObject(entity, Formatting.Indented);
            var httpContent = new StringContent(JsonConvert.SerializeObject(entity, Formatting.Indented), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = null;

            try
            {
                httpResponse = await client.PostAsync(baseurl, httpContent);
            }
            catch (WebException ex)
            {
                //Logs the exception (Hopefully) in the txt doc in app data
             
            }
            return httpResponse;
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string baseurl, T entity)
        {
            HttpClient client = await GetClient();
            var httpContent = new StringContent(JsonConvert.SerializeObject(entity, Formatting.Indented), System.Text.Encoding.UTF8, "application/json");

            return await client.PutAsync(baseurl, httpContent);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string baseurl)
        {
            HttpClient client = await GetClient();

            return await client.DeleteAsync(baseurl);
        }
        
        

        protected async Task<HttpResponseMessage> GetAsync(string baseurl)
        {
            HttpClient client = new HttpClient();
            
            var token = string.Empty;

            var currentContext = _httpContextAccessor.HttpContext;

            token = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , token);

            HttpResponseMessage result = null;
            try
            {
                result = await client.GetAsync(baseurl);
                if (!result.IsSuccessStatusCode)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logs the exception (Hopefully) in the txt doc in app data
                
                
            }

            return result;
        }

        protected async Task<T> TryReadAsync<T>(HttpResponseMessage response) where T : class
        {
            if (response.Content == null || !response.IsSuccessStatusCode)
            {
                return null; // <-- Dont eat the error here!
            }

            var content = await response.Content.ReadAsStringAsync();

            
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task<IEnumerable<T>> GetListAsync<T>(HttpResponseMessage responseMessage) where T : class
        {

            HttpClient client = await GetClient();
            
            if (responseMessage.Content == null)
            {
                return null; // <-- Dont eat the error here!
            }

            var content = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

        private async Task<HttpClient> GetClient()
        {
            HttpClient client;

            client = new HttpClient();
            var token = string.Empty;

            var currentContext = _httpContextAccessor.HttpContext;

            token = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;

        }

    }


}
