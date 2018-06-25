using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectedSensor : MonoBehaviour 
{
	public BoneType boneType = BoneType.None;
	public TextMeshProUGUI ipAddressText;
	public UdpConnectedIMU m_imuConnection;

	public void Init(UdpConnectedIMU conenction)
	{
		m_imuConnection = conenction;
		if(ipAddressText != null)
		{
			ipAddressText.text = m_imuConnection.ipAddress;
        }
	}
}
