using BusinessObjects.Entities;
using DataAccessObjects;
using Repositories.IRepository;
using System.Collections.Generic;

namespace Repositories.Repository
{
    public class AccountRepo : IAccountRepo
    {
        public void ChangeStatus(SystemAccount account) => SystemAccountManager.Instance.ChangeStatus(account);

        public void DeleteAccount(SystemAccount account) => SystemAccountManager.Instance.Remove(account);

        public SystemAccount GetAccountByID(int id) => SystemAccountManager.Instance.GetAccountById(id);

        public IEnumerable<SystemAccount> GetAccounts() => SystemAccountManager.Instance.GetAccountList();

        public void InsertAccount(SystemAccount account) => SystemAccountManager.Instance.AddNew(account);

        public IEnumerable<SystemAccount> Search(string search) => SystemAccountManager.Instance.Search(search);

        public void UpdateAccount(SystemAccount account) => SystemAccountManager.Instance.Update(account);
    }
}
