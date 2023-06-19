using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using ToDoAppPremium.Models;
using ToDoBackend;

namespace ToDoAppPremium.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ToDoService toDoService;

    private static Random random = new Random();

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
        this._logger.LogInformation("Start GetToDosAsync");
        try
        {
            var day = random.Next(1, 15);
            string fromDate = $"06/{day}/2023";
            var response = await this.toDoService.GetToDosAsync(fromDate);
            this._logger.LogDebug("End GetToDosAsync");
            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError("An error occur when trying to get todos !");
            SentrySdk.CaptureException(ex);
            return Enumerable.Empty<ToDo>();
        }
    }
}
