using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThAmCo.Catering.Models;

namespace ThAmCo.CateringFacade.Food
{
    public interface IFoodManagement
    {

        Task<FoodGetDto> GetFood(int id);

        Task<FoodGetDto> AddFood(Catering.Data.Food food);

        Task<bool> UpdateFood(int foodId, Catering.Data.Food food);

        Task<bool> RemoveFood(int foodId);
    }
}
