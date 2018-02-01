using UnityEngine;

public class IMU_Orientation : MonoBehaviour
{
    //float m_checksum;
    [Range(0, 1)]
    public float rotationSpeed = 0.5f;
    IMU_SerialCommunication m_serialManager;

    void Start()
    {
        m_serialManager = IMU_SerialCommunication.Instance as IMU_SerialCommunication;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        string latestFrame = m_serialManager.ReturnData;
        if (latestFrame != null)
        {
            //int calibStatus = 0;
            Quaternion newRotation = ParseRotation(latestFrame/*, ref calibStatus*/);
            if (newRotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed);
            }
        }
    }

    Quaternion ParseRotation(string frame/*, ref int calibrationStatus*/)
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
        //}
        //return transform.rotation;
    }
}