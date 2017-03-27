# Config Explorer for ASPNet Core

| Platform | Status|
|---------|-------|
|Windows  | [![Build status](https://img.shields.io/appveyor/ci/Soda-Digital/SodaPop-ConfigExplorer.svg?maxAge=200)](https://ci.appveyor.com/project/Soda-Digital/SodaPop.ConfigExplorer) |
|Linux | [![Build Status](https://img.shields.io/travis/ctolkien/SodaPop.ConfigExplorer.svg?maxAge=200)](https://travis-ci.org/ctolkien/SodaPop.ConfigExplorer) |

![Version](https://img.shields.io/nuget/v/SodaPop.ConfigExplorer.svg?maxAge=200)
![license](https://img.shields.io/github/license/ctolkien/SodaPop.ConfigExplorer.svg?maxAge=2592000)

## Install

### Nuget

```Install-Package SodaPop.ConfigExplorer -pre```

### dotnet CLI

`dotnet add package SodaPop.ConfigExplorer`


## Configuration

```csharp

if (env.IsDevelopment)
{
    //config is the `IConfigurationRoot` from your `ConfigurationBuilder`
    app.UseConfigExplorer(config);
}
```

Once configured, access a diagnostic listing all the available keys and values in the configuration system via:

```
http://localhost:port/config
```

## A Strong Warning On Security

Configuration can often contain application secrets such as connection strings. As a precuationary measure the end point will only load if the host is `localhost` and we will also filter out any configuration key which has a name containing `ConnectionString`. Even with these set, we strongly advise this middleware is only added in development environments.

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
    .Add(MyCustomSource)
    .Build();
```

Depending on where you're running, what environment variables are set, which configuration options you're using, what _order_ the cofiguration items are added, which files are present or not present can result in you getting different configuration.

Compounding this, there are a number of "magic" prefixes used to hook in for Azure integration.

This tool will simply list out all the availble keys and values currently available in the entire configuration system.

## Availble Options

```csharp
app.UseConfigExplorer(config, new ConfigExplorerOptions //optional
{
    LocalHostOnly = true, //default
    PathMatch = "/config", //default
    TryRedactConnectionStrings = true //default
});
```

