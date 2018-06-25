using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BoneType
{
	Head,
	Torso,
	RightForearm,
	RightShoulder,
	LeftForearm,
	LeftShoulder,
	RightUpperLeg,
	RightLowerLeg,
	LeftUpperLeg,
	LeftLowerLeg,
    None
}

public class MotionCaptureManager : MonoBehaviour
{
    [System.Serializable]
    public struct BodyPart
    {
        public GameObject bodyPart;
		public BoneType type;
		[HideInInspector]
		public IMU_Orientation orientation;
    }
	IMU_UdpCommunication udpCommunication;
	SensorLobby	sensorLobby;
	public BodyPart[] bodyParts;

	[SerializeField] RecordTransformHierarchy avatar;
	[SerializeField] Button recordButton;
	[SerializeField] Button playButton;
	public bool isRecording = false;
	public bool isPlaying = false;

    void Start ()
    {
		udpCommunication = IMU_UdpCommunication.Instance;
		sensorLobby = SensorLobby.Instance;

		// udpCommunication.OnClientConnected += InitOrientationComponent;
	}

	public void StartMotionCaptureManager()
	{
		for(int i=0; i<bodyParts.Length;i++)
        {
			bodyParts[i].orientation = bodyParts[i].bodyPart.AddComponent<IMU_Orientation>();
			bodyParts[i].orientation.boneType = bodyParts[i].type;

			UdpConnectedIMU connection = sensorLobby.GetConnectionOfBodyPartType(bodyParts[i].type);
			if(connection != null)
			{
				bodyParts[i].orientation.Init(connection);
			}
			else
			{
				Debug.LogWarning("FAILED TO INITIALIZED BODY PART, sensor might not be assigned");
			}
        }
		//playButton.interactable = false;
		StartMotionCapture();
	}

	// void InitOrientationComponent (UdpConnectedIMU connection)
	// {
	// 	IMU_Orientation orientation = GetNotInitializedBodyPart();
	// 	orientation.Init(connection);
	// }

	// IMU_Orientation GetNotInitializedBodyPart()
	// {
	// 	IMU_Orientation component = null;
	// 	for (int i = 0; i < bodyParts.Length; i++) 
	// 	{
	// 		component = bodyParts[i].orientation;
	// 		if (!component.IsInitialized) 
	// 		{
	// 			return component;
	// 		}
	// 	}
	// 	return null;
	// }

	public void Record()
	{
		if(!isRecording)
		{
			avatar.record = true;
			isRecording = true;
			playButton.interactable = false;
			recordButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parar";
		}
		else
		{
			avatar.record = false;
			isRecording = false;
			playButton.interactable = true;
			recordButton.GetComponentInChildren<TextMeshProUGUI>().text = "Capturar";
		}
	}

	public void Playback()
	{
		if(!isPlaying)
		{
			avatar.GetComponent<Animator>().enabled = true;
			recordButton.interactable = false;
			isPlaying = true;
			playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Parar";
		}
		else
		{
			avatar.GetComponent<Animator>().enabled = false;
			recordButton.interactable = true;
			isPlaying = false;
			playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reproducir";
		}
	}

	public void StartMotionCapture()
	{
		Debug.Log("Starting Motion Capture!");
		udpCommunication.SendToAll("START");
	}
	
	public void StopMotionCapture()
	{
		Debug.Log("Stoping Motion Capture!");
		udpCommunication.SendToAll("STANDBY");
	}
}
