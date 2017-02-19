using AbleSuccess.Commissions.Data;
using AbleSuccess.Commissions.WebUi.Common;
using AbleSuccess.Commissions.WebUi.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AbleSuccess.Commissions.WebUi.Manager
{
    public class EmployeeManager
    {
        private readonly IUnitOfWork _unitOfWork;

        #region [CONSTRUCTOR]

        public EmployeeManager()
        {
            _unitOfWork = new AbleSuccessUnitOfWork(Helper.ConnectionString);
        }

        #endregion

        #region [PUBLIC METHODS]

        public List<LookupModel> GetLookup()
        {
            List<LookupModel> model = new List<LookupModel>();

            // Get data
            var query = _unitOfWork.GetRepository<Mst_Profile>().GetQueryable(o => o.Status == 1);

            // Mapping dto with model
            return query.Select(o => new LookupModel()
            {
                Key = o.ProfileId.ToString(),
                Value = o.FirstName + " " + o.LastName,
            }).ToList();
        }

        public EmployeeCollectionViewModel GetEmployeeList(EmployeeCollectionViewModel model = null)
        {
            if (model == null) model = new EmployeeCollectionViewModel();


            // Get data
            var query = _unitOfWork.GetRepository<Mst_Profile>().GetQueryable(o => o.Status == 1);

            // Set Where cause
            if (!string.IsNullOrWhiteSpace(model.SearchEmployeeName))
                query = query.Where(o => o.FirstName.StartsWith(model.SearchEmployeeName) || o.LastName.StartsWith(model.SearchEmployeeName));

            if (model.SearchPosition != null && model.SearchPosition >= 0)
                query = query.Where(o => o.Position == model.SearchPosition);

            if (model.SearchDevision != null && model.SearchDevision >= 0)
                query = query.Where(o => o.Devision == model.SearchDevision);

            // Mapping dto with model
            model.EmployeeCollection = query.Select(o => new EmployeeModel()
            {
                ProfileId = o.ProfileId,
                UserId = o.UserId,
                FirstName = o.FirstName,
                LastName = o.LastName,
                NickName = o.NickName,
                Position = o.Position,
                Devision = o.Devision,
                Email = o.Email,
                Telephone = o.Telephone,
                Address = o.Address,
                Photo = o.Photo
            }).ToList();

            // Set Lookup
            model.LookupPosition = Helper.LookupPosition;
            model.LookupDevision = Helper.LookupDevision;

            // Mapping Id with lookup name
            for (int i = 0; i < model.EmployeeCollection.Count; i++)
            {
                EmployeeModel employee = model.EmployeeCollection[i];

                employee.PositionName = Helper.GetNameFromLookup(employee.Position, model.LookupPosition);
                employee.DevisionName = Helper.GetNameFromLookup(employee.Devision, model.LookupDevision);

                model.EmployeeCollection[i] = employee;
            }

            return model;
        }

        public EmployeeDetailViewModel GetEmployeeDetail(int id)
        {
            EmployeeDetailViewModel model;

            // Get data
            if (id > 0)
            {
                model = (from p in _unitOfWork.GetRepository<Mst_Profile>().GetQueryable(o => o.ProfileId == id)

                         select new EmployeeDetailViewModel
                         {
                             ProfileId = p.ProfileId,
                             UserId = p.UserId,
                             FirstName = p.FirstName,
                             LastName = p.LastName,
                             NickName = p.NickName,
                             Position = p.Position,
                             Devision = p.Devision,
                             Email = p.Email,
                             Telephone = p.Telephone,
                             Address = p.Address,
                             Photo = p.Photo

                         }).FirstOrDefault();
            }
            else
            {
                int userId = int.Parse(Helper.UserId);
                model = (from p in _unitOfWork.GetRepository<Mst_Profile>().GetQueryable()
                         join u in _unitOfWork.GetRepository<Mst_Credential>().GetQueryable(o => o.UserId == userId) on p.UserId equals u.UserId

                         select new EmployeeDetailViewModel
                         {
                             ProfileId = p.ProfileId,
                             UserId = p.UserId,
                             FirstName = p.FirstName,
                             LastName = p.LastName,
                             NickName = p.NickName,
                             Position = p.Position,
                             Devision = p.Devision,
                             Email = p.Email,
                             Telephone = p.Telephone,
                             Address = p.Address,
                             Photo = p.Photo

                         }).FirstOrDefault();
            }

            if (model != null)
            {
                // Set Lookup
                model.LookupPosition = Helper.LookupPosition;
                model.LookupDevision = Helper.LookupDevision;

                // Mapping Id with lookup name
                model.PositionName = Helper.GetNameFromLookup(model.Position, model.LookupPosition);
                model.DevisionName = Helper.GetNameFromLookup(model.Devision, model.LookupDevision);
            }

            return model;
        }

        public void UpdateEmployee(EmployeeDetailViewModel model)
        {
            // Update Profile
            IRepository<Mst_Profile> repoProfile = _unitOfWork.GetRepository<Mst_Profile>();
            Mst_Profile profile = repoProfile.GetQueryable(o => o.ProfileId == model.ProfileId).FirstOrDefault();
            if (profile != null)
            {
                //profile.FirstName = model.FirstName;
                //profile.LastName = model.LastName;
                profile.NickName = model.NickName;
                //profile.Position = model.Position;
                //profile.Devision = model.Devision;
                profile.Address = model.Address;
                profile.Email = model.Email;
                profile.Telephone = model.Telephone;
                //profile.Photo = model.Photo;
                repoProfile.Update(profile);
            }

            _unitOfWork.Execute();
        }

        public void ChangeImage(int id, byte[] image)
        {
            IRepository<Mst_Profile> repo = _unitOfWork.GetRepository<Mst_Profile>();

            Mst_Profile profile = repo.GetQueryable(o => o.UserId == id).FirstOrDefault();

            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllBytes(rootPath + @"\Content\UserImages\" + Helper.Username, image);
            profile.Photo = rootPath + @"\Content\UserImages\" + Helper.Username;
            repo.Update(profile);

            _unitOfWork.Execute();
        }

        public void ChangePassword(int id, string oldPassword, string newPassword)
        {
            IRepository<Mst_Credential> repo = _unitOfWork.GetRepository<Mst_Credential>();
            oldPassword = Helper.GetHash(oldPassword);

            Mst_Credential credential = repo.GetQueryable(o => o.UserId == id && o.Password == oldPassword).FirstOrDefault();
            credential.Password = Helper.GetHash(newPassword);
            repo.Update(credential);

            _unitOfWork.Execute();
        }

        #endregion

        #region [PRIVATE METHOD]


        #endregion
    }
}