using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.commands;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.TransportersList;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class TransporterDataService : BaseDataService, ITransporterDataService
    {

        public string ApiURL { get; set; }
        public TransporterDataService(IHttpContextAccessor httpContextAccessor , IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor , config)
        {
            baseurl = _APIServerURL + "/api/transporters/";
        }

        public string baseurl;

        public async Task<Response> CreateTransporter(TransporterViewModel _transporter)
        {
            var response = await PostAsync<TransporterViewModel>(baseurl, _transporter);

            if (!response.IsSuccessStatusCode)
            {
                if(response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull( response, errorMessage);
                }
                return Response.Unsuccesfull( response, response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<TransporterViewModel> GetTransporterById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<TransporterViewModel>(result);
        }
        
        public async Task<TransporterViewModel> GetTransporterByName(String name)
        {
            var service = this;
            var TransporterEnumerable = await service.ListTransporters(0, 0);
            
            
            foreach(var item in TransporterEnumerable.Transporters)
                if (item.Name == name)
                    return item;

            return null;
        }

        public async Task<TransportersListViewModel> ListTransporters(int page, int pageSize)
        {
            var result = await GetAsync(baseurl);
            if (!result.IsSuccessStatusCode)
            {
                
            }
            return await TryReadAsync<TransportersListViewModel>(result); 
        }

        public async Task<Response> AddSupplierToTransporter( AddSupplierToTransporterCommand command)
        {
            var endpoint = baseurl + "addsupplier";

            var response = await PutAsync<AddSupplierToTransporterCommand>(endpoint, command);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response , errorMsg);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> RemoveSupplierFromTransporter(RemoveSupplierFromTransporterCommand command)
        {
            var endpoint = baseurl + "removesupplier";

            var response = await PutAsync<RemoveSupplierFromTransporterCommand>(endpoint, command);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response , errorMsg);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> UpdateTransporter(Guid id, TransporterViewModel transporter)
        {
            var endpoint = baseurl + id;

            var response = await PutAsync<TransporterViewModel>(endpoint, new TransporterViewModel
            {
                Address = transporter.Address,
                Email = transporter.Email,
                Name = transporter.Name,
                Telephone = transporter.Telephone,
                TransporterId = transporter.TransporterId
                    
            });
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response , errorMsg);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        

        public async Task<Response> DeleteTransporter(Guid id)
        {
            var endpoint = baseurl + id;

            var response = await DeleteAsync(endpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response ,errorMsg);
                }
                return Response.Unsuccesfull(response ,response.ReasonPhrase);
            }
            return Response.Succes();
        }
    }
}
