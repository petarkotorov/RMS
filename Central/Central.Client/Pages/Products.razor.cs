using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Central.Client.Pages
{
    public partial class Products : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject] 
        public IJSRuntime JS { get; set; }

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

        protected async Task DeleteProduct(Guid id)
        {
            var confirmed = await this.JS.InvokeAsync<bool>("confirm",
                $"Are you sure you want to delete the selected product?");

            if (!confirmed)
                return;

            var response = await this.HttpClient.DeleteAsync($"api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                this.ProductsSource = this.ProductsSource.Where(x => x.Id != id).ToList();
                this.StateHasChanged();
            }
        }

        protected void AddProduct()
        {
            this.NavManager.NavigateTo("/products/add");
        }
    }
}
