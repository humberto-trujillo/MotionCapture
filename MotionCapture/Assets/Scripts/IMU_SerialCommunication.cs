using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class IMU_SerialCommunication : IMU_Communication
{
    public string port = "COM3"; //When using MacOS -> "/dev/cu.usbmodem1411"
    public int baudRate = 38400;
    SerialPort m_stream;
    Thread m_receiveDataThread;

    string m_returnData;
    public string ReturnData { get { return m_returnData; } }

    public override void Awake()
    {
        base.Awake();
        m_stream = new SerialPort(port, baudRate);
        InitComm();
    }

    public override void InitComm()
    {
        base.InitComm();
        m_receiveDataThread = new Thread(new ThreadStart(FetchData));
        m_receiveDataThread.Start();
    }

    public override void FetchData()
    {
        base.FetchData();
        Debug.Log("Starting Serial COMM Thread!");
        m_stream.Open();
        Debug.Log("Serial port " + port + " Open!");
        while (true)
        {
            if (!m_stream.IsOpen)
            {
                break;
            }
            m_returnData = m_stream.ReadLine();
            if (m_returnData != null)
            {
                m_stream.BaseStream.Flush();
            }
        }
    }

    public string[] SplittedData()
    {
        string[] tokens = m_returnData.Split(',');
        return tokens;
    }

    public override void OnCommTerminate()
    {
        base.OnCommTerminate();
        m_stream.Close();
        Debug.Log("Serial port " + port + " closed!");
    }
}