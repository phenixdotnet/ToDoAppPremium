﻿using System.Diagnostics;
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
        string? correlationId = this.Request.Headers["correlationId"];

        using (var activity = source.StartActivity("ToDo.Get"))
        {
            this._logger.LogInformation("{0}|Start ToDoController.Get", correlationId);
            try
            {
                var fd = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
            }
            catch(Exception ex)
            {
                this._logger.LogError("{correlationId}|Unable to parse the date", correlationId);
                throw new ArgumentOutOfRangeException("fromDate");
            }

            var results = Enumerable.Range(1, 5).Select(index => new ToDo
            {
                Title = "Task " + index
            })
            .ToArray();

            this._logger.LogInformation("{0}|End ToDoController.Get", correlationId);
            return results;
        }
    }
}

