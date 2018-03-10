using System.Collections.Generic;
using UnityEngine;

public class MotionCaptureManager : MonoBehaviour
{
    [System.Serializable]
    public struct BodyPart
    {
        public GameObject bodyPart;
        public Material material;
    }

    IMU_TcpCommunication m_tcpCommunication;
    public List<BodyPart> bodyParts = new List<BodyPart>();

    int bodyPartIndex = 0;

    void Awake()
    {
        m_tcpCommunication = IMU_TcpCommunication.Instance;
    }

    void Start ()
    {
        m_tcpCommunication.ClientConnectedEvent += InitOrientationComponent;
        foreach (var part in bodyParts)
        {
            part.bodyPart.AddComponent<IMU_Orientation>();
        }
	}

    void InitOrientationComponent(TcpConnectedIMU connection)
    {
        bodyParts[bodyPartIndex++].bodyPart.GetComponent<IMU_Orientation>().Init(connection);
    }
}
