using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configuration
{
	internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
	{
		public void Configure(EntityTypeBuilder<T> builder)
		{
			builder.Property(x => x.Name)
				.HasColumnType("varchar")
				.HasMaxLength(50);
			builder.Property(x => x.Email)
				.HasColumnType("varchar")
				.HasMaxLength(100);
			builder.ToTable(tb =>
				tb.HasCheckConstraint("GymUserValidEmailCheck", "Email Like '_%@_%._%'"));
			builder.Property(x => x.Phone)
					.HasColumnType("varchar")
					.HasMaxLength(11);
			builder.ToTable(tb =>
				tb.HasCheckConstraint("GymUserValidPhoneCheck", "Phone Like '01_________' and Phone not Like '%[^0-9]%'"));
		}
	}
}
