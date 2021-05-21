# Callcounter .NET integration package

This package can be used to gather API request & response data from ASP.NET Core based applications and send it to Callcounter.

Callcounter is an API analytics platform that collect information about requests (calls) to your API using so-called
integrations. Integrations come in the form of a Ruby gem, a Nuget package, a Pip module, etcetera. The integrations
can send the data to Callcounter using an API, which is described at: https://callcounter.eu/pages/api

After collecting data, the web interface can then be used to view all kinds of metrics, giving you insight in the
(mis)usage of your API.

## Install

Run `dotnet add package Callcounter` to add the latest available version to your project.

## Configuration

Configure callcounter with the following key, this can be placed in your `appsettings.json`.

```json
{
  "CallcounterProjectToken": "/* fill with your project token */"
}
```

Now extend your `Startup.cs` with a number of calls:

```
...

using Callcounter.Net;

public void ConfigureServices(IServiceCollection services)
{
	...

	services.AddCallcounter();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
	...

	app.UseCallcounter();
}
```

After deploying you should start seeing data in Callcounter. Note that this might take some time because this package
only sends data every few requests or every few minutes.

## Bug reporting

Bugs can be reported through the Github issues found at: https://github.com/jessetilro/callcounter.net/issues

## Releasing

- Verify tests pass.
- Increment version number in: `Callcounter.Net/Callcounter.Net/CallCounter.Net.csproj`
- Increment user agent version number in: `Callcounter.Net/Callcounter.Net/CallcounterMiddleWare.cs`
- Commit all changes.
- Create a git tag for the release.
- Push the git tag.
- Build the nuget package: `dotnet build --configuration Release`
- Upload the generated file to nuget.org

## About Callcounter

[Callcounter](https://callcounter.eu) is a service built by [Webindie](https://webindie.nl) that
helps API providers with debugging and optimising the usage of their APIs.
