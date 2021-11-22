using Ardalis.Specification;
using RepairMarketPlace.ApplicationCore.Entities;
using System;
using System.Linq;


namespace RepairMarketPlace.ApplicationCore.Specifications
{
    public class ShopByIdSpec : Specification<Shop>, ISingleResultSpecification<Shop>
    {
        public ShopByIdSpec(Guid userId)
        {
            Query.AsNoTracking()
                .Where(shop => shop.UserId == userId);
        }
        public ShopByIdSpec(int id)
        {
            Query.AsNoTracking()
                .Where(shop => shop.Id == id);
        }
    }
}
