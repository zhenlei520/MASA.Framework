[中](README.zh-CN.md) | EN

## Masa.Contrib.Data.EFCore.MySql

Example:

``` powershelll
Install-Package Masa.Contrib.Data.EFCore.MySql
Install-Package Masa.Contrib.Data.Contracts.EFCore //Use the data filtering and soft delete capabilities provided by the protocol, if you don't need it, you can not refer to it
```

### Usage 1

1. Configure appsettings.json

``` appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;port=3306;Database=identity;Uid=myUsername;Pwd=P@ssw0rd;"
  }
}
```

2. Register `MasaDbContext`

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder =>
{
    optionsBuilder.UseFilter(); //Enable filtering, provided by Masa.Contrib.Data.Contracts.EFCore
    optionsBuilder.UseMySQL(); //Use MySQL database
});
```

### Usage 2

``` C#
builder.Services.AddMasaDbContext<CustomDbContext>(optionsBuilder =>
{
    optionsBuilder.UseFilter(); //Enable filtering, provided by Masa.Contrib.Data.Contracts.EFCore
    optionsBuilder.UseMySQL("Server=localhost;port=3306;Database=identity;Uid=myUsername;Pwd=P@ssw0rd;"); //Use MySQL database
});
```

> For the link string, please refer to: https://www.connectionstrings.com/mysql