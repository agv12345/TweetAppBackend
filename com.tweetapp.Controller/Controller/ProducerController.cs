using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DotNet.MSIdentity.Shared;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TweetApp.Controller.Controller
{
    [Route("api/v1.0/tweets")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string
            bootstrapServers = "localhost:9092";

        private readonly string topic = "TweetApp";

        // DELETE: api/API/5
        [HttpDelete("/kafka/delete{id}")]
        private async Task<bool> DeleteTweet(string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder
                           <Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync
                    (topic, new Message<Null, string>
                    {
                        Value = message
                    });

                    Debug.WriteLine($"Deleting:{result.Timestamp.UtcDateTime}");
                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}