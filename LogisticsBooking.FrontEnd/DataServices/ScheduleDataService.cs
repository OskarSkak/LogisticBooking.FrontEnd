

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class ScheduleDataService : BaseDataService, IScheduleDataService
    {
        private string baseurl;
        
        public ScheduleDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/schedules/";
        }



        
            public async Task<Response> CreateSchedule(CreateScheduleCommand schedule) 
            {
            var response = await PostAsync<CreateScheduleCommand>(baseurl, schedule);

            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return new Response(false); 
            }

            public async Task<Response> CreateScheduleFromMAster(CreateScheduleFromMasterCommand schedule)
            {
                var endpoint = baseurl + "master";
                var response = await PostAsync<CreateScheduleFromMasterCommand>(endpoint, schedule);

            
                if (response.IsSuccessStatusCode)
                {
                    return new Response(true );
                }
                return new Response(false);
            }
            
            public async Task<Response> CreateScheduleFromSchedule(CreateScheduleFromActiveSchedule schedule)
            {
                var endpoint = baseurl + "schedule";
                var response = await PostAsync<CreateScheduleFromActiveSchedule>(endpoint, schedule);

            
                if (response.IsSuccessStatusCode)
                {
                    return new Response(true );
                }
                return new Response(false);
            }

            public async Task<SchedulesListViewModel> GetSchedules()
        {
            var response = await GetAsync(this.baseurl);
            
            var result = await TryReadAsync<SchedulesListViewModel>(response);

            return result;
        }

        public async Task<ScheduleViewModel> GetScheduleById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<ScheduleViewModel>(result);
        }

        public async Task<Response> UpdateSchedule(ScheduleViewModel schedule)
        {
            var endpoint = baseurl + schedule.ScheduleId;  
        
            var response = await PutAsync<ScheduleViewModel>(endpoint, schedule);

            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull( response, errorMsg);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> DeleteSchedule(Guid id)
        {
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                if (response.Content != null)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Response.Unsuccesfull(response , errorMessage);
                }
                return Response.Unsuccesfull(response , response.ReasonPhrase);
            }
            return Response.Succes();
        }

        public async Task<Response> CreateManySchedule(CreateManyScheduleCommand schedule)
        {
            baseurl = baseurl + "list";
            var response = await PostAsync(baseurl, schedule);
            
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return new Response(false);
        
        }

        public async Task<SchedulesListViewModel> GetScheduleBydate(DateTime date)
        {
            var endpoint = baseurl + "date/" + date.ToString("yyyy-MM-dd");
            var response = await GetAsync(endpoint);
            return await TryReadAsync<SchedulesListViewModel>(response);
        }
    }
}