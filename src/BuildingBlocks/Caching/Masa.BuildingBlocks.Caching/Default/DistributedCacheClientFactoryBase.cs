﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.Caching;

public class DistributedCacheClientFactoryBase : CacheClientFactoryBase<IDistributedCacheClient>, IDistributedCacheClientFactory
{
    protected override string DefaultServiceNotFoundMessage
        => "Default DistributedCache not found, you need to add it, like services.AddStackExchangeRedisCache()";

    protected override string SpecifyServiceNotFoundMessage => "Please make sure you have used [{0}] DistributedCache, it was not found";

    protected override MasaFactoryOptions<CacheRelationOptions<IDistributedCacheClient>> FactoryOptions => _optionsMonitor.CurrentValue;

    private readonly IOptionsMonitor<DistributedCacheFactoryOptions> _optionsMonitor;

    public DistributedCacheClientFactoryBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<DistributedCacheFactoryOptions>>();
    }
}
