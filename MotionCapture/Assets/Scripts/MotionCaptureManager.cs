using System.Collections.Generic;
using UnityEngine;

public class MotionCaptureManager : MonoBehaviour
{
    [System.Serializable]
    public struct BodyPart
    {
        public GameObject bodyPart;
		public IMU_Orientation orientation;
    }

    IMU_TcpCommunication m_tcpCommunication;
	public BodyPart[] bodyParts;

    void Start ()
    {
		m_tcpCommunication = IMU_TcpCommunication.Instance;
        m_tcpCommunication.ClientConnectedEvent += InitOrientationComponent;
		for(int i=0; i<bodyParts.Length;i++)
        {
			bodyParts[i].orientation = bodyParts[i].bodyPart.AddComponent<IMU_Orientation>();
        }
	}

    void InitOrientationComponent(TcpConnectedIMU connection)
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
