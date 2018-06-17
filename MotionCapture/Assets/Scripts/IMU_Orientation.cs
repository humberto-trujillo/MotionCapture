using System.Collections;
using UnityEngine;

public class IMU_Orientation : MonoBehaviour
{
	public bool IsInitialized {get { return m_isInit;}}
	bool m_isInit = false;

	public BoneType boneType;

    [Range(0, 1)]
    public float interpolationSpeed = 0.5f;
	UdpConnectedIMU m_IMUConnection;

	private Quaternion initialOffset;
	private Quaternion importOffset = new Quaternion(0,0,1,0);
	private bool isInitialQuaternion = true;

	private Quaternion finalRotation;

	private Quaternion lastRawQuaternion;
	private float corruptThreshold = 3.0f;
		
	public void Init(UdpConnectedIMU connection)
	{
		m_IMUConnection = connection;
		m_isInit = true;
	}

    void Update()
    {
        if(m_IMUConnection != null)
        {
            string latestFrame = m_IMUConnection.LatestMessage;
			if (!string.IsNullOrEmpty (latestFrame) && !latestFrame.Equals("standBy")) 
			{
				Debug.Log(latestFrame);
				Quaternion rotation = ParseOrientationFrame (latestFrame);
				if(isInitialQuaternion)
				{
					initialOffset = Quaternion.Inverse(rotation);
					isInitialQuaternion = false;
					return;
				}
				finalRotation = isBoneRotated()? rotation * initialOffset * importOffset : rotation * initialOffset;
				transform.localRotation = Quaternion.Slerp (transform.localRotation, finalRotation, interpolationSpeed);
			}
        }
    }

    Quaternion ParseOrientationFrame(string frame)
    {
        Quaternion rotation = new Quaternion();
		Quaternion raw = new Quaternion();

        string[] tokens = frame.Split(',');
		if (tokens.Length < 4) 
		{
			Debug.LogWarning("In StandBy");
			return transform.localRotation;
		}

		bool parsingResult = false;
		float w, x, y, z;

		parsingResult = float.TryParse(tokens[0], out w);
		parsingResult = float.TryParse(tokens[1], out x);
		parsingResult = float.TryParse(tokens[2], out y);
		parsingResult = float.TryParse(tokens[3], out z);

		if (!parsingResult) 
		{
			Debug.LogWarning ("Parsing error with frame: "+ frame);
			return transform.localRotation;
		}
		raw.Set(x,y,z,w);
		if(lastRawQuaternion != null)
		{
			if(isQuaternionCorrupt(raw))
			{
				return transform.localRotation;
			}
		}
		lastRawQuaternion = new Quaternion(raw.x,raw.y,raw.z,raw.w);
        //Cordinate system transformation
        rotation.Set(-x, -z, -y, w);
		//return (isQuaternionCorrupt(rotation))? transform.localRotation : rotation;
		return rotation;
    }

	bool isBoneRotated()
	{
		return (boneType == BoneType.LeftUpperLeg || boneType == BoneType.RightUpperLeg);
	}

	bool isQuaternionCorrupt(Quaternion q)
	{
		return Mathf.Abs(q.w - lastRawQuaternion.w) > corruptThreshold ||
			Mathf.Abs(q.x - lastRawQuaternion.x) > corruptThreshold ||
			Mathf.Abs(q.y - lastRawQuaternion.y) > corruptThreshold ||
			Mathf.Abs(q.z - lastRawQuaternion.z) > corruptThreshold;
	}
}
