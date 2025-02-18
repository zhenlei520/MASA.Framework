﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

namespace Masa.BuildingBlocks.Authentication.Identity;

public interface IMultiEnvironmentUserContext : IUserContext
{
    string? Environment { get; }
}
