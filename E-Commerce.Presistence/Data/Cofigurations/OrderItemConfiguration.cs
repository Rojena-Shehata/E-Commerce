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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price).HasPrecision(8, 2);

            builder.OwnsOne(x => x.Product, entityProduct =>
            {
                entityProduct.Property(x => x.ProductName).HasMaxLength(100);
                entityProduct.Property(x => x.PictureUrl).HasMaxLength(200);
            });
         }
    }
}
