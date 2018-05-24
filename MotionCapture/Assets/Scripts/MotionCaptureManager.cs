using System.Collections.Generic;
using UnityEngine;

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
	LeftLowerLeg
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
	public BodyPart[] bodyParts;

    void Start ()
    {
		udpCommunication = IMU_UdpCommunication.Instance;
		udpCommunication.OnClientConnected += InitOrientationComponent;
		for(int i=0; i<bodyParts.Length;i++)
        {
			bodyParts[i].orientation = bodyParts[i].bodyPart.AddComponent<IMU_Orientation>();
			bodyParts[i].orientation.boneType = bodyParts[i].type;
        }
	}

	void InitOrientationComponent (UdpConnectedIMU connection)
	{
		IMU_Orientation orientation = GetNotInitializedBodyPart();
		orientation.Init(connection);
	}

	IMU_Orientation GetNotInitializedBodyPart()
	{
		IMU_Orientation component = null;
		for (int i = 0; i < bodyParts.Length; i++) 
		{
			component = bodyParts[i].orientation;
			if (!component.IsInitialized) 
			{
				return component;
			}
		}
		return null;
	}
}
