using System;
using System.Collections.Generic;
using System.Text;

namespace KimmysCredentialing.Models
{
    public class Office
    {   
        public int OfficeId { get; set; }
        public string Name { get; set; }

        public string OfficeManager { get; set; }

        public List<Provider> Providers { get; set;} = new();
        public string NPI { get; set; }
        public string Location { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get ; set; }
        public string City { get; set; }
        public string State {  get; set; }
        public string ZipCode {  get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


    }
}
