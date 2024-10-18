using Azure;
using Azure.Data.Tables;
using System;
using System.Text.Json.Serialization;

namespace Azure.Containers.WebApi.TableStorage.Entities
{

    public class City : ITableEntity
    {
        // Required Propertites
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Custom properties
        public string Name { get; set; }
        public string Capital { get; set; }
        public string Image { get; set; }

        public City() { }
        public City(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

    }
}

