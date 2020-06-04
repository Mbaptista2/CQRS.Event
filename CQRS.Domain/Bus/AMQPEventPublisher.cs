using CQRS.Domain.Events;
using CQRS.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Domain.Bus
{
	public class AMQPEventPublisher : IEventPublisher
	{
		private IConfiguration _configuration;
		private readonly ConnectionFactory connectionFactory;

		public AMQPEventPublisher(IHostingEnvironment env, AMQPEventSubscriber aMQPEventSubscriber, IConfiguration configuration)
		{
			connectionFactory = new ConnectionFactory();
			_configuration = configuration;
			

			var username = configuration["amqp:username"];
			var password = configuration["amqp:password"];
			var hostname = configuration["amqp:hostname"];
			var uri = configuration["amqp:uri"];
			var virtualhost = configuration["amqp:virtualhost"];

			connectionFactory = new ConnectionFactory()
			{
				UserName = username,
				Password = password,
				HostName = hostname,
				Uri = new Uri(uri),
				VirtualHost = virtualhost
			};
		}

		public void Publish<T>(T @event) where T : IEvent
		{
			using (IConnection conn = connectionFactory.CreateConnection())
			{
				using (IModel channel = conn.CreateModel())
				{
					var queue = @event is ClienteCriadoEvent ?
						Constants.QUEUE_CUSTOMER_CREATED : @event is ClienteAtualizadoEvent ?
							Constants.QUEUE_CUSTOMER_UPDATED : Constants.QUEUE_CUSTOMER_DELETED;

					channel.QueueDeclare(
						queue: queue,
						durable: false,
						exclusive: false,
						autoDelete: false,
						arguments: null
					);

					var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

					channel.BasicPublish(
						exchange: "",
						routingKey: queue,
						basicProperties: null,
						body: body
					);
				}
			}
		}
	}
}
