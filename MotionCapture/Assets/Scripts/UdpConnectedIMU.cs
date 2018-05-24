[System.Serializable]
public class UdpConnectedIMU
{
	public string ipAddress;
	string latestMessage;
	public string LatestMessage {get { return latestMessage;} set{ latestMessage = value;}}

	public UdpConnectedIMU(string ipAddress)
	{
		this.ipAddress = ipAddress;
	}
}