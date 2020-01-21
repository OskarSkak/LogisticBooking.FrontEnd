using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace LogisticsBooking.FrontEnd.ConfigHelpers

{
    public interface IUserUtility
    {
        Guid GetCurrentUserId();
    }
    public  class UserUtility : IUserUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var value =  _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            return value == null ? Guid.Empty : Guid.Parse(value);
        } 
    }
}