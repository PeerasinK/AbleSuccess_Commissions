
using System.Collections.Generic;

namespace AbleSuccess.Commissions.WebUi.Models
{
    public class EmployeeModel : BaseModel
    {
        public int ProfileId { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public int Position { get; set; }

        public string PositionName { get; set; }

        public int Devision { get; set; }

        public string DevisionName { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public string Photo { get; set; }
    }

    public class EmployeeCollectionViewModel : BaseModel
    {
        public int? SearchPosition { get; set; }

        public int? SearchDevision { get; set; }

        public string SearchEmployeeName { get; set; }

        public List<EmployeeModel> EmployeeCollection { get; set; }

        public List<LookupModel> LookupPosition { get; set; }

        public List<LookupModel> LookupDevision { get; set; }
    }

    public class EmployeeDetailViewModel : EmployeeModel
    {
        public List<LookupModel> LookupPosition { get; set; }

        public List<LookupModel> LookupDevision { get; set; }

    }
}