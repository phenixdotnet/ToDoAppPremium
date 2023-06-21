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
        string correlationId = Guid.NewGuid().ToString();
        this.ViewData["ToDos"] = await this.GetToDosAsync(correlationId);
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

    private async Task<IEnumerable<ToDo>> GetToDosAsync(string correlationId)
    {
        this._logger.LogInformation("{0}|Start GetToDosAsync", correlationId);
        try
        {
            var day = random.Next(1, 15);
            string fromDate = $"06/{day}/2023";

            var response = await this.toDoService.GetToDosAsync(fromDate, correlationId);
            this._logger.LogInformation("{0}|End GetToDosAsync", correlationId);
            return response;
        }
        catch (Exception ex)
        {
            this._logger.LogError("{0}|An error occur when trying to get todos !", correlationId);
            SentrySdk.CaptureException(ex);
            return Enumerable.Empty<ToDo>();
        }
    }
}
