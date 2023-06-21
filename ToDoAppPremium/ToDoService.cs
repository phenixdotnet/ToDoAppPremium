using System;
using System.Diagnostics;
using System.Text.Json;
using ToDoBackend;

namespace ToDoAppPremium
{
	public class ToDoService
	{
		const string BaseUrl = "https://localhost:7141";

        private static ActivitySource source = new ActivitySource(TelemetryConstants.ServiceName);
		private readonly ILogger<ToDoService> _logger;

        public ToDoService(ILogger<ToDoService> logger)
		{
			this._logger = logger;
		}

		public async Task<IEnumerable<ToDo>> GetToDosAsync(string fromDate, string correlationId)
		{
            this._logger.LogInformation("{0}|Start GetToDosAsync", correlationId);
			IEnumerable<ToDo>? results = null;

            using (var activity = source.StartActivity("ToDoService.GetToDo"))
			{
				using(HttpClient client = new HttpClient())
				{
					using (var response = await client.GetAsync(BaseUrl + "/todo?fromDate=" + fromDate).ConfigureAwait(false))
					{
						response?.EnsureSuccessStatusCode();
                        results = await response.Content.ReadFromJsonAsync<IEnumerable<ToDo>>().ConfigureAwait(false);
                        if (results == null)
                            results = Enumerable.Empty<ToDo>();
                    }
				}
            }

            this._logger.LogInformation("{0}|End GetToDosAsync", correlationId);
			return results;
        }
	}
}

