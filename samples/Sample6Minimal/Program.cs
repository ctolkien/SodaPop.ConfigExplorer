using SodaPop.ConfigExplorer;

var builder = WebApplication.CreateBuilder(args);

// configure the global config explorer
builder.Services.AddConfigExplorer(options =>
{
    options.LocalHostOnly = false;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // add config explorer for global configuration (configured above)
    app.UseConfigExplorer();

    // add config explorer for subsection with explicit configuration
    app.UseConfigExplorer(app.Configuration.GetSection("Tier1"), new ConfigExplorerOptions
    {
        PathMatch = "/config/example",
    });
}

app.MapGet("/hello", () =>
{
    return Results.Ok("Hello world!");
});

app.MapGet("/", () =>
{
    return Results.Text("""
        <!DOCTYPE html><html>
        <head><title>SodaPop.ConfigExplorer.Sample</title></head>
        <body>
        <p>Browse to the global demo here: <a href="/config">Show global configuration</a></p>
        <p>Browse to the subsection demo here: <a href="/config/example">Show 'Tier1' section configuration</a></p>
        </body>
        </html>
        """, "text/html");
});

app.Run();
