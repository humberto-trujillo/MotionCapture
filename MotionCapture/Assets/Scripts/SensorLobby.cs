using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorLobby : MonoBehaviour
{
	[SerializeField] Button startButton;
    [SerializeField] ConnectedSensorControlUnit connectedControlUnit;
    [SerializeField] ConnectedSensorControlUnit assignedControlUnit;
    [SerializeField] ConnectedSensor connectedSensorPrefab;
    [SerializeField] CameraFocus cameraFocus;


	private IMU_UdpCommunication udpCommunication;
	private List<ConnectedSensor> assignedSensors = new List<ConnectedSensor>();

	private void Start()
	{
		startButton.interactable = false;
		udpCommunication = IMU_UdpCommunication.Instance;
		udpCommunication.OnClientConnected += OnConnectedSensor;

		assignedControlUnit.OnSensorDropped += OnSensorAssigned;
		connectedControlUnit.OnSensorDropped += OnSensorTakeBack;
	}

	private void OnConnectedSensor(UdpConnectedIMU connection)
	{
		ConnectedSensor sensor = connectedControlUnit.AddItemInFreeCell(connectedSensorPrefab);
		sensor.Init(connection);
	}
    
	private void OnSensorTakeBack(ConnectedSensor sensorItem)
	{
		if(assignedSensors.Contains(sensorItem))
		{
			assignedSensors.Remove(sensorItem);
		}
		cameraFocus.Reset();
		ValidateSetup();
	}

	private void OnSensorAssigned(ConnectedSensor sensorItem)
	{
		if(!assignedSensors.Contains(sensorItem))
		{
			assignedSensors.Add(sensorItem);
        }
		cameraFocus.FocusTo(sensorItem.boneType);
		ValidateSetup();
	}

    private void ValidateSetup()
	{
		startButton.interactable = assignedSensors.Count > 0;
	}
}
