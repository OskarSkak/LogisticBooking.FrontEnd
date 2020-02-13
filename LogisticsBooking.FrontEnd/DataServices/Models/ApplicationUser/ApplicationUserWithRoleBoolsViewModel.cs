namespace LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser
{
    public class ApplicationUserWithRoleBoolsViewModel
    {
        public string ApplicationUserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsWarehouse { get; set; }
        public bool IsTransporter { get; set; }
        public bool IsClient { get; set; }
        public bool IsOffice { get; set; }

        public ApplicationUserWithRoleBoolsViewModel(){}
        
        public ApplicationUserWithRoleBoolsViewModel(ApplicationUserViewModel user)
        {
            foreach (var role in user.ActiveRoles)
            {
                if (role.Name.ToLower() == "kontor") IsOffice = true;
                if (role.Name.ToLower() == "lager") IsWarehouse = true;
                if (role.Name.ToLower() == "transporter") IsTransporter = true;
                if (role.Name.ToLower() == "client") IsClient = true;
                if (role.Name.ToLower() == "admin") IsAdmin = true; 
            }

            ApplicationUserId = user.ApplicationUserId;
            Email = user.Email;
            Name = user.Name;
        }
    }
}
