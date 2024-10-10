using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using static ToDoList.Models.Enums;

namespace ToDoList.Models
{
    public class Tasks
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Content { get; set; }
        public Status? Status { get; set; }
    }
}
