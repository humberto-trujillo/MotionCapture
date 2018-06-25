using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensorLobby : Singleton<SensorLobby>
{
	[SerializeField] TextMeshProUGUI titleText;
	[SerializeField] Button targetStartButton;
	[SerializeField] Button planeStartButton;
    [SerializeField] ConnectedSensorControlUnit connectedControlUnit;
    [SerializeField] ConnectedSensorControlUnit assignedControlUnit;
    [SerializeField] ConnectedSensor connectedSensorPrefab;
    [SerializeField] CameraFocus cameraFocus;
	[SerializeField] GameObject lobbyAvatar;
	[SerializeField] GameObject avatar;
	[SerializeField] TextMeshProUGUI countdownText;
	[SerializeField] MotionCaptureManager mocap;

	[SerializeField] Transform imageTarget;
	[SerializeField] Transform groundPlane;
	public int countdown = 5;


	private IMU_UdpCommunication udpCommunication;
	public List<ConnectedSensor> assignedSensors = new List<ConnectedSensor>();
	private bool newConnection = false;
	private UdpConnectedIMU newConnectedIMU;

	private void Start()
	{
		targetStartButton.interactable = false;
		planeStartButton.interactable = false;

		countdownText.gameObject.SetActive(false);
		udpCommunication = IMU_UdpCommunication.Instance;
		udpCommunication.OnClientConnected += OnConnectedSensor;

		assignedControlUnit.OnSensorDropped += OnSensorAssigned;
		connectedControlUnit.OnSensorDropped += OnSensorTakeBack;
	}

	private void Update() 
	{
		if(newConnection)
		{
			ConnectedSensor sensor = connectedControlUnit.AddItemInFreeCell(connectedSensorPrefab);
			sensor.Init(newConnectedIMU);
			newConnection = false;
		}	
	}

	private void OnConnectedSensor(UdpConnectedIMU connection)
	{
		newConnection = true;
		newConnectedIMU = connection;
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

	public UdpConnectedIMU GetConnectionOfBodyPartType(BoneType boneType)
	{
		ConnectedSensor sensor = assignedSensors.Find(x => x.boneType == boneType);
		return (sensor != null)? sensor.m_imuConnection : null;
	}

    private void ValidateSetup()
	{
		targetStartButton.interactable = assignedSensors.Count > 0;
		planeStartButton.interactable = assignedSensors.Count > 0;
	}

	public void LoadARLogic(bool isTarget = true)
	{
		cameraFocus.Reset();
		connectedControlUnit.gameObject.SetActive(false);
		assignedControlUnit.gameObject.SetActive(false);
		targetStartButton.gameObject.SetActive(false);
		planeStartButton.gameObject.SetActive(false);
		titleText.text = "Colócate en esta pose";
		StartCoroutine(CountdownRoutine(isTarget));
	}

	private IEnumerator CountdownRoutine(bool isTarget)
	{
		int count = countdown;
		countdownText.gameObject.SetActive(true);
		GraphicsMover mover = countdownText.GetComponent<GraphicsMover>();
		while (count >= 0)
		{
			countdownText.text = (count--).ToString();
			mover.ScaleGraphic();
			yield return new WaitForSeconds(1.0f);
			mover.ResetTransform();
		}
		countdownText.gameObject.SetActive(false);
		titleText.text = "";
		lobbyAvatar.SetActive(false);

		avatar.transform.SetParent(isTarget? imageTarget : groundPlane);
		mocap.StartMotionCaptureManager();
	}
}
