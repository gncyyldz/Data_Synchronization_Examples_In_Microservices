using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServiceB.Models.Entities;
using ServiceB.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDBService>();

#region MongoDB'ye Seed Data Ekleme
using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
MongoDBService mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var collection = mongoDBService.GetCollection<Employee>();
if (!collection.FindSync(s => true).Any())
{
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e5", Name = "Gençay", Department = "Yazýlým" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e6", Name = "Hilmi", Department = "Aðýr vasýta" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e7", Name = "Þuayip", Department = "Oluk&Çatý" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e8", Name = "Rakýf", Department = "Muhabbet Sohbet" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09e9", Name = "Rýfký", Department = "Þoför" });
    await collection.InsertOneAsync(new() { PersonId = "650240a832521eccf69b09ea", Name = "Muiddin", Department = "Muhasebe" });
}
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("update/{personId}/{newName}", async (
    [FromRoute] string personId,
    [FromRoute] string newName,
    MongoDBService mongoDBService) =>
{
    var employees = mongoDBService.GetCollection<Employee>();
    Employee employee = await (await employees.FindAsync(e => e.PersonId == personId)).FirstOrDefaultAsync();
    employee.Name = newName;
    await employees.FindOneAndReplaceAsync(p => p.Id == employee.Id, employee);
    return true;
});

app.Run();
