using UnityEngine;

public class IMU_Orientation : MonoBehaviour
{
	public bool IsInitialized {get { return m_isInit;}}
	bool m_isInit = false;

    [Range(0, 1)]
    public float interpolationSpeed = 0.5f;
    TcpConnectedIMU m_IMUConnection;

    void Start()
    {
        transform.rotation = Quaternion.identity;
    }

    public void Init(TcpConnectedIMU connection)
    {
        m_IMUConnection = connection;
		m_isInit = true;
    }

    void Update()
    {
        if(m_IMUConnection != null)
        {
            string latestFrame = m_IMUConnection.LatestMessage;
			if (!string.IsNullOrEmpty (latestFrame)) 
			{
				//int calibStatus = 0;
				Quaternion newOrientation = ParseOrientationFrame (latestFrame/*, ref calibStatus*/);
				if (newOrientation != Quaternion.identity) 
				{
					transform.rotation = Quaternion.Slerp (transform.rotation, newOrientation, interpolationSpeed);
				}
			}
        }
    }

    Quaternion ParseOrientationFrame(string frame/*, ref int calibrationStatus*/)
    {
        Quaternion rotation = Quaternion.identity;
        string[] tokens = frame.Split(',');

        float w = float.Parse(tokens[0]);
        float x = float.Parse(tokens[1]);
        float y = float.Parse(tokens[2]);
        float z = float.Parse(tokens[3]);

        rotation.Set(x, y, z, w);
        //calibrationStatus = int.Parse(tokens[tokens.Length - 3]);
        //m_checksum = Mathf.Sqrt(w * w + x * x + y * y + z * z);
        //if(Mathf.Abs(m_checksum - float.Parse(tokens[tokens.Length-2])) < 0.1)
        //{
        Quaternion rotationAdjust = Quaternion.identity;
        // http://www.euclideanspace.com/maths/algebra/realNormedAlgebra/quaternions/transforms/examples/index.htm
        // 90 degree adjustment because of the mounting orientation
        rotationAdjust.Set(0.7071f, 0, 0, 0.7071f);
        return rotationAdjust * rotation;
    }
}
