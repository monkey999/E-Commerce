using A_Domain.Models;
using A_Domain.Repo_interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category, Guid> _categoryRepository;

        public CategoryService(IGenericRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);

            var created = await _categoryRepository.SaveChangesAsyncWithResult();

            return created > 0;
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            var category = GetCategoryByIdAsync(categoryId);

            if (category == null)
                return false;

            _categoryRepository.RemoveById(categoryId);

            var deleted = await _categoryRepository.SaveChangesAsyncWithResult();

            return deleted > 0;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAll().OrderBy(x => x.CategoryId).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _categoryRepository.FindByCondition(x => x.CategoryId.Equals(categoryId)).SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            _categoryRepository.Update(category);

            var updated = await _categoryRepository.SaveChangesAsyncWithResult();

            return updated > 0;
        }
    }
}
