using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net.Http.Json;

namespace Store.Client.Pages
{
    public partial class Products : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        public List<ProductModel> ProductsSource { get; set; } = new();

        public bool IsLoading { get; set; } = true;

        public bool ShowError { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.ProductsSource = await this.HttpClient.GetFromJsonAsync<List<ProductModel>>("api/products");
            }
            catch
            {
                this.ShowError = true;
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        protected void AddProduct()
        {
            this.NavManager.NavigateTo("/products/add");
        }
    }
}
