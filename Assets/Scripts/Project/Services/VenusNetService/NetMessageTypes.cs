namespace Project.Services.VenusNetService
{
	public enum NetMessageTypes
	{
		NotDefined = 0,
		Heartbeat = 1,
		HeartbeatAck = 2,
		ServerWelcome = 3,
		ClientWelcome = 4,
		WorldSnapshot = 5,
		ClientId = 6,
		WorldDelta = 7,
		TimeSync = 8,
		RequestServerSnapshot = 9,
	}
}


