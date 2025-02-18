// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.Contrib.Data.EFCore.Tests;

public class TestBase
{
    protected IServiceCollection Services;

    [TestInitialize]
    public void Initialize()
    {
        Services = new ServiceCollection();
    }

    protected CustomDbContext CreateDbContext(bool enableSoftDelete, out IServiceProvider serviceProvider,
        bool initConnectionString = true)
    {
        Services.AddMasaDbContext<CustomDbContext>(options =>
        {
            if (enableSoftDelete)
                options.UseTestFilter();

            if (initConnectionString)
                options.UseTestSqlite($"data source=test-{Guid.NewGuid()}");
            else
                options.UseSqlite();
        });
        serviceProvider = Services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<CustomDbContext>();
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
}
