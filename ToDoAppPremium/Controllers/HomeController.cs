using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoAppPremium.Models;
using ToDoBackend;

namespace ToDoAppPremium.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ToDoService toDoService;

    public HomeController(ToDoService toDoService, ILogger<HomeController> logger)
    {
        _logger = logger;
        this.toDoService = toDoService;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Index()
    {
        this.ViewData["ToDos"] = await this.GetToDosAsync();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task<IEnumerable<ToDo>> GetToDosAsync()
    {
        try
        {
            return await this.toDoService.GetToDosAsync();
        }
        catch (Exception)
        {
            this._logger.LogError("An error occur when trying to get todos !");
            return Enumerable.Empty<ToDo>();
        }
    }
}

