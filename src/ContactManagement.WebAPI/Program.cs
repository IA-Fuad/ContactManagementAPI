using ContactManagement.WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContactManagementWebAPIServices(builder.Configuration);

var app = builder.Build();

await app.AddContactManagementWebAPIMiddlewares();

app.Run();