using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepository;
using System.Collections.Generic;

namespace Repositories.Repository
{
    public class AccountRepo : IAccountRepo
    {
        // Xóa phương thức này
        // public SystemAccount GetAccountByID(int id) => SystemAccountManager.Instance.GetAccountById(id);

        // Giữ lại các phương thức khác
        public void ChangeStatus(SystemAccount account) => SystemAccountManager.Instance.ChangeStatus(account);
        public void DeleteAccount(SystemAccount account) => SystemAccountManager.Instance.Remove(account);
        public IEnumerable<SystemAccount> GetAccounts() => SystemAccountManager.Instance.GetSystemAccountList();
        public void InsertAccount(SystemAccount account) => SystemAccountManager.Instance.AddNew(account);
        public IEnumerable<SystemAccount> Search(string search) => SystemAccountManager.Instance.Search(search);
        public void UpdateAccount(SystemAccount account) => SystemAccountManager.Instance.Update(account);
    }
}
