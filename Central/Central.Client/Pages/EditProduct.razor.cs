using Central.Client.Config;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net.Http.Json;

namespace Central.Client.Pages
{
    public partial class EditProduct : ComponentBase
    {
        [Inject] 
        public HttpClient HttpClient { get; set; }

        [Inject] 
        public NavigationManager NavManager { get; set; }

        [Inject]
        public ClientConfig Config { get; set; }

        [Parameter] 
        public Guid Id { get; set; }

        public ProductModel Product { get; set; } = new();

        public bool IsLoading { get; set; } = true;

        public bool ShowError { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.Product = await this.HttpClient.GetFromJsonAsync<ProductModel>($"api/products/{this.Id}");
                if (this.Product == null)
                {
                    this.ShowError = true;
                }
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

        protected async Task SaveProduct()
        {
            var response = await HttpClient.PutAsJsonAsync($"api/products/{this.Id}", this.Product);

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
