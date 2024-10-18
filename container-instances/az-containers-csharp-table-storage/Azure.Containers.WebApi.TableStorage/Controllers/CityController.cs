using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Containers.WebApi.TableStorage.Entities;
using Azure.Containers.WebApi.TableStorage.Dto;

namespace Azure.Containers.WebApi.TableStorage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly string _tableName = "Cities";
    private readonly string _connectionString;
    private readonly TableClient _tableClient;
    private readonly TableServiceClient _serviceClient;
    
    public CityController(IConfiguration configuration)
    {
        
        _connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("The Azure Storage connection string is not configured in the environment variables.");
        }

        _serviceClient = new TableServiceClient(_connectionString);
        _tableClient = _serviceClient.GetTableClient(_tableName);
        _tableClient.CreateIfNotExists();
    }

    // GET: api/City
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        var cities = new List<City>();

        await foreach (var city in _tableClient.QueryAsync<City>())
        {
            cities.Add(city);
        }

        return Ok(cities);
    }

    // POST: api/City
    [HttpPost]
    public async Task<ActionResult> AddCity(CityDto cityDto)
    {
        var city = new City
        {
            PartitionKey = "default",
            RowKey = Guid.NewGuid().ToString(),
            Name = cityDto.Name,
            Capital = cityDto.Capital,
            Image = cityDto.Image
        };

        try
        {
            await _tableClient.AddEntityAsync(city);
            return CreatedAtAction(nameof(GetCity), new { partitionKey = city.PartitionKey, rowKey = city.RowKey }, city);
        }
        catch (RequestFailedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/City/{partitionKey}/{rowKey}
    [HttpGet("{partitionKey}/{rowKey}")]
    public async Task<ActionResult<City>> GetCity(string partitionKey, string rowKey)
    {
        try
        {
            var city = await _tableClient.GetEntityAsync<City>(partitionKey, rowKey);
            return Ok(city);
        }
        catch (RequestFailedException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT: api/City/{partitionKey}/{rowKey}
    [HttpPut("{partitionKey}/{rowKey}")]
    public async Task<ActionResult> UpdateCity(string partitionKey, string rowKey, CityDto city)
    {
        try
        {
            var existingCity = await _tableClient.GetEntityAsync<City>(partitionKey, rowKey);
            existingCity.Value.Name = city.Name;
            existingCity.Value.Capital = city.Capital;
            existingCity.Value.Image = city.Image;

            await _tableClient.UpdateEntityAsync(existingCity.Value, existingCity.Value.ETag, TableUpdateMode.Replace);
            return NoContent();
        }
        catch (RequestFailedException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

