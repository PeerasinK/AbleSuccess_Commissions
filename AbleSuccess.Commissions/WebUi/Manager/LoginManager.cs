using AbleSuccess.Commissions.Data;
using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AbleSuccess.Commissions.WebUi.Manager
{
    public class LoginManager
    {
        private readonly IUnitOfWork _unitOfWork;

        #region [CONSTRUCTOR]

        public LoginManager()
        {
            _unitOfWork = new AbleSuccessUnitOfWork(Helper.ConnectionString);
        }

        #endregion

        #region [PUBLIC METHODS]

        public LoginModel Authenticate(string username, string password)
        {
            string hashedPassword = Helper.GetHash(password);
            Mst_Credential credential = _unitOfWork.GetRepository<Mst_Credential>()
                .GetQueryable().FirstOrDefault(o => o.Username == username && o.Password == hashedPassword && o.Status == 1);
            return credential == null ? null : new LoginModel
            {
                UserId = credential.UserId,
                Username = credential.Username,
                Role = credential.Role,
                Password = credential.Password,
            };
        }

        #endregion

        #region [PRIVATE METHOD]



        #endregion
    }
}