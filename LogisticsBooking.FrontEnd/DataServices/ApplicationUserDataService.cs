using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class ApplicationUserDataService : BaseDataService , IApplicationUserDataService
    {
        private readonly IOptions<IdentityServerConfiguration> _identityserverConfig;
        private string UrlUser;
        private string UrlRole;
        private string UriTransporter;

        public ApplicationUserDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config , IOptions<IdentityServerConfiguration> identityserverConfig) : base(httpContextAccessor, config)
        {
            _identityserverConfig = identityserverConfig;
            
            UrlUser = identityserverConfig.Value.IdentityServerUrl + "/users/";
            UrlRole = identityserverConfig.Value.IdentityServerUrl + "/roles";
            UriTransporter = identityserverConfig.Value.IdentityServerUrl + "/users/transporters";

        }

        public async Task<ListApplicationUserViewModels> GetAllUsers()
        {
            var response = await GetAsync(UrlUser);
            var result = await TryReadAsync<ListApplicationUserViewModels>(response);
            return result;
        }

        public async Task<Response> CreateUser(CreateUserCommand createUserCommand)
        {
            var response = await PostAsync<CreateUserCommand>(UrlUser, createUserCommand);
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return Response.Unsuccesfull();
        }

        public async Task<ApplicationUserViewModel> GetUserById(GetUserByIdCommand getUserByIdCommand)
        {
            var response = await GetAsync(UrlUser +  getUserByIdCommand.Id);
            var result = await TryReadAsync<ApplicationUserViewModel>(response);
            return result;
        }

        public async Task<Response> UpdateUser(ApplicationUserViewModel applicationUserViewModel)
        {
            var endpoint = UrlUser;  
            
        
            var response = await PutAsync(endpoint, applicationUserViewModel);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response,errorMsg);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }
        
        public async Task<Response> UpdateUser(UpdateUserWithRolesCommand updateUserWithRolesCommand )
        {
            var endpoint = UrlUser;   
        
            var response = await PutAsync(endpoint, updateUserWithRolesCommand);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response,errorMsg);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> UpdateUserRole(UpdateRoleCommand updateRoleCommand)
        {
            var endpoint = UrlRole;   
        
            var response = await PutAsync(endpoint, updateRoleCommand);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response,errorMsg);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> DeleteUser(Guid id)
        {
            var endpoint = UrlUser + id;
            var response = await DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response ,errorMessage);
                }
                return Response.Unsuccesfull(response ,response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> CreateTransporter(CreateUserCommand createUserCommand)
        {
            var response = await PostAsync<CreateUserCommand>(UriTransporter, createUserCommand);
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return Response.Unsuccesfull();
        
        }
    }
}