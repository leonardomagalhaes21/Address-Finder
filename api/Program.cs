var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/address/{postcode}", async (String postcode) =>
{
    var address = await AddressFinder.FindAddress(postcode);
    if (address == null)
    {
        return Results.NotFound(new { message = "Address not found." });
    }
    return Results.Ok(address);
})
.WithName("GetAddressFromPostcode")
.WithOpenApi();

app.MapGet("/api/history", () =>
{
    var history = SearchHistory.GetSearchHistory();
    return Results.Ok(history);
})
.WithName("GetSearchHistory")
.WithOpenApi();


app.MapPost("/api/history", async (string postcode) =>
{
    var address = await AddressFinder.FindAddress(postcode);
    if (address == null)
    {
        return Results.NotFound(new { message = "Invalid Postcode." });
    }
    SearchHistory.AddSearchHistory(postcode);
    return Results.Ok();
})
.WithName("AddSearchHistory")
.WithOpenApi();

app.Run();

