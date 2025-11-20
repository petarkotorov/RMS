using Shared.Models;

namespace Store.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductModel>> GetAllAsync();
        Task<ProductModel?> GetByIdAsync(Guid id);
        Task<ProductModel> CreateAsync(ProductModel model);
        Task<ProductModel> UpdateAsync(Guid id, ProductModel model);
        Task DeleteAsync(Guid id);
    }
}
