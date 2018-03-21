using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class IMU_UdpCommunication : Singleton<IMU_UdpCommunication> 
{
	public int port = 5001;
	List<IPEndPoint> clientList = new List<IPEndPoint>();
	UdpClient connection;

	public override void Awake()
	{
		base.Awake();
		connection = new UdpClient(port);
		connection.BeginReceive(OnReceive, null);
		Debug.Log("Waiting for connections...");
	}

	public void AddClient(IPEndPoint ipEndpoint)
    {
		if(clientList.Contains(ipEndpoint) == false)
		{ // If it's a new client, add to the client list
			Debug.Log("Connection from "+ipEndpoint);
			clientList.Add(ipEndpoint);
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
			byte[] data = connection.EndReceive(ar, ref ipEndpoint);
			AddClient(ipEndpoint);

			string message = System.Text.Encoding.UTF8.GetString(data);
			Debug.Log(message);

		}
		catch(SocketException e)
		{
			Debug.Log("Client disconnected! "+ e);
		}
		connection.BeginReceive(OnReceive, null);
    }

    private void OnApplicationQuit()
    {
		connection.Close();
    }

}
