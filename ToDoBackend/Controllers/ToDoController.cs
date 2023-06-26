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
    public IEnumerable<ToDo> Get(string fromDate)
    {
        var correlationId = this.Request.Headers["X-CorrelationId"].ToString();

        using (var activity = source.StartActivity("ToDo.Get"))
        {
            try
            {
                var fd = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
            }
            catch (Exception ex)
            {
                this._logger.LogError("{correlationId}|Unable to parse the date '{fromDate}'", correlationId, fromDate);

                //throw new InvalidDataException("fromDate");
                #region better
                throw new InvalidDataException("fromDate", $"Unable to parse the date '{fromDate}' with pattern 'dd/MM/yyyy'", correlationId);
                #endregion
            }

            var results = Enumerable.Range(1, 5).Select(index => new ToDo
            {
                Title = "Task " + index
            })
            .ToArray();

            this._logger.LogInformation("{correlationId}|Returning {countToDo} ToDo", correlationId, results.Length);
            return results;
        }
    }
}

