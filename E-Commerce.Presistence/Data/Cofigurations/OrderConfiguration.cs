using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presistence.Data.Cofigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x=>x.SubTotal).HasPrecision(8,2);
            //Owned Address
            builder.OwnsOne(x => x.Address, ownedNavigationEntity =>
            {
                ownedNavigationEntity.Property(x => x.Street).HasMaxLength(50);
                ownedNavigationEntity.Property(x => x.City).HasMaxLength(50);
                ownedNavigationEntity.Property(x => x.FirstName).HasMaxLength(50);
                ownedNavigationEntity.Property(x => x.LastName).HasMaxLength(50);
                ownedNavigationEntity.Property(x => x.Country).HasMaxLength(50);
            });


        }
    }
}
