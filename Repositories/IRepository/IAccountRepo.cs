using BusinessObjects.Entities;
using System.Collections.Generic;

namespace Repositories.IRepository
{
    public interface IAccountRepo
    {
        SystemAccount GetAccountById(int id);
        void DeleteAccount(SystemAccount account);
        IEnumerable<SystemAccount> GetAccounts();
        void InsertAccount(SystemAccount account);
        IEnumerable<SystemAccount> Search(string search);
        void UpdateAccount(SystemAccount account);
        string GetNameById(int id);
    }
}