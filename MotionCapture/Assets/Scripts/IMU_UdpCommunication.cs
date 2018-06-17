using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class IMU_UdpCommunication : Singleton<IMU_UdpCommunication> 
{
	public delegate void ClientConnected(UdpConnectedIMU connection);
	public event ClientConnected OnClientConnected;

	public int port = 5001;
	List<IPEndPoint> clientList = new List<IPEndPoint>();

	public List<UdpConnectedIMU> connections = new List<UdpConnectedIMU>();
	UdpClient serverConnection;

	public override void Awake()
	{
		base.Awake();
		serverConnection = new UdpClient(port);
	}

	void Start()
	{
		serverConnection.BeginReceive(OnReceive, null);
		Debug.Log("Waiting for connections...");
	}

	public void AddClient(IPEndPoint ipEndpoint)
    {
		Debug.Log("Connection from "+ipEndpoint);
		clientList.Add(ipEndpoint);

		UdpConnectedIMU connection = new UdpConnectedIMU(ipEndpoint.Address.ToString());
		connections.Add(connection);

		if (OnClientConnected != null) 
		{
			OnClientConnected (connection);
		}
    }

    /// <summary>
    /// TODO: add timestamps to timeout and remove clients from the list.
    /// </summary>
    public void RemoveClient(IPEndPoint ipEndpoint)
    { 
    	clientList.Remove(ipEndpoint);
    }

    void OnReceive(IAsyncResult ar)
    {
		try
		{
			IPEndPoint ipEndpoint = null;
			byte[] data = serverConnection.EndReceive(ar, ref ipEndpoint);

			if(clientList.Contains(ipEndpoint) == false)
			{
				AddClient(ipEndpoint);
			}

			string message = System.Text.Encoding.UTF8.GetString(data);
			Debug.Log(message);
			UdpConnectedIMU correspondingConnection = connections.Find(con => con.ipAddress == ipEndpoint.Address.ToString());
			if(correspondingConnection != null)
			{
				correspondingConnection.LatestMessage = message;
			}
			else
			{
				Debug.Log("Connection not registered!");
			}
		}
		catch(SocketException e)
		{
			Debug.Log("Client disconnected! "+ e);
		}
		serverConnection.BeginReceive(OnReceive, null);
    }

	public void SendToAll(string msg)
	{
		Byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(msg);
		foreach (var client in clientList)
		{
			try
			{
				serverConnection.Send(sendBytes, sendBytes.Length, client);
			}
			catch (Exception e)
			{
				Debug.LogWarning(e.ToString());	
			}
		}
	}

	private void OnApplicationQuit()
    {
		SendToAll("StandBy");
		serverConnection.Close();
    }
}