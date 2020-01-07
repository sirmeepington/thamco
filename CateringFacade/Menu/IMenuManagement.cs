using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Catering.Models;

namespace ThAmCo.CateringFacade.Menu
{
    public interface IMenuManagement
    {

        Task<MenuGetDto> GetMenu(int id);

        Task<List<MenuGetDto>> GetMenus();

        Task<MenuGetDto> UpdateMenu(int id, MenuPostDto menu);

        Task<bool> RemoveMenu(int id);

        Task<bool> CreateMenu(int id, MenuPostDto menu);

    }
}
