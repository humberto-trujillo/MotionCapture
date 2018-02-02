using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class IMU_TcpCommunication : MonoBehaviour 
{
    public int port = 56789;
	/// <summary>
	/// Lista de conexiones
	/// </summary>
	List<TcpConnectedIMU> clientList = new List<TcpConnectedIMU>();

	/// <summary>
	/// El string para renderizar en Unity.
	/// </summary>
	public static string messageToDisplay;

	/// <summary>
	/// Acepta nuevas conexiones
	/// </summary>
	TcpListener listener;

	void Awake()
	{
        //base.Awake();
		listener = new TcpListener(IPAddress.Any, port);
		listener.Start();
        Debug.Log("Waiting for connections...");
		listener.BeginAcceptTcpClient(OnServerConnect, null);
	}

	void OnApplicationQuit()
	{

		if(listener != null)
		{
			listener.Stop();
		}
		for(int i = 0; i < clientList.Count; i++)
		{
			clientList[i].Close();
		}
	}

	#region Async Events
	void OnServerConnect(IAsyncResult ar)
	{
		TcpClient tcpClient = listener.EndAcceptTcpClient(ar);
		clientList.Add(new TcpConnectedIMU(tcpClient));
		listener.BeginAcceptTcpClient(OnServerConnect, null);
		Debug.Log ("Cliente conectado!");
	}
	#endregion

	public void OnDisconnect(TcpConnectedIMU client)
	{
		clientList.Remove(client);
	}
}
