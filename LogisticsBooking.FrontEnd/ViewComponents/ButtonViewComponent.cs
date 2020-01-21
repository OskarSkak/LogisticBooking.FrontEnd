using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsBooking.FrontEnd.ViewComponents
{
    public class ButtonViewComponent : ViewComponent
    {

       
        
        public async Task<IViewComponentResult> InvokeAsync(string label)
        {
            Button button = new Button();
            button.Name = label;
            
            return View(button);
        }
        
        
        public class Button
        {
            public string Name { get; set; }
        }
    }
}