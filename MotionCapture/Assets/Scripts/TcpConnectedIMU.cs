using System.Net.Sockets;
using System;
using System.Net;
using UnityEngine;

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
	public string IPAddress { get { return ((IPEndPoint)m_connection.Client.RemoteEndPoint).Address.ToString(); } }

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
		Debug.Log (m_latestMessage);
		Stream.BeginRead(m_readBuffer, 0, m_readBuffer.Length, OnRead, null);
	}
}
