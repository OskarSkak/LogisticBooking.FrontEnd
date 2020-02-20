using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class ValidationMessage
    {
        public bool IsSuccess { get;  }
        
        public List<string> Errors { get; } = new List<string>();

        public ValidationMessage( bool isSuccess , params string[] errors)
        {
            IsSuccess = isSuccess;
            foreach (var error in errors)
            {
                Errors.Add(error);
            }
        }
    }
}