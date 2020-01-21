using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LogisticsBooking.FrontEnd.Documents
{
    public class Response
    {
        public bool IsSuccesfull { get; private set; }
        public string Message { get; private set; }
        public string Exception { get; private set; }
        
        public HttpResponseMessage HttpResponse { get; set; }

        public Response(bool isSuccesfull, HttpResponseMessage response = null,  string message = null, string exception = null)
        {
            IsSuccesfull = isSuccesfull;
            Message = message;
            Exception = exception;
            HttpResponse = response;
        }

        public static Response Succes( HttpResponseMessage response = null)
        {
            return new Response(true , response );
        }

        public static Response Unsuccesfull()
        {
            return new Response(false);
        }

        public static Response Unsuccesfull(HttpResponseMessage response , string error)
        {
            return new Response(false, response,  error);
        }
    }
}
