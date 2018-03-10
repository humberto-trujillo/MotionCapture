using System.Net.Sockets;
using System;

[System.Serializable]
public class TcpConnectedIMU
{
	/// <summary>
	/// Conexión a IMU (cliente)
	/// </summary>
	readonly TcpClient m_connection;
	readonly byte[] m_readBuffer = new byte[5000];
	NetworkStream Stream { get{ return m_connection.GetStream(); } }

    string m_latestMessage;
    public string LatestMessage { get { return m_latestMessage; } }

    public TcpConnectedIMU(TcpClient tcpClient)
	{
		m_connection = tcpClient;
		m_connection.NoDelay = true;
		Stream.BeginRead(m_readBuffer, 0, m_readBuffer.Length, OnRead, null);
	}

	internal void Close()
	{
		m_connection.Close();
	}

	void OnRead(IAsyncResult ar)
	{
		int length = Stream.EndRead(ar);
		if(length <= 0)
		{
            IMU_TcpCommunication.Instance.OnDisconnect(this);
			return;
		}
        m_latestMessage = System.Text.Encoding.UTF8.GetString(m_readBuffer, 0, length);
		Stream.BeginRead(m_readBuffer, 0, m_readBuffer.Length, OnRead, null);
	}

	internal void EndConnect(IAsyncResult ar)
	{
		m_connection.EndConnect(ar);
		Stream.BeginRead(m_readBuffer, 0, m_readBuffer.Length, OnRead, null);
	}
}
