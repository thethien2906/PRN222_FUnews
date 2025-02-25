using BusinessObjects.Entities;
using Repositories.IRepository;
using Services.IService;
using Services.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo _accountRepo;

        public AccountService(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
        }

        public IEnumerable<SystemAccountDTO> GetAccounts() =>
            _accountRepo.GetAccounts().Select(account => new SystemAccountDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
                AccountPassword = account.AccountPassword
          
            });

        public SystemAccountDTO GetAccountById(int id)
        {
            if (id <= 0) throw new ArgumentException("ID must be greater than zero.", nameof(id));
            var account = _accountRepo.GetAccountById(id);
            return account != null ? new SystemAccountDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
                AccountPassword = account.AccountPassword
            } : null;
        }

        public void InsertAccount(SystemAccountDTO accountDTO)
        {
            if (accountDTO == null) throw new ArgumentNullException(nameof(accountDTO));
            if (string.IsNullOrWhiteSpace(accountDTO.AccountName)) throw new ArgumentException("AccountName cannot be empty.", nameof(accountDTO.AccountName));

            var account = new SystemAccount
            {
                AccountId = accountDTO.AccountId,
                AccountName = accountDTO.AccountName,
                AccountEmail = accountDTO.AccountEmail,
                AccountRole = accountDTO.AccountRole,
                AccountPassword = accountDTO.AccountPassword
            };
            _accountRepo.InsertAccount(account);
        }

        public void UpdateAccount(SystemAccountDTO accountDTO)
        {
            if (accountDTO == null) throw new ArgumentNullException(nameof(accountDTO));
            if (accountDTO.AccountId <= 0) throw new ArgumentException("AccountId must be greater than zero.", nameof(accountDTO.AccountId));

            var existingAccount = _accountRepo.GetAccountById(accountDTO.AccountId);
            if (existingAccount == null) throw new KeyNotFoundException($"Account with ID {accountDTO.AccountId} not found.");

            var account = new SystemAccount
            {
                AccountId = accountDTO.AccountId,
                AccountName = accountDTO.AccountName,
                AccountEmail = accountDTO.AccountEmail,
                AccountRole = accountDTO.AccountRole,
                AccountPassword = accountDTO.AccountPassword
            };
            _accountRepo.UpdateAccount(account);
        }

        public void SaveAccount(SystemAccountDTO accountDTO)
        {
            if (accountDTO == null) throw new ArgumentNullException(nameof(accountDTO));
            if (string.IsNullOrWhiteSpace(accountDTO.AccountName)) throw new ArgumentException("AccountName cannot be empty.", nameof(accountDTO.AccountName));

            var account = new SystemAccount
            {
                AccountId = accountDTO.AccountId,
                AccountName = accountDTO.AccountName,
                AccountEmail = accountDTO.AccountEmail,
                AccountRole = accountDTO.AccountRole,
                AccountPassword = accountDTO.AccountPassword
            };
            if (account.AccountId > 0 && _accountRepo.GetAccountById(account.AccountId) != null)
            {
                _accountRepo.UpdateAccount(account);
            }
            else
            {
                _accountRepo.InsertAccount(account);
            }
        }

        public void DeleteAccount(int id)
        {
            if (id <= 0) throw new ArgumentException("ID must be greater than zero.", nameof(id));
            var account = _accountRepo.GetAccountById(id);
            if (account == null) throw new KeyNotFoundException($"Account with ID {id} not found.");
            _accountRepo.DeleteAccount(account);
        }

        public void ChangeStatus(int id)
        {
            if (id <= 0) throw new ArgumentException("ID must be greater than zero.", nameof(id));
            var account = _accountRepo.GetAccountById(id);
            if (account == null) throw new KeyNotFoundException($"Account with ID {id} not found.");
            _accountRepo.ChangeStatus(account);
        }

        public IEnumerable<SystemAccountDTO> Search(string search) =>
            _accountRepo.Search(search).Select(account => new SystemAccountDTO
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole,
                AccountPassword = account.AccountPassword
            });
    }
}