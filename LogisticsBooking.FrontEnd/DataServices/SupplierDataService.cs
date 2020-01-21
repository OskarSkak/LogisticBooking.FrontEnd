using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class SupplierDataService : BaseDataService, ISupplierDataService
    {
        public SupplierDataService(IHttpContextAccessor httpContextAccessor , IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor , config)
        {
            baseurl = _APIServerURL + "/api/suppliers/";
        }

        private string baseurl;
        
        public async Task<Response> CreateSupplier(CreateSupplierViewModel _supplier)
        {
            var response = await PostAsync<CreateSupplierViewModel>(baseurl, _supplier);

            
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull( response,errorMessage);
                }
                return Response.Unsuccesfull( response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> DeleteSupplier(Guid id)
        {
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull();
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<SupplierViewModel> GetSupplierById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<SupplierViewModel>(result);
        }

        public async Task<SuppliersListViewModel> ListSuppliers(int page, int pageSize)
        {
            var result = await GetAsync(baseurl);
            return await TryReadAsync<SuppliersListViewModel>(result);
        }

        public async Task<SupplierViewModel> GetSupplierByName(string name)
        {
            var suppliersEnumerable = await ListSuppliers(0, 0);
            var suppliersList = suppliersEnumerable;
            foreach (var item in suppliersList.Suppliers)
                if (item.Name == name) return item;

            return null;
        }

        public async Task<Response> UpdateSupplier(Guid id, SupplierViewModel supplier)
        {
            var endpoint = baseurl + id;
            
            var response = await PutAsync<SupplierViewModel>(endpoint, supplier);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response ,errorMsg);
                }
                return Response.Unsuccesfull( response , response.ReasonPhrase);
            }
            return Response.Succes();
        }
    }
}
