using BusinessObjects.Entities;
using System.Collections.Generic;

namespace Repositories.IRepository
{
    public interface IAccountRepo
    {
        IEnumerable<SystemAccount> GetAccounts();
        SystemAccount GetAccountByID(int id);
        void InsertAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(SystemAccount account);
        void ChangeStatus(SystemAccount account);
        IEnumerable<SystemAccount> Search(string search);
    }
}
