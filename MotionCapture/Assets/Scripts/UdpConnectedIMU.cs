using UnityEngine;

[System.Serializable]
public struct IMUQuaternion
{
	public float w;
	public float x;
	public float y;
	public float z;
}

[System.Serializable]
public struct IMUData
{
	public string programState;
	public IMUQuaternion quaternion;
}

[System.Serializable]
public class UdpConnectedIMU
{
	public string ipAddress;
	string latestMessage;
	public string LatestMessage {get { return latestMessage;} set{ latestMessage = value;}}

	public IMUData imuData;
	public bool standBy = true;

	public UdpConnectedIMU(string ipAddress)
	{
		this.ipAddress = ipAddress;
	}

	public void TelemetryUpdate(string jsonString)
	{
		imuData = JsonUtility.FromJson<IMUData>(jsonString);
		standBy = (imuData.programState == "STANDBY");
	}

	public Quaternion GetSensorQuaternion()
	{
		IMUQuaternion q = imuData.quaternion;
		return new Quaternion(q.x, q.y, q.z, q.w);
	}
}