using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;

namespace WeatherService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapGet("/weather", async context =>
                {
                    await context.Response.WriteAsync("Getting Temperature: ");


                   string connectionString = "Endpoint=sb://coding-challenge-md.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=O3t9ulfLdgntnvBvpPDoOLdhGWp9HUzCTRBFsooSCvs=";
                   string eventHubName = "event-hub-01";
                    await using (var producerClient = new EventHubProducerClient(connectionString, eventHubName))
                    {
                        // Create a batch of events 
                        using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                        // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                        eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(WeatherService.getWeather(8.8078f, 53.0752f, 'C'))));
                        // Use the producer client to send the batch of events to the event hub
                        await producerClient.SendAsync(eventBatch);
                    }
                    //await context.Response.WriteAsync("C: "+ WeatherService.getWeather(8.8078f,53.0752f,'C'));
                    //await context.Response.WriteAsync("F: " + WeatherService.getWeather("bremen", 'F'));
                });
            });
        }
    }
}
