﻿
namespace AbleSuccess.Commissions.WebUi.Models
{
    public class LoginModel : BaseModel
    {
        public int UserId { get; set; }

        public int ProfileId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }
    }
}