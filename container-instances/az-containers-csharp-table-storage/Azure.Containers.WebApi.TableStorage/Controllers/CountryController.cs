using Azure.Containers.WebApi.TableStorage.Dto;
using Azure.Containers.WebApi.TableStorage.Entities;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Azure.Containers.WebApi.TableStorage.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private const string TableName = "Countries";
    private readonly TableClient _tableClient;

    public CountryController(IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("The Azure Storage connection string is not configured in the environment variables.");
        }

        var serviceClient = new TableServiceClient(connectionString);
        
        _tableClient = serviceClient.GetTableClient(TableName);
        _tableClient.CreateIfNotExists();
    }

    // GET: api/City
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        var countries = new List<Country>();

        await foreach (var country in _tableClient.QueryAsync<Country>())
        {
            countries.Add(country);
        }

        return Ok(countries);
    }

    // POST: api/City
    [HttpPost]
    public async Task<ActionResult> AddCity(CountryDto countryDto)
    {
        var country = new Country
        {
            PartitionKey = "default",
            RowKey = Guid.NewGuid().ToString(),
            Name = countryDto.Name,
            Capital = countryDto.Capital,
            Image = countryDto.Image
        };

        try
        {
            await _tableClient.AddEntityAsync(country);
            return CreatedAtAction(nameof(GetCity), new { partitionKey = country.PartitionKey, rowKey = country.RowKey }, country);
        }
        catch (RequestFailedException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/City/{partitionKey}/{rowKey}
    [HttpGet("{partitionKey}/{rowKey}")]
    public async Task<ActionResult<Country>> GetCity(string partitionKey, string rowKey)
    {
        try
        {
            var city = await _tableClient.GetEntityAsync<Country>(partitionKey, rowKey);
            return Ok(city);
        }
        catch (RequestFailedException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT: api/City/{partitionKey}/{rowKey}
    [HttpPut("{partitionKey}/{rowKey}")]
    public async Task<ActionResult> UpdateCity(string partitionKey, string rowKey, CountryDto country)
    {
        try
        {
            var existingCity = await _tableClient.GetEntityAsync<Country>(partitionKey, rowKey);
            existingCity.Value.Name = country.Name;
            existingCity.Value.Capital = country.Capital;
            existingCity.Value.Image = country.Image;

            await _tableClient.UpdateEntityAsync(existingCity.Value, existingCity.Value.ETag, TableUpdateMode.Replace);
            return NoContent();
        }
        catch (RequestFailedException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

