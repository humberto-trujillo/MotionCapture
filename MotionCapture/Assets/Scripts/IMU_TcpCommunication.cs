using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class IMU_TcpCommunication : Singleton<IMU_TcpCommunication>
{
    public delegate void OnClientConnected(TcpConnectedIMU connection);
    public event OnClientConnected ClientConnectedEvent;

    public int port = 5001;

	/// <summary>
	/// Lista de conexiones
	/// </summary>
	List<TcpConnectedIMU> m_clientList = new List<TcpConnectedIMU>();

	/// <summary>
	/// Acepta nuevas conexiones
	/// </summary>
	TcpListener m_listener;

    public override void Awake()
    {
        base.Awake();
        m_listener = new TcpListener(IPAddress.Any, port);
        m_listener.Start();
        Debug.Log("Waiting for connections...");
		m_listener.BeginAcceptTcpClient(OnServerConnect, null);
	}

	void OnApplicationQuit()
	{
		if(m_listener != null)
		{
			m_listener.Stop();
		}
		for(int i = 0; i < m_clientList.Count; i++)
		{
			m_clientList[i].Close();
		}
	}

	#region Async Events
	void OnServerConnect(IAsyncResult ar)
	{
		TcpClient tcpClient = m_listener.EndAcceptTcpClient(ar);
        TcpConnectedIMU connection = new TcpConnectedIMU(tcpClient);
        m_clientList.Add(connection);
        Debug.Log("Client connected!");
        if(ClientConnectedEvent != null)
        {
            ClientConnectedEvent(connection);
        }
        m_listener.BeginAcceptTcpClient(OnServerConnect, null);
	}
	#endregion

	public void OnDisconnect(TcpConnectedIMU client)
	{
		m_clientList.Remove(client);
		Debug.Log ("Client disconnected!");
	}
}
