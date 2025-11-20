using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net.Http.Json;

namespace Store.Client.Pages
{
    public partial class AddProduct : ComponentBase
    {
        [Inject] 
        public HttpClient HttpClient { get; set; }

        [Inject] 
        public NavigationManager NavManager { get; set; }

        public ProductModel Product { get; set; } = new ProductModel();

        protected async Task SaveProduct()
        {
            var response = await this.HttpClient.PostAsJsonAsync("api/products", this.Product);

            if (response.IsSuccessStatusCode)
            {
                this.NavManager.NavigateTo("/products");
            }
        }

        protected void BackToProducts()
        {
            this.NavManager.NavigateTo("/products");
        }
    }
}
