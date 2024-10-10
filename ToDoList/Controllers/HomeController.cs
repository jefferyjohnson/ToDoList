using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using ToDoList.Models;
using static ToDoList.Models.Enums;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly TasksRepository _taskRepository;

        public HomeController(TasksRepository tasksRepository)
        {
            _taskRepository = tasksRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskRepository.GetAllAsync();
            var inProgressTasks = tasks.Where(t => t.Status == Status.Pending).ToList();
            var toDoTasks = tasks.Where(t => t.Status == Status.ToDo).ToList();
            var completedTasks = tasks.Where(t => t.Status == Status.Completed).ToList();
            var viewModel = new TasksViewModel
            {
                InProgressTasks = inProgressTasks,
                ToDoTasks = toDoTasks,
                CompletedTasks = completedTasks
            };
            ViewBag.StatusOptions = Enum.GetValues(typeof(Enums.Status))
                .Cast<Enums.Status>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                })
                .ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var model = new Tasks
            {
                Id = task.Id,
                Content = task.Content,
                Status = task.Status
            };
            return View(task);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tasks task)
        {
            if (ModelState.IsValid)
            {
                await _taskRepository.CreateAsync(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(string id, Tasks task)
        {
            task = await _taskRepository.GetByIdAsync(id);
            if (id != task.Id)
            {
                return BadRequest();
            }

            if(task.Status == Enums.Status.ToDo)
            {
                task.Status = Enums.Status.Pending;
            }
            else if(task.Status == Enums.Status.Pending)
            {
                task.Status = Enums.Status.Completed;
            }
            else
            {
                task.Status = task.Status;
            }


            if (ModelState.IsValid)
            {
                await _taskRepository.UpdateAsync(id, task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _taskRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
