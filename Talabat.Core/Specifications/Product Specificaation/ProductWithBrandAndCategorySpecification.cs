using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specificaation
{
    public class ProductWithBrandAndCategorySpecification : BaseSpecifications<Product>
    {
        // this Constructor will be used for creating object , that will be use to get all products
        public ProductWithBrandAndCategorySpecification(ProductSpecParams specParams) : base(P=>
            (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search.ToLower())) &&
            (!specParams.BrandId.HasValue  || P.BrandId == specParams.BrandId.Value) &&
            (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
        )
        {
            Inculeds.Add(P=>P.Brand);
            Inculeds.Add(P => P.Category);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
                AddOrderBy(P => P.Name);

            ApplyPagination((specParams.PageIndex-1)*specParams.PageSize , specParams.PageSize);
        }

        // this Constructor will be used for creating object , that will be use to get  products by id
        public ProductWithBrandAndCategorySpecification(int id):base(P=>P.Id==id)
        {
            Inculeds.Add(P => P.Brand);
            Inculeds.Add(P => P.Category);
        }
    }
}


