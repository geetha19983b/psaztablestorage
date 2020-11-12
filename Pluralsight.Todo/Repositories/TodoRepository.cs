using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pluralsight.Todo.Repositories
{
    public class TodoRepository
    {
        private CloudTable todoTable = null;
        public TodoRepository()
        {
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=fekbergpluralsight-cosmos;AccountKey=O9mv1To3pgy23se1LKht8rFRGivCVAiAG5ThiCHPqCR0ZypnT2qVOGavEKPI6212iAoC8DbIF746SU5sFKxOLA==;TableEndpoint=https://fekbergpluralsight-cosmos.table.cosmos.azure.com:443/;");


            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=canyonsa;AccountKey=tLbcOWtMmIZ07iUPa5JSZ8wWjhzJAdloDLvH0NkOMnCmLoHNRMdsRhIF9ngpWwPtD+MXu+sDtCFHy6oz1SnQiw==;EndpointSuffix=core.windows.net");

            var tableClient = storageAccount.CreateCloudTableClient();

            todoTable = tableClient.GetTableReference("canyontodo");
        }

        public IEnumerable<TodoEntity> All()
        {
            var query = new TableQuery<TodoEntity>()
                .Where(TableQuery.GenerateFilterConditionForBool(nameof(TodoEntity.Completed),
                QueryComparisons.Equal,
                false));

            var entities = todoTable.ExecuteQuery(query);

            return entities;
        }

        public void CreateOrUpdate(TodoEntity entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);

            todoTable.Execute(operation);
        }

        public void Delete(TodoEntity entity)
        {
            var operation = TableOperation.Delete(entity);

            todoTable.Execute(operation);
        }

        public TodoEntity Get(string partitionKey, string rowKey)
        {
            var operation = TableOperation.Retrieve<TodoEntity>(partitionKey, rowKey);

            var result = todoTable.Execute(operation);

            return result.Result as TodoEntity;
        }
    }

    public class TodoEntity : TableEntity
    {
        public string Content { get; set; }
        public bool Completed { get; set; }
        public string Due { get; set; }
    }
}