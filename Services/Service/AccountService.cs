using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Repositories.IRepository;
using Services.IService;
using BusinessObjects.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _acconutRepo;

        public AccountService(IAccountRepo accountService)
        {
            _acconutRepo = accountService;
        }

        public IEnumerable<SystemAccount> GetAccounts() => _acconutRepo.GetAccounts();
        public SystemAccount GetAccountById(int id) => _acconutRepo.GetAccountById(id);
        public void SaveAccount(SystemAccount account) => _acconutRepo.InsertAccount(account);
        public void UpdateAccount(SystemAccount account) => _acconutRepo.UpdateAccount(account);
        public void InsertAccount(SystemAccount account) => _acconutRepo.InsertAccount(account);
        public void DeleteAccount(SystemAccount account) => _acconutRepo.DeleteAccount(account);
        public void ChangeStatus(SystemAccount account) => _acconutRepo.ChangeStatus(account);
        public IEnumerable<SystemAccount> Search(string search) => _acconutRepo.Search(search);
  
       
       
    }
}