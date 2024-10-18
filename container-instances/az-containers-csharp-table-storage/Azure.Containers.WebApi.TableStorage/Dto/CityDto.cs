using Azure;
using Azure.Data.Tables;
using System;
using System.Text.Json.Serialization;

namespace Azure.Containers.WebApi.TableStorage.Dto;

public class CityDto
{
    public string Name { get; set; }
    public string Capital { get; set; }
    public string Image { get; set; }
}
