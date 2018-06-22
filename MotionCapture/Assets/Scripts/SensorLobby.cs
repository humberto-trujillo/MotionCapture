using UnityEngine;
using UnityEngine.UI;

public class SensorLobby : MonoBehaviour
{
	public Button startButton;
	public bool isConfigured = false;
	public ConnectedSensorControlUnit connectedControlUnit;
	public ConnectedSensorControlUnit assignedControlUnit;
	public DragAndDropItem sensorItemPrefab;
	public CameraFocus cameraFocus;

	private IMU_UdpCommunication udpCommunication;

	private void Start()
	{
		startButton.interactable = false;
		udpCommunication = IMU_UdpCommunication.Instance;
		udpCommunication.OnClientConnected += OnConnectedSensor;
		assignedControlUnit.OnSensorDropped += OnSensorDropped;
		connectedControlUnit.OnSensorDropped += OnSensorDropped;
	}

	private void OnConnectedSensor(UdpConnectedIMU connection)
	{
		connectedControlUnit.AddItemInFreeCell(sensorItemPrefab);
	}
    
	private void OnSensorDropped(BoneType boneType)
	{
		if(boneType != BoneType.None)
		{
		    cameraFocus.FocusTo(boneType);
        }
		else
		{
			cameraFocus.Reset();
		}
	}
}
