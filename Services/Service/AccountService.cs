using BusinessObjects.Entities;
using DataAccessObjects.AppDbContext;
using Services.IService;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly FunewsManagementContext _context;

        public AccountService(FunewsManagementContext context)
        {
            _context = context;
        }

        public IEnumerable<SystemAccount> GetAccounts()
        {
            return _context.SystemAccounts.ToList(); // Lấy tất cả tài khoản
        }

        public SystemAccount GetAccountById(int id)
        {
            return _context.SystemAccounts.FirstOrDefault(a => a.AccountId == id); // Lấy tài khoản theo ID
        }

        public void SaveAccount(SystemAccount account)
        {
            _context.SystemAccounts.Add(account); // Thêm tài khoản mới
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        public void InsertAccount(SystemAccount account)
        {
            _context.SystemAccounts.Add(account); // Thêm tài khoản mới
            _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
        }

        public void UpdateAccount(SystemAccount account)
        {
            _context.SystemAccounts.Update(account); // Cập nhật tài khoản
            _context.SaveChanges(); // Lưu thay đổi
        }

        public void DeleteAccount(SystemAccount account)
        {
            _context.SystemAccounts.Remove(account); // Xóa tài khoản
            _context.SaveChanges(); // Lưu thay đổi
        }

        public void ChangeStatus(SystemAccount account)
        {
            // Giả sử bạn có một trường trạng thái trong tài khoản để thay đổi trạng thái tài khoản
            _context.SystemAccounts.Update(account); // Cập nhật trạng thái tài khoản
            _context.SaveChanges(); // Lưu thay đổi
        }

        public IEnumerable<SystemAccount> Search(string search)
        {
            return _context.SystemAccounts
                           .Where(a => a.AccountName.Contains(search) || a.AccountEmail.Contains(search)) // Tìm kiếm tài khoản theo tên hoặc email
                           .ToList();
        }

        public bool AccountExists(int id)
        {
            return _context.SystemAccounts.Any(a => a.AccountId == id); // Kiểm tra tài khoản tồn tại
        }
    }
}
