using System.Collections.Generic;
using UnityEngine;

public class MotionCaptureManager : MonoBehaviour
{
	public enum BodyPartType
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

    [System.Serializable]
    public struct BodyPart
    {
        public GameObject bodyPart;
		public BodyPartType type;
		public IMU_Orientation orientation;
    }
	IMU_UdpCommunication m_udpCommunication;
	public BodyPart[] bodyParts;


    void Start ()
    {
		m_udpCommunication = IMU_UdpCommunication.Instance;
		m_udpCommunication.OnClientConnected += InitOrientationComponent;
		for(int i=0; i<bodyParts.Length;i++)
        {
			bodyParts[i].orientation = bodyParts[i].bodyPart.AddComponent<IMU_Orientation>();
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
