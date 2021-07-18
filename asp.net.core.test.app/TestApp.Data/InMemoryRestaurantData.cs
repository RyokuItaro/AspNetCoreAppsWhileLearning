using System.Collections.Generic;
using System.Linq;
using TestApp.Core;

namespace TestApp.Data
{
    public class InMemoryRestaurantData : IRestaurantData
    {
        readonly List<Restaurant> restaurants;
        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>()
            {
                new Restaurant { Id = 1, Name = "Scott's Pizza", Location = "Marryland", Cuisine = CuisineType.Italian },
                new Restaurant { Id = 2, Name = "Karczma", Location = "Novigrad", Cuisine = CuisineType.Mexican },
                new Restaurant { Id = 3, Name = "Kuasa", Location = "Middleearth", Cuisine = CuisineType.Indian }
            };
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            restaurants.Add(newRestaurant);
            newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
            return newRestaurant;
        }

        public int Commit()
        {
            return 0;
        }

        public Restaurant Delete(int id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id == id);
            if(restaurant != null)
            {
                restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public Restaurant GetById(int id)
        {
            return restaurants.SingleOrDefault(r => r.Id == id);
        }

        public int GetCountRestaurants()
        {
            return restaurants.Count();
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
                if(restaurant != null)
                {
                    restaurant.Name = updatedRestaurant.Name;
                    restaurant.Location = updatedRestaurant.Location;
                    restaurant.Cuisine = updatedRestaurant.Cuisine;
                }
            return restaurant;
        }
    }
}
