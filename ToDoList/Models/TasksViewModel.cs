namespace ToDoList.Models
{
    public class TasksViewModel
    {
        public List<Tasks>? InProgressTasks { get; set; }
        public List<Tasks>? ToDoTasks { get; set; }
        public List<Tasks>? CompletedTasks { get; set; }

        public Enums.Status SelectedStatus { get; set; }
    }
}
