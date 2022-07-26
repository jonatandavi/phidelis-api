using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using phidelis_api;
using phidelis_api.Controllers;
using phidelis_api.Models;
using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public class TimedHostedService : IHostedService
{
    private int executionCount = 0;
    private Timer? _timer = null;
    string Baseurl = "https://gerador-nomes.herokuapp.com/";
    private AppDbContext _context;
    public IConfiguration Configuration { get; }

    public TimedHostedService()
    {
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(GlobalData.Timer*60));

        return Task.CompletedTask;
    }

    public async void GenerateRegistrarion()
    {
        Console.WriteLine(GlobalData.Timer);
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(Baseurl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage Res = await client.GetAsync("nomes/5");

            if (Res.IsSuccessStatusCode)
            {
                foreach(var name in JsonConvert.DeserializeObject<string[]>(Res.Content.ReadAsStringAsync().Result))
                {
                    Utils utils = new Utils();
                    SqliteConnection sqliteConnection = new SqliteConnection("Data Source=Phidelis.db");
                    sqliteConnection.Open();
                    string cmd = @"INSERT INTO Students(Name, DocNumber, DateTimeRegistration)
                                    VALUES('" + name + "', '" + utils.GenerateCpf() + "', '" + DateTime.Now.ToString() + "')" ;
                    SqliteCommand sQLiteCommand = new SqliteCommand(cmd, sqliteConnection);
                    sQLiteCommand.ExecuteScalar();
                    sqliteConnection.Close();              
                }
            }
        }
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);
        Console.WriteLine(
            "Timed Hosted Service is working. Count: {0}", count);

        GenerateRegistrarion();
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

}