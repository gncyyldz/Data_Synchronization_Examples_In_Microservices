using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServiceB.Consumers;
using ServiceB.Models.Entities;
using ServiceB.Services;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<UpdatePersonNameEventConsumer>();

    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMQ"]);

        _configurator.ReceiveEndpoint(RabbitMQSettings.ServiceB_UpdatePersonNameEventQueue, e => e.ConfigureConsumer<UpdatePersonNameEventConsumer>(context));
    });
});

#region MongoDB'ye Seed Data Ekleme
using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var collection = mongoDBService.GetCollection<Employee>();
if (!collection.FindSync(s => true).Any())
{
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e5", Name = "Gen�ay", Department = "Yaz�l�m" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e6", Name = "Hilmi", Department = "A��r vas�ta" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e7", Name = "�uayip", Department = "Oluk&�at�" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e8", Name = "Rak�f", Department = "Muhabbet Sohbet" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e9", Name = "R�fk�", Department = "�of�r" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09ea", Name = "Muiddin", Department = "Muhasebe" });
}
#endregion

var app = builder.Build();

app.Run();
