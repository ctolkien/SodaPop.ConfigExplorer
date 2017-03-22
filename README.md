# AspNetOptionsExplorer

| Platform | Status|
|---------|-------|
|Windows  | [![Build status](https://img.shields.io/appveyor/ci/Soda-Digital/AspNetOptionsExplorer.svg?maxAge=2000)](https://ci.appveyor.com/project/Soda-Digital/AspNetOptionsExplorer) |
|Linux | [![Build Status](https://img.shields.io/travis/ctolkien/AspNetOptionsExplorer.svg?maxAge=2000)](https://travis-ci.org/ctolkien/AspNetOptionsExplorer) |

[![codecov](https://codecov.io/gh/ctolkien/AspNetOptionsExplorer/branch/master/graph/badge.svg)](https://codecov.io/gh/ctolkien/AspNetOptionsExplorer)
![Version](https://img.shields.io/nuget/v/AspNetOptionsExplorer.svg?maxAge=2000)
[![license](https://img.shields.io/github/license/ctolkien/AspNetOptionsExplorer.svg?maxAge=2592000)]()

## Install

### Nuget

```Install-Package <coming soon>```

### dotnet CLI

`dotnet add package <coming soon>`


## Configuration

```csharp

if (env.IsDevelopment)
{
    //config is the `IConfigurationRoot` from the `ConfigurationBuilder`
    app.UseAspNetOptionsExplorer(config);
}
```

Once configured access a diagnostic listing all the available keys and values in the configuration system via:

```
http://localhost:port/options
```

## A Strong Warning On Security

Configuration can often contain application secrets such as connection strings. As a precuationary measure the end point will only load if the host is `localhost` and we will also filter out any configuration key which has a name container `ConnectionString`. Even with these set, we strongly advise this middleware is only added in development environments.

## What's It Do?

The configuration system in AspNet Core is extremely flexible, but it sometimes can be hard to know what value you're going to receive. An example configuration might look like:

```csharp
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
    .AddInMemoryCollection()
    .Add(MyCustomSource)
    .Build();
```

Depending on where you're running, what environment variables are set, which configuration options you're using, what _order_ the cofiguration items are added can result in you getting different configuration.

Compounding this, there are a number of "magic" prefixes used to hook in for Azure integration.

This tool will simply list out all the availble keys and values currently available in the entire configuration system.



