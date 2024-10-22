using Azure.Data.Tables;

namespace Azure.Containers.WebApi.TableStorage.Entities
{

    public class Country: ITableEntity
    {
        // Required Properties
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Custom properties
        public string Name { get; set; }
        public string Capital { get; set; }
        public string Image { get; set; }

        public Country() { }
        
    }
}

