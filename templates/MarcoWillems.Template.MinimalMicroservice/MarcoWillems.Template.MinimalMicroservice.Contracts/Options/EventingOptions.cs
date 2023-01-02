using System;
namespace MarcoWillems.Template.MinimalMicroservice.Contracts.Options;

public class EventingOptions
{
	public string RabbitMQHost { get; set; } = string.Empty;
	public string RabbitMQUsername { get; set; } = string.Empty;
	public string RabbitMQPassword { get; set; } = string.Empty;
}

