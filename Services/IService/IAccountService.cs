
using BusinessObjects.Entities;
using System.Collections.Generic;

namespace Services.IService
{
    public interface IAccountService
    {
        IEnumerable<SystemAccount> GetAccounts();
        void SaveAccount(SystemAccount account);
        void InsertAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(SystemAccount account);
        void ChangeStatus(SystemAccount account);
        IEnumerable<SystemAccount> Search(string search);
    }
}
