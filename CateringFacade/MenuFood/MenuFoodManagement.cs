using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Catering.Models;

namespace ThAmCo.CateringFacade.MenuFood
{
    public class MenuFoodManagement : IMenuFoodManagement
    {

        private ILogger<MenuFoodManagement> _logger;
        private IConfiguration _config;
        private HttpClient _client = null;

        public MenuFoodManagement(ILogger<MenuFoodManagement> logger, IConfiguration config)
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

        public async Task<MenuFoodGetDto> AddToMenu(int menuId, int foodId)
        {
            EnsureClient();
            MenuFoodGetDto dto = null; 
            try
            {
                var response = await _client.PostAsJsonAsync("api/menufood", new { menuId, foodId});
                response.EnsureSuccessStatusCode();
                dto = await response.Content.ReadAsAsync<MenuFoodGetDto>();
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when adding food ("+foodId+") to a menu ("+menuId+"). Exception: " + ex);
            }
            return dto;
        }

        public async Task<List<MenuFoodGetDto>> GetAllEntries()
        {
            EnsureClient();
            List<MenuFoodGetDto> dto = null;
            try
            {
                var response = await _client.GetAsync("api/menufood");
                response.EnsureSuccessStatusCode();
                dto = await response.Content.ReadAsAsync<List<MenuFoodGetDto>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when getting all MenuFood entries. Exception: " + ex);
            }
            return dto;
        }

        public async Task<bool> RemoveFromMenu(int menuId, int foodId)
        {
            EnsureClient();
            bool success = false;
            try
            {
                var response = await _client.DeleteAsync("api/menufood/"+menuId+"/"+foodId);
                response.EnsureSuccessStatusCode();
                success = true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when removing MenuFood entry ("+menuId+","+foodId+"). Exception: " + ex);
            }
            return success;
        }
    }
}
