using Services.DTOs;

public interface ICategoryService
{
    IEnumerable<CategoryDTO> GetCategories();
    CategoryDTO GetCategoryById(int id);
    void InsertCategory(CategoryDTO categoryDTO);
    void UpdateCategory(CategoryDTO categoryDTO);
    void SaveCategory(CategoryDTO categoryDTO);
    void DeleteCategory(int id);
    void ChangeStatus(int id);
    IEnumerable<CategoryDTO> Search(string search);
}