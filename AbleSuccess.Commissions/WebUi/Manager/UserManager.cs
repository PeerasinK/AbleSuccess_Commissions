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
using System.Web.Mvc;

namespace AbleSuccess.Commissions.WebUi.Manager
{
    public class UserManager
    {
        private readonly IUnitOfWork _unitOfWork;

        private const string FILE_PATH = @"\Contents\UserImages\";

        #region [CONSTRUCTOR]

        public UserManager()
        {
            _unitOfWork = new AbleSuccessUnitOfWork(Helper.ConnectionString);
        }

        #endregion

        #region [PUBLIC METHODS]

        public List<UserDetailViewModel> GetUserDetailList()
        {
            return (from c in _unitOfWork.GetRepository<Mst_Credential>().GetQueryable(o => o.Status == 1)
                    join p in _unitOfWork.GetRepository<Mst_Profile>().GetQueryable() on c.UserId equals p.UserId

                    select new UserDetailViewModel
                    {
                        // Mst_Credential
                        UserId = c.UserId,
                        UserCode = c.UserCode,
                        Username = c.Username,
                        Role = c.Role,
                        Status = c.Status,
                        // Mst_Profile
                        ProfileId = p.ProfileId,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Devision = p.Devision,
                        Position = p.Position
                    }).ToList();
        }

        public UserCollectionViewModel GetUserList(UserCollectionViewModel model = null)
        {
            if (model == null) model = new UserCollectionViewModel();

            // Get data
            var query = _unitOfWork.GetRepository<Mst_Credential>().GetQueryable();

            // Set Where cause
            if (!string.IsNullOrWhiteSpace(model.SearchUsername))
                query = query.Where(o => o.Username.StartsWith(model.SearchUsername));

            if (model.SearchUserStatus != null && model.SearchUserStatus >= 0)
                query = query.Where(o => o.Status == model.SearchUserStatus);

            if (model.SearchUserRole != null && model.SearchUserRole >= 0)
                query = query.Where(o => o.Role == model.SearchUserRole);

            // Mapping dto with model
            model.UserCollection = query.Select(o => new UserModel()
            {
                UserId = o.UserId,
                UserCode = o.UserCode,
                Username = o.Username,
                Role = o.Role,
                Status = o.Status
            }).ToList();

            // Set Lookup
            model.LookupUserStatus = Helper.LookupUserStatus;
            model.LookupRole = Helper.LookupRole;

            // Mapping Id with lookup name
            for (int i = 0; i < model.UserCollection.Count; i++)
            {
                UserModel user = model.UserCollection[i];

                user.RoleName = Helper.GetNameFromLookup(user.Role, model.LookupRole);
                user.StatusName = Helper.GetNameFromLookup(user.Status, model.LookupUserStatus);

                model.UserCollection[i] = user;
            }

            return model;
        }

        public UserDetailViewModel GetUserDetail(int id)
        {
            UserDetailViewModel model;

            // Get data
            model = (from c in _unitOfWork.GetRepository<Mst_Credential>().GetQueryable(o => o.UserId == id)
                     join p in _unitOfWork.GetRepository<Mst_Profile>().GetQueryable(o => o.UserId == id) on c.UserId equals p.UserId

                     select new UserDetailViewModel
                     {
                         // Mst_Credential
                         UserId = c.UserId,
                         UserCode = c.UserCode,
                         Username = c.Username,
                         Role = c.Role,
                         Status = c.Status,
                         // Mst_Profile
                         FirstName = p.FirstName,
                         LastName = p.LastName,
                         Devision = p.Devision,
                         Position = p.Position

                     }).FirstOrDefault();

            if (model != null)
            {
                // Set Lookup
                model.LookupUserStatus = Helper.LookupUserStatus;
                model.LookupRole = Helper.LookupRole;
                model.LookupPosition = Helper.LookupPosition;
                model.LookupDevision = Helper.LookupDevision;

                // Mapping Id with lookup name
                model.RoleName = Helper.GetNameFromLookup(model.Role, model.LookupRole);
                model.StatusName = Helper.GetNameFromLookup(model.Status, model.LookupUserStatus);
                model.PositionName = Helper.GetNameFromLookup(model.Position, model.LookupPosition);
                model.DevisionName = Helper.GetNameFromLookup(model.Devision, model.LookupDevision);
            }

            return model;
        }

        public void CreateUser(UserDetailViewModel model)
        {
            // Check username
            Mst_Credential cred = _unitOfWork.GetRepository<Mst_Credential>().GetQueryable(o => o.Username == model.Username).FirstOrDefault();
            if (cred != null) throw new ArgumentException("Username already exists");

            // Insert Credential
            Mst_Credential credential = MappingUserDetailViewModelToCredential(model);
            _unitOfWork.GetRepository<Mst_Credential>().Insert(credential);
            _unitOfWork.Execute();

            // Insert Profile
            model.UserId = credential.UserId;
            Mst_Profile profile = MappingUserDetailViewModelToProfile(model);
            _unitOfWork.GetRepository<Mst_Profile>().Insert(profile);
            _unitOfWork.Execute();

            // Create default image
            CopyNoImageFile(model.Username);
        }

        public void UpdateUser(UserDetailViewModel model)
        {
            bool isChangeUsername = false;

            // Check username
            Mst_Credential cred = _unitOfWork.GetRepository<Mst_Credential>()
                .GetQueryable(o => o.Username == model.Username && o.UserId != model.UserId).FirstOrDefault();
            if (cred != null) throw new ArgumentException("Username already exists");

            // Update Credential
            IRepository<Mst_Credential> repoCredential = _unitOfWork.GetRepository<Mst_Credential>();
            Mst_Credential credential = repoCredential.GetQueryable(o => o.UserId == model.UserId).FirstOrDefault();
            if (credential != null)
            {
                if (credential.Username != model.Username)
                {
                    credential.Username = model.Username;
                    isChangeUsername = true;
                }
                credential.UserCode = model.UserCode;
                credential.Role = model.Role;
                credential.Status = model.Status;
                repoCredential.Update(credential);
            }

            // Update Profile
            IRepository<Mst_Profile> repoProfile = _unitOfWork.GetRepository<Mst_Profile>();
            Mst_Profile profile = repoProfile.GetQueryable(o => o.UserId == model.UserId).FirstOrDefault();
            if (profile != null)
            {
                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                profile.Position = model.Position;
                profile.Devision = model.Devision;
                profile.Status = model.Status;
                if (isChangeUsername) profile.Photo = string.Format("..{0}{1}.jpg", FILE_PATH, model.Username);
                repoProfile.Update(profile);
            }

            _unitOfWork.Execute();

            // Create default image
            if (isChangeUsername) CopyNoImageFile(model.Username);
        }

        public void UpdateUserStatus(int id, int status)
        {
            IRepository<Mst_Credential> repoCred = _unitOfWork.GetRepository<Mst_Credential>();
            IRepository<Mst_Profile> repoProfile = _unitOfWork.GetRepository<Mst_Profile>();

            Mst_Credential credential = repoCred.GetQueryable(o => o.UserId == id).FirstOrDefault();
            if (credential != null)
            {
                credential.Status = status;
                repoCred.Update(credential);

                Mst_Profile profile = repoProfile.GetQueryable(o => o.UserId == id).FirstOrDefault();
                if (profile != null)
                {
                    profile.Status = status;
                    repoProfile.Update(profile);
                }
            }

            _unitOfWork.Execute();
        }

        public void ResetPassword(int id)
        {
            IRepository<Mst_Credential> repo = _unitOfWork.GetRepository<Mst_Credential>();

            Mst_Credential credential = repo.GetQueryable(o => o.UserId == id).FirstOrDefault();
            if (credential != null)
            {
                credential.Password = Helper.GetHash(Helper.DefaultPassword);
                repo.Update(credential);
            }

            _unitOfWork.Execute();
        }

        #endregion

        #region [PRIVATE METHOD]

        private void CopyNoImageFile(string username)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            string path = rootPath + FILE_PATH;
            System.IO.File.Copy(path + "no-image.jpg", string.Format("{0}{1}.jpg", path, username), true);
        }

        private Mst_Credential MappingUserDetailViewModelToCredential(UserDetailViewModel model)
        {
            return new Mst_Credential
            {
                UserCode = model.UserCode,
                Username = model.Username,
                Password = Helper.GetHash(Helper.DefaultPassword),
                Role = model.Role,
                Status = model.Status
            };
        }

        private Mst_Profile MappingUserDetailViewModelToProfile(UserDetailViewModel model)
        {
            return new Mst_Profile
            {
                UserId = model.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Position = model.Position,
                Devision = model.Devision,
                Status = model.Status,
                Photo = string.Format("..{0}{1}.jpg", FILE_PATH, model.Username)
            };
        }

        #endregion
    }
}