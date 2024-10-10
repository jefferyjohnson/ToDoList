using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ToDoList.Models;

namespace ToDoList
{
    public class TasksRepository
    {
        private readonly IMongoCollection<Tasks> _TasksCollection;

        public TasksRepository(IOptions<MongoDBSettings> settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _TasksCollection = database.GetCollection<Tasks>("Tasks");
        }

        public async Task<List<Tasks>> GetAllAsync()
        {
            return await _TasksCollection.Find(task => true).ToListAsync();
        }

        public async Task<Tasks> GetByIdAsync(string id)
        {
            return await _TasksCollection.Find<Tasks>(task => task.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Tasks task)
        {
            await _TasksCollection.InsertOneAsync(task);
        }

        public async Task UpdateAsync(string id, Tasks updatedTask)
        {
            await _TasksCollection.ReplaceOneAsync(task => task.Id == id, updatedTask);
        }

        public async Task DeleteAsync(string id)
        {
            await _TasksCollection.DeleteOneAsync(task => task.Id == id);
        }

    }
}
