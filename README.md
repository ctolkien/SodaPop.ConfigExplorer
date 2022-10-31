# Config Explorer for ASPNET Core

[![Build Status](https://dev.azure.com/chadtolkien/SodaPop.ConfigExplorer/_apis/build/status/SodaPop.ConfigExplorer?branchName=master)](https://dev.azure.com/chadtolkien/SodaPop.ConfigExplorer/_build/latest?definitionId=5&branchName=master)

[![ConfigExplorer on NuGet](https://img.shields.io/nuget/v/SodaPop.ConfigExplorer.svg?maxAge=200)](https://www.nuget.org/packages/SodaPop.ConfigExplorer)
[![MIT license](https://img.shields.io/github/license/ctolkien/SodaPop.ConfigExplorer.svg?maxAge=2592000)](LICENSE)

## Install

### Nuget

```Install-Package SodaPop.ConfigExplorer```

### dotnet CLI

`dotnet add package SodaPop.ConfigExplorer`


## Getting Started

In your `ConfigureServices` method:

```csharp
services.AddConfigExplorer();
```

In your `Configure` method:

```csharp

if (env.IsDevelopment)
{
    //config is the `IConfigurationRoot` from your `ConfigurationBuilder`
    app.UseConfigExplorer(config);
}
```

Once configured, access a diagnostic listing of all the available keys and values in the configuration system via:

```
http://localhost:port/config
```

## A Strong Warning On Security

Configuration can often contain application secrets such as connection strings. As a precautionary measure the end point will only load if the host is `localhost` and we will also filter out any configuration key which has a name containing `ConnectionString`. Even with these set, we strongly advise this middleware is only added in development environments.

## Example

![Example](https://cloud.githubusercontent.com/assets/515955/24350435/ed011456-132d-11e7-857a-10a31305eb83.png)

## Whats It Do?

The configuration system in AspNet Core is extremely flexible, but it sometimes can be hard to know what value you're going to receive. An example configuration might look like:

```csharp
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
    .AddJsonFile("someotherappsetting.json", optional: true) 
    .AddInMemoryCollection()
    .AddEnvironmentVariables()
    .Add(MyCustomSource)
    .Build();
```

Depending on where you're running, what environment variables are set on the current machine, which configuration options you're using, what _order_ the configuration items are added, which files are present or not present can result in you getting different configuration.

Compounding this, there are a number of "magic" prefixes used to hook in for Azure integration.

This tool will simply list out all the keys and values currently available in the entire configuration system.

## Available Options

```csharp
app.UseConfigExplorer(config, new ConfigExplorerOptions //optional
{
    LocalHostOnly = true, //default
    PathMatch = "/config", //default
    TryRedactConnectionStrings = true //default
});
```

