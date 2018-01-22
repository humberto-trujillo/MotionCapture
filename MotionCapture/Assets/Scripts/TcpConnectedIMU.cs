using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;

public class TcpConnectedIMU
{
	/// <summary>
	/// Conexión a IMU (cliente)
	/// </summary>
	readonly TcpClient connection;

	readonly byte[] readBuffer = new byte[5000];
	NetworkStream stream { get{ return connection.GetStream(); } }

	public TcpConnectedIMU(TcpClient tcpClient)
	{
		connection = tcpClient;
		connection.NoDelay = true; // deshabilitar caché
		stream.BeginRead(readBuffer, 0, readBuffer.Length, OnRead, null);
	}

	internal void Close()
	{
		connection.Close();
	}

	void OnRead(IAsyncResult ar)
	{
		int length = stream.EndRead(ar);
		if(length <= 0)
		{ // Connection closed
			//TCPChat.instance.OnDisconnect(this);
			return;
		}

		string newMessage = System.Text.Encoding.UTF8.GetString(readBuffer, 0, length);
		//TcpIMUCommunication.messageToDisplay += newMessage + Environment.NewLine;
		Debug.Log(newMessage);
//		if(TCPChat.instance.isServer)
//		{
//			TCPChat.BroadcastChatMessage(newMessage);
//		}

		stream.BeginRead(readBuffer, 0, readBuffer.Length, OnRead, null);
	}

	internal void EndConnect(IAsyncResult ar)
	{
		connection.EndConnect(ar);
		stream.BeginRead(readBuffer, 0, readBuffer.Length, OnRead, null);
	}
}
