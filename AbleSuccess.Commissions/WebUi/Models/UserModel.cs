
using System.Collections.Generic;

namespace AbleSuccess.Commissions.WebUi.Models
{
    public class UserModel : BaseModel
    {
        public int UserId { get; set; }

        public string UserCode { get; set; }

        public string Username { get; set; }

        public int Role { get; set; }

        public string RoleName { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }
    }

    public class UserCollectionViewModel : BaseModel
    {
        public int? SearchUserStatus { get; set; }

        public int? SearchUserRole { get; set; }

        public string SearchUsername { get; set; }

        public List<UserModel> UserCollection { get; set; }

        public List<LookupModel> LookupUserStatus { get; set; }

        public List<LookupModel> LookupRole { get; set; }
    }

    public class UserDetailViewModel : UserModel
    {
        public int ProfileId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Position { get; set; }

        public string PositionName { get; set; }

        public int Devision { get; set; }

        public string DevisionName { get; set; }

        public List<LookupModel> LookupUserStatus { get; set; }

        public List<LookupModel> LookupRole { get; set; }

        public List<LookupModel> LookupPosition { get; set; }

        public List<LookupModel> LookupDevision { get; set; }

    }
}