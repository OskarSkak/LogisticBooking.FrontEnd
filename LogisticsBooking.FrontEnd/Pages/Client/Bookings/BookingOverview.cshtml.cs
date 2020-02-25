using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Itenso.TimePeriod;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking.CommandModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;


namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    
    public class UpdateTimeModel
    {
        public Guid Id { get; set; }
        public List<TimeSpan> Times { get; set; }
    }
    public class BookingOverviewModel : PageModel
    {
        [BindProperty] 
        public int WEEK { get; } = 7;
        [BindProperty] 
        public int MONTH { get; } = 31;
        [BindProperty] 
        public string NameOfFile { get; set; }

        private IBookingDataService bookingDataService;
        private readonly IStringLocalizer<View> _localizer;
        private readonly ILogger<BookingOverviewModel> _logger;
        [BindProperty] public BookingsListViewModel BookingsListViewModel { get; set; } = new BookingsListViewModel();
        public bool InBetweenDates { get; set; }
        public int NumberOfDays { get; set; }
        
        [BindProperty]
        public DateTime Start { get; set; }
        [BindProperty]
        public DateTime End { get; set; }

        public BookingOverviewModel(IBookingDataService _bookingDataService , IStringLocalizer<View> localizer,
            ILogger<BookingOverviewModel> logger)
        {
            bookingDataService = _bookingDataService;
            _localizer = localizer;
            _logger = logger;
        }

        
        
        public async Task OnGet()
        {
            


            //var Subjectid = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            //Console.WriteLine(Subjectid);
            /*
            var id = "7";
            var numberOfDays = 0;
            if (id != null) numberOfDays = int.Parse(id);

            NumberOfDays = numberOfDays;
            BookingsListViewModel.Bookings = new List<BookingViewModel>();

            if (numberOfDays != 0)
            {
                BookingsListViewModel = await  bookingDataService.GetBookingsInbetweenDates(DateTime.Now,
                        DateTime.Now.AddDays(numberOfDays)) 
                    ;
            }
            else
            {
                BookingsListViewModel = await bookingDataService.GetBookings();

                if (BookingsListViewModel != null)
                {
                    foreach (var booking in BookingsListViewModel.Bookings)
                    {
                        if (string.IsNullOrWhiteSpace(booking.TransporterName)) booking.TransporterName = "N/A";
                        if (string.IsNullOrWhiteSpace(booking.Email)) booking.Email = "N/A";

                        booking.ActualArrival = default(DateTime).Add(booking.ActualArrival.TimeOfDay);
                        booking.EndLoading = default(DateTime).Add(booking.EndLoading.TimeOfDay);
                        booking.StartLoading = default(DateTime).Add(booking.StartLoading.TimeOfDay);

                        foreach (var order in booking.OrdersListViewModel)
                        {
                            if (string.IsNullOrWhiteSpace(order.CustomerNumber)) order.CustomerNumber = "N/A";
                            if (string.IsNullOrWhiteSpace(order.OrderNumber)) order.OrderNumber = "N/A";
                            if (string.IsNullOrWhiteSpace(order.InOut)) order.InOut = "N/A";
                        }
                    }

                    for (var i = BookingsListViewModel.Bookings.Count - 1; i >= 0; i--)
                        if (BookingsListViewModel.Bookings[i].EndLoading.Date != default)
                            BookingsListViewModel.Bookings.Remove(BookingsListViewModel.Bookings[i]);
                }
            }
            */
            
                    }

        public async Task<IActionResult> OnPostUpdate(List<DateTime> dateTo, List<TimeSpan> actualArrival, List<TimeSpan> startLoading,
            List<TimeSpan> endLoading, List<string> id)
        {

            var counter = 0;

            foreach (var GuidID in id)
            {
                
                
                
                
                var bookingToUpdate = await bookingDataService.GetBookingById(Guid.Parse(GuidID));

                bookingToUpdate.ActualArrival = new DateTime(dateTo[counter].Year, dateTo[counter].Month, dateTo[counter].Day,
                    actualArrival[counter].Hours, actualArrival[counter].Minutes, 0);
                bookingToUpdate.StartLoading = new DateTime(dateTo[counter].Year, dateTo[counter].Month, dateTo[counter].Day,
                    startLoading[counter].Hours, startLoading[counter].Minutes, 0);
                bookingToUpdate.EndLoading = new DateTime(dateTo[counter].Year, dateTo[counter].Month, dateTo[counter].Day,
                    endLoading[counter].Hours, endLoading[counter].Minutes, 0);
                
                var response = await bookingDataService.UpdateBooking(CreateUpdateBookingCommand(bookingToUpdate));
                counter++;
            }
            

            

           
            

            return new RedirectToPageResult("BookingOverview" , new {culture = CultureInfo.CurrentCulture.Name});
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostUpdateNew([FromBody] UpdateTimeModel updateTimeModel)
        {
            var bookingToUpdate = await bookingDataService.GetBookingById(updateTimeModel.Id);
            UpdateArrivalInformationsCommand updateArrivalInformationsCommand = new UpdateArrivalInformationsCommand();
            AddTimesToBooking(updateArrivalInformationsCommand, updateTimeModel);
            updateArrivalInformationsCommand.InternalId = updateTimeModel.Id;
            var response = await bookingDataService.UpdateArrivalInformations(updateArrivalInformationsCommand);
            return new ObjectResult(HttpStatusCode.OK);
        }

        private void AddTimesToBooking(UpdateArrivalInformationsCommand bookingToUpdate, UpdateTimeModel updateTimeModel)
        {
            if (updateTimeModel.Times.Count != 3) return;
            bookingToUpdate.ActualArrival = new DateTime(bookingToUpdate.ActualArrival.Year , bookingToUpdate.ActualArrival.Month , bookingToUpdate.ActualArrival.Day , updateTimeModel.Times[0].Hours ,updateTimeModel.Times[0].Minutes , updateTimeModel.Times[0].Seconds  );
            bookingToUpdate.StartLoading = new DateTime(bookingToUpdate.StartLoading.Year , bookingToUpdate.StartLoading.Month , bookingToUpdate.StartLoading.Day , updateTimeModel.Times[1].Hours ,updateTimeModel.Times[1].Minutes , updateTimeModel.Times[1].Seconds );
            bookingToUpdate.EndLoading = new DateTime(bookingToUpdate.EndLoading.Year , bookingToUpdate.EndLoading.Month , bookingToUpdate.EndLoading.Day , updateTimeModel.Times[2].Hours ,updateTimeModel.Times[2].Minutes , updateTimeModel.Times[2].Seconds );

        }


        private UpdateBookingCommand CreateUpdateBookingCommand(BookingViewModel bookingToUpdate)
        {
            return new UpdateBookingCommand
            {
                Email = bookingToUpdate.Email,
                Port = bookingToUpdate.Port,
                ActualArrival = bookingToUpdate.ActualArrival,
                BookingTime = bookingToUpdate.ActualArrival,
                EndLoading = bookingToUpdate.EndLoading,
                ExternalId = bookingToUpdate.ExternalId,
                InternalId = bookingToUpdate.InternalId,
                StartLoading = bookingToUpdate.StartLoading,
                TotalPallets = bookingToUpdate.TotalPallets,
                TransporterId = bookingToUpdate.TransporterId,
                TransporterName = bookingToUpdate.TransporterName
            };
        }

        public async Task<ActionResult> OnPostExportExcel(DateTime Start, DateTime End)
        {
            BookingsListViewModel.Bookings = new List<BookingViewModel>();
            BookingsListViewModel = await bookingDataService.GetBookingsInbetweenDates(Start ,End);

            var from = DateTime.Now;
            var endDate = DateTime.Now.Date.ToShortDateString();

            foreach (var booking in BookingsListViewModel.Bookings)
            {
                if (booking.BookingTime < from) from = booking.BookingTime;
            }

            var fromDateString = from.ToShortDateString();

            System.IO.Stream spreadsheetStream = new System.IO.MemoryStream();
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Orders");

            worksheet.Cell(1, 1).SetValue("KUNNR");
            worksheet.Cell(1, 2).SetValue("DATO");
            worksheet.Cell(1, 3).SetValue("ORDNR");
            worksheet.Cell(1, 4).SetValue("PALLER");
            worksheet.Cell(1, 5).SetValue("TRANSPORTØR");
            worksheet.Cell(1, 6).SetValue("KONTAKTOPLYSNINGER");
            worksheet.Cell(1, 7).SetValue("BOOKETTID");
            worksheet.Cell(1, 8).SetValue("PORT");
            worksheet.Cell(1, 9).SetValue("KUNDE");
            worksheet.Cell(1, 10).SetValue("FAKTISK_\nANKOMST");
            worksheet.Cell(1, 11).SetValue("START_\nLÆSNING");
            worksheet.Cell(1, 12).SetValue("SLUT_\nLÆSNING");

            var cellX = 1;
            var cellY = 2;

            foreach (var booking in BookingsListViewModel.Bookings)
            {
                foreach (var order in booking.OrdersListViewModel)
                {
                    worksheet.Cell(cellY, cellX++).SetValue(order.CustomerNumber);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.BookingTime.ToShortDateString());
                    worksheet.Cell(cellY, cellX++).SetValue(order.OrderNumber);
                    worksheet.Cell(cellY, cellX++).SetValue(order.TotalPallets);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.TransporterName);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.Email);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.BookingTime.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.Port);
                    worksheet.Cell(cellY, cellX++).SetValue(order.SupplierViewModel.Name);
                    worksheet.Cell(cellY, cellX++).SetValue(booking.ActualArrival.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.StartLoading.ToShortTimeString());
                    worksheet.Cell(cellY, cellX++).SetValue(booking.EndLoading.ToShortTimeString());
                    cellY++;
                    cellX = 1;
                }
            }

            //Fixed column width - comes out messy if not fixed
            worksheet.ColumnWidth = 20;
            workbook.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;
            var fileName = "Ordre_FRA_" + fromDateString + "_TIL_" + endDate;

            return new FileStreamResult(spreadsheetStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName + ".xlsx"
            };
        }

        public async Task<IActionResult> OnGetToday()
        {
            var bookings = await bookingDataService.GetBookingsInbetweenDates(DateTime.Today  , DateTime.Today);

            var json = new JsonResult(bookings);

            return json;
        }
        
        public async Task<IActionResult> OnGetTomorrow()
        {
            var bookings =
                await bookingDataService.GetBookingsInbetweenDates(DateTime.Today.AddDays(1),
                    DateTime.Today.AddDays(1));

            var json = new JsonResult(bookings);

            return json;
        }

        public async Task<IActionResult> OnGetYesterday()
        {
            var bookings =
                await bookingDataService.GetBookingsInbetweenDates(DateTime.Today.AddDays(-1),
                    DateTime.Today.AddDays(-1));

            var json = new JsonResult(bookings);

            return json;
        }
        
        public async Task<IActionResult> OnGetLastWeek()
        {
            var bookings =
                await bookingDataService.GetBookingsInbetweenDates(DateTime.Today.AddDays(-7),
                    DateTime.Today);

            var json = new JsonResult(bookings);

            return json;
        }
        
        public async Task<IActionResult> OnGetLastMonth()
        {
            var bookings =
                await bookingDataService.GetBookingsInbetweenDates(DateTime.Today.AddDays(-30),
                    DateTime.Today);

            var json = new JsonResult(bookings);

            return json;
        }
        
        public async Task<IActionResult> OnGetNextWeek()
        {
            var bookings =
                await bookingDataService.GetBookingsInbetweenDates(DateTime.Today,
                    DateTime.Today.AddDays(7));

            var json = new JsonResult(bookings);

            return json;
        }
        
        public async Task<IActionResult> OnGetNextMonth()
        {
            var bookings =
                await bookingDataService.GetBookingsInbetweenDates(DateTime.Today,
                    DateTime.Today.AddDays(30));

            var json = new JsonResult(bookings);

            return json;
        }
        
        public async Task<IActionResult> OnGetCustom(DateTime start, DateTime end)
        {
            Start = start;
            End = end;

            var bookings = await bookingDataService.GetBookingsInbetweenDates(start,end);

             var json = new JsonResult(bookings);

            return json;
        }
    }
}