using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ThAmCo.Catering.Models;

namespace ThAmCo.CateringFacade.Food
{
    public class FoodManagement : IFoodManagement
    {

        private readonly ILogger<FoodManagement> _logger;
        private readonly IConfiguration _config;

        private HttpClient _client = null;

        public FoodManagement(ILogger<FoodManagement> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        private void EnsureClient()
        {
            if (_client != null)
                return;

            _client = new HttpClient()
            {
                BaseAddress = new Uri(_config["CateringBaseUrl"]),
                Timeout = TimeSpan.FromSeconds(5)
            };
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        public async Task<FoodGetDto> GetFood(int id)
        {
            EnsureClient();

            FoodGetDto foodGetDto = null;
            try
            {
                var response = await _client.GetAsync("api/food/"+id);
                response.EnsureSuccessStatusCode();
                foodGetDto = await response.Content.ReadAsAsync<FoodGetDto>();
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught exception whilst getting food id: " + id + ". Exception: " + ex.Message);
            }

            return foodGetDto;
        }


        public async Task<FoodGetDto> AddFood(Catering.Data.Food food)
        {
            EnsureClient();

            FoodGetDto foodGetDto = null;
            try
            {
                var response = await _client.PostAsJsonAsync("api/food", food);
                response.EnsureSuccessStatusCode();
                foodGetDto = await response.Content.ReadAsAsync<FoodGetDto>();
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught exception whilst adding food: " + food + ". Exception: " + ex.Message);
            }

            return foodGetDto;
        }

        public async Task<bool> UpdateFood(int foodId, Catering.Data.Food food)
        {
            EnsureClient();

            bool success = false;
            try
            {
                var response = await _client.PutAsJsonAsync("api/food/"+foodId, food);
                response.EnsureSuccessStatusCode();
                success = true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught exception whilst updating food: " + food + ". Exception: " + ex.Message);
            }

            return success;
        }

        public async Task<bool> RemoveFood(int foodId)
        {
            EnsureClient();

            bool success = false;
            try
            {
                var response = await _client.DeleteAsync("api/food/" + foodId);
                response.EnsureSuccessStatusCode();
                success = true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught exception whilst deleting food ID: " + foodId + ". Exception: " + ex.Message);
            }

            return success;
        }


        ~FoodManagement()
        {
            if (_client != null)
                _client.Dispose();
                _client = null;
        }
    }
}
