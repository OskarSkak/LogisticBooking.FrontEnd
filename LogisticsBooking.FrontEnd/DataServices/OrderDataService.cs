using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices.Utilities
{
    public class OrderDataService : BaseDataService, IOrderDataService
    {
        private string baseurl;

        public OrderDataService(IHttpContextAccessor httpContextAccessor,
            IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/orders/";
        }

        public async Task<Response> CreateOrder(OrderViewModel order)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderViewModel>> GetOrders()
        {
            var response = await GetAsync(baseurl);
            var result = await TryReadAsync<List<OrderViewModel>>(response);
            return result;
        }

        public async Task<OrderViewModel> GetOrderById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<OrderViewModel>(result);
        }

        public async Task<Response> UpdateOrder(OrderViewModel order)
        {
            var endpoint = baseurl + order.OrderId;   
        
            var response = await PutAsync<OrderViewModel>(endpoint, order);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull( response,errorMsg);
                }
                return Response.Unsuccesfull( response,response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> DeleteOrder(Guid id)
        {
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response,errorMessage);
                }
                return Response.Unsuccesfull(response,response.ReasonPhrase);
            }
            return Response.Succes();
         }
       
    }
}