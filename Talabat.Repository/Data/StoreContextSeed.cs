using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedingAsync(StoreContext _dbContext)
        {
            if (_dbContext.ProductBrands?.Count()==0)
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

                foreach (var brand in Brands)
                {
                    _dbContext.Set<ProductBrand>().Add(brand);
                }
                await _dbContext.SaveChangesAsync(); 
            }

            if (_dbContext.productCategories?.Count() == 0)
            {
                var CategoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);

                foreach (var Category in Categories)
                {
                    _dbContext.Set<ProductCategory>().Add(Category);
                }
                await _dbContext.SaveChangesAsync();
            }

            if (_dbContext.Products?.Count() == 0)
            {
                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                foreach (var Product in Products)
                {
                    _dbContext.Set<Product>().Add(Product);
                }
                await _dbContext.SaveChangesAsync();
            }


            if (_dbContext.deliveryMethods?.Count() == 0)
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeeding/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);

                foreach (var delivery in deliveryMethods)
                {
                    _dbContext.Set<DeliveryMethod>().Add(delivery);
                }
                await _dbContext.SaveChangesAsync();
            }

        }


    }
}
