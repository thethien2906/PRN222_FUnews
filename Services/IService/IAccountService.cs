using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects.Helper;
using Services.DTOs;

namespace Services.IService
{
    public interface IAccountService
    {
        // Lấy tất cả tài khoản
        IEnumerable<SystemAccountDTO> GetAccounts();

        // Lấy tài khoản theo ID
        SystemAccountDTO GetAccountById(int id);

        // Thêm tài khoản mới
        void InsertAccount(SystemAccountDTO accountDTO);

        // Cập nhật tài khoản
        void UpdateAccount(SystemAccountDTO accountDTO);

        // Lưu tài khoản (có thể dùng để thêm hoặc cập nhật)
        void SaveAccount(SystemAccountDTO accountDTO);

        // Xóa tài khoản
        void DeleteAccount(int id);

        // Thay đổi trạng thái của tài khoản
        void ChangeStatus(int id);

        // Tìm kiếm tài khoản
        IEnumerable<SystemAccountDTO> Search(string search);
    }
}