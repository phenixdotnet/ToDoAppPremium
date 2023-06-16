using Microsoft.AspNetCore.Mvc;

namespace ToDoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoController : ControllerBase
{
    private readonly ILogger<ToDoController> _logger;

    public ToDoController(ILogger<ToDoController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetToDo")]
    public IEnumerable<ToDo> Get()
    {
        this._logger.LogInformation("Start ToDoController.Get");
        var results = Enumerable.Range(1, 5).Select(index => new ToDo
        {
            Title = "Task " + index
        })
        .ToArray();

        this._logger.LogInformation("End ToDoController.Get");
        return results;
    }
}

