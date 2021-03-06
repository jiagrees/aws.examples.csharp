﻿using System.Threading.Tasks;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using SqsReader.Dynamo;

namespace SqsReader.Services.Processors
{
    public class ActorMessageProcessor : IMessageProcessor
    {
        private readonly IActorsRepository _actorsRepository;
        private readonly ILogger<ActorMessageProcessor> _logger;

        public ActorMessageProcessor(IActorsRepository actorsRepository, ILogger<ActorMessageProcessor> logger)
        {
            _actorsRepository = actorsRepository;
            _logger = logger;
        }

        public bool CanProcess(string messageType)
        {
            return messageType == typeof(Actor).Name;
        }

        public async Task ProcessAsync(Message message)
        {
            var actor = JsonConvert.DeserializeObject<Actor>(message.Body);
            await _actorsRepository.SaveActorAsync(actor);
            _logger.LogInformation("ActorMessageProcessor invoked with {FirstName} and {LastName}, {@Content}",
                actor.FirstName, actor.LastName, actor);
        }
    }
}