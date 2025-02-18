using BusinessObjects.Entities;
using System.Collections.Generic;

namespace Services.IService
{
    public interface IAccountService
    {
        // Lấy tất cả tài khoản
        IEnumerable<SystemAccount> GetAccounts();

        // Lấy tài khoản theo ID
        SystemAccount GetAccountById(int id);

        // Lưu tài khoản mới
        void SaveAccount(SystemAccount account);

        // Thêm tài khoản mới (Insert)
        void InsertAccount(SystemAccount account);

        // Cập nhật tài khoản
        void UpdateAccount(SystemAccount account);

        // Xóa tài khoản
        void DeleteAccount(SystemAccount account);

        // Thay đổi trạng thái của tài khoản
        void ChangeStatus(SystemAccount account);

        // Tìm kiếm tài khoản
        IEnumerable<SystemAccount> Search(string search);

        // Kiểm tra tài khoản có tồn tại không
        bool AccountExists(int id);
    }
}
