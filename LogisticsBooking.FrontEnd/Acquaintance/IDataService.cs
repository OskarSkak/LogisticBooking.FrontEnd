using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Dashboard;
using LogisticsBooking.FrontEnd.DataServices.Models.DeletedBooking.CommandModels;
using LogisticsBooking.FrontEnd.DataServices.Models.DeletedBooking.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.TransportersList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using OrderViewModel = LogisticsBooking.FrontEnd.DataServices.Models.OrderViewModel;

namespace LogisticsBooking.FrontEnd.Acquaintance
{
    public interface IBookingDataService
    {
        Task<Response> CreateBooking(CreateBookingCommand booking);
        Task<BookingsListViewModel> GetBookings();
        Task<BookingViewModel> GetBookingById(Guid id);
        Task<Response> UpdateBooking(UpdateBookingCommand booking);
        Task<Response> DeleteBooking(Guid id);
        Task<BookingsListViewModel> GetBookingsInbetweenDates(DateTime from, DateTime to);
    }

    public interface IDeletedBookingDataService
    {
        Task<DeletedBookingsListViewModel> GetDeletedBookings();
        Task<DeletedBookingViewModel> GetDeletedBookingById(Guid id);
        Task<Response> UpdateDeletedBooking(UpdateDeletedBookingCommand command);
        Task<Response> DeleteBooking(Guid id);
    }

    public interface IScheduleDataService
    {
        Task<Response> CreateSchedule(CreateScheduleCommand schedule);
        Task<SchedulesListViewModel> GetSchedules();
        Task<ScheduleViewModel> GetScheduleById(Guid id);
        Task<Response> UpdateSchedule(ScheduleViewModel schedule);
        Task<Response> DeleteSchedule(Guid id);
        Task<Response> CreateManySchedule(CreateManyScheduleCommand schedule);
        Task<SchedulesListViewModel> GetScheduleBydate(DateTime date);
    }

    public interface IOrderDataService
    {
        Task<Response> CreateOrder(OrderViewModel order);
        Task<List<OrderViewModel>> GetOrders();
        Task<OrderViewModel> GetOrderById(Guid id);
        Task<Response> UpdateOrder(UpdateOrderCommand order);
        Task<Response> DeleteOrder(Guid id);
    }

    public interface ITransporterDataService
    {
        Task<Response> CreateTransporter(TransporterViewModel _transporter);
        Task<Response> UpdateTransporter(Guid id, TransporterViewModel transporter);
        Task<TransportersListViewModel> ListTransporters(int page, int pageSize);
        Task<TransporterViewModel> GetTransporterById(Guid id);
        Task<Response> DeleteTransporter(Guid id);
        Task<TransporterViewModel> GetTransporterByName(string name);
    }

    public interface ISupplierDataService
    {
        Task<Response> CreateSupplier(CreateSupplierViewModel _supplier);
        Task<Response> UpdateSupplier(Guid id, SupplierViewModel supplier);
        Task<Response> DeleteSupplier(Guid id);
        Task<SupplierViewModel> GetSupplierById(Guid id);
        Task<SuppliersListViewModel> ListSuppliers(int page, int pageSize);
        Task<SupplierViewModel> GetSupplierByName(string name);
    }

    public interface IUtilBookingDataService
    {
        Task<UtilBooking> GetBookingNumber();
    }

    public interface IIntervalDataService
    {
        Task<IntervalViewModel> GetIntetvalById(Guid id);
    }

    public interface IMasterScheduleDataService
    {
        Task<Response> CreateNewMasterSchedule(CreateNewMasterScheduleStandardCommand createMasterScheduleCommand);

        Task<MasterSchedulesStandardViewModel> GetAllSchedules();

        Task<Response> SetMasterScheduleActive(SetMasterScheduleStandardActiveCommand masterScheduleStandardActive);

        Task<MasterSchedulesStandardViewModel> GetActiveMasterSchedule();

        Task<Response> DeleteMasterScheduleStandard(Guid masterScheduleStandardId);
        
        Task<MasterScheduleStandardViewModel> GetMasterScheduleById(Guid masterScheduleStandardId);
    }

    public interface IApplicationUserDataService
    {
        Task<ListApplicationUserViewModels> GetAllUsers();
        Task<Response> CreateUser(CreateUserCommand createUserCommand);

        Task<ApplicationUserViewModel> GetUserById(GetUserByIdCommand getUserByIdCommand);

        Task<Response> UpdateUser(ApplicationUserViewModel applicationUserViewModel);

        Task<Response> UpdateUserRole(UpdateRoleCommand updateRoleCommand);

        Task<Response> DeleteUser(Guid id);

        Task<Response> CreateTransporter(CreateUserCommand createUserCommand);
    }


    public interface IDashboardDataService
    {
        Task<DashboardViewModel> GetDashboard();
    }

    public interface ITransporterBookingsDataService
    {
        Task<BookingsListViewModel> GetOldBookingsByTransporter(Guid TransporterId);
        Task<BookingsListViewModel> GetBookingsByTransporter(Guid TransporterId);
    }
}
