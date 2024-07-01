﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JWTAuthServer.Core.Models;

namespace JWTAuthServer.Data.Configurations;

public class UserAppConfiguration : IEntityTypeConfiguration<UserApp>
{
    public void Configure(EntityTypeBuilder<UserApp> builder)
    {
        builder.Property(x => x.City).HasMaxLength(50);
    }
}
