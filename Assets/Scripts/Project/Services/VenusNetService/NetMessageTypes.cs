namespace Project.Services.VenusNetService
{
	public enum NetMessageTypes
	{
		Heartbeat = 1,
		HeartbeatAck = 2,
		ServerWelcome = 3,
		ClientWelcome = 4,
		WorldSnapshot = 5,
		WorldDelta = 6,
		TimeSync = 7,
		RequestServerSnapshot = 8,
	}
}


