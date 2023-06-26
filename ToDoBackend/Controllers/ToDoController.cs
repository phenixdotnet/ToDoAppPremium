using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ToDoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoController : ControllerBase
{
    private static ActivitySource source = new ActivitySource(TelemetryConstants.ServiceName);
    private readonly ILogger<ToDoController> _logger;

    public ToDoController(ILogger<ToDoController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetToDo")]
    public async Task<IEnumerable<ToDo>> GetAsync(string fromDate)
    {
        var correlationId = this.Request.Headers["X-CorrelationId"].ToString();

        using (var activity = source.StartActivity("ToDo.Get"))
        {
            DateTime fd;
            try
            {
                fd = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
            }
            catch (Exception ex)
            {
                this._logger.LogError("{correlationId}|Unable to parse the date '{fromDate}'", correlationId, fromDate);
                throw new InvalidDataException("fromDate", $"Unable to parse the date '{fromDate}' with pattern 'dd/MM/yyyy'", correlationId);
            }

            this._logger.LogInformation("{correlationId}|Loading all todo since {fromDate}", correlationId, fd);
            var results = await this.QueryDBAsync(fd, correlationId).ConfigureAwait(false);

            this._logger.LogInformation("{correlationId}|Returning {countToDo} ToDo", correlationId, results.Count());
            return results;
        }
    }

    private async Task<IEnumerable<ToDo>> QueryDBAsync(DateTime fromDate, string correlationId)
    {
        var results = Enumerable.Range(1, 5).Select(index => new ToDo
        {
            Title = "Task " + index
        })
            .ToArray();


        var delta = (DateTime.Now - fromDate);
        await Task.Delay((int)delta.TotalDays * 500);

        return results;
    }
}

