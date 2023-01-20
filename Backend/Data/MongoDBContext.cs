using ApiToDatabase.Models;
using ApiToDatabase.Models.RequestModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations.ElementNameValidators;
using System;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;

namespace ApiToDatabase.Data;

public class MongoDbContext
{
    private readonly IMongoCollection<User> _context;

    public MongoDbContext(IOptions<DatabaseSettings> userDatabaseSettings)
    {
        var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient
        .GetDatabase(userDatabaseSettings.Value.DatabaseName);

        _context = mongoDatabase
        .GetCollection<User>(userDatabaseSettings.Value.UsersCollectionName);
    }
}