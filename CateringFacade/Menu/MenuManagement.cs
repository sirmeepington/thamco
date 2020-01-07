
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Catering.Models;

namespace ThAmCo.CateringFacade.Menu
{
    public class MenuManagement : IMenuManagement
    {
        private ILogger<MenuManagement> _logger;
        private IConfiguration _config;
        private HttpClient _client = null;

        public MenuManagement(ILogger<MenuManagement> logger, IConfiguration config)
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


        public async Task<MenuGetDto> GetMenu(int id)
        {
            EnsureClient();

            MenuGetDto menu;
            try
            {
                var response = await _client.GetAsync("api/menu/" + id);
                response.EnsureSuccessStatusCode();
                menu = await response.Content.ReadAsAsync<MenuGetDto>();
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when getting a menu at id " + id + ". Exception: " + ex);
                menu = null;
            }
            return menu;
        }

        public async Task<bool> RemoveMenu(int id)
        {
            EnsureClient();

            bool success = false;
            try
            {
                var response = await _client.DeleteAsync("api/menu/" + id);
                response.EnsureSuccessStatusCode();
                success = true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when deleting a menu at id " + id + ". Exception: " + ex);
            }
            return success;
        }

        public async Task<MenuGetDto> UpdateMenu(int id, MenuPostDto menu)
        {
            EnsureClient();

            MenuGetDto newMenu = null;
            try
            {
                var response = await _client.PutAsJsonAsync("api/menu/" + id, menu);
                response.EnsureSuccessStatusCode();
                newMenu = await response.Content.ReadAsAsync<MenuGetDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when deleting a menu at id " + id + ". Exception: " + ex);
            }
            return newMenu;
        }

        public async Task<bool> CreateMenu(int id, MenuPostDto menu)
        {
            EnsureClient();

            bool success = false;
            try
            {
                var response = await _client.PostAsJsonAsync("api/menu" + id, menu);
                response.EnsureSuccessStatusCode();
                success = true;
            } catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when creating a new menu at id " + id + ". Exception: " + ex);
            }

            return success;
        }

        public async Task<List<MenuGetDto>> GetMenus()
        {
            EnsureClient();

            List<MenuGetDto> menu;
            try
            {
                var response = await _client.GetAsync("api/menu/");
                response.EnsureSuccessStatusCode();
                menu = await response.Content.ReadAsAsync<List<MenuGetDto>>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("Caught an error when getting menus. Exception: " + ex);
                menu = null;
            }
            return menu;
        }

        ~MenuManagement()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }
}
