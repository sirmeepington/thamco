using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Catering.Models;

namespace ThAmCo.CateringFacade.MenuFood
{
    public interface IMenuFoodManagement
    {

        Task<List<MenuFoodGetDto>> GetAllEntries();

        Task<MenuFoodGetDto> AddToMenu(int menuId, int foodId);

        Task<bool> RemoveFromMenu(int menuId, int foodId);
    }
}
