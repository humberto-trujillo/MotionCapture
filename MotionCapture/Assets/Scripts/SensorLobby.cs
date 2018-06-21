using UnityEngine;
using UnityEngine.UI;

public class SensorLobby : MonoBehaviour
{
	public Button startButton;
	public bool isConfigured = false;

	private void Start()
	{
		startButton.interactable = false;
	}

	public void StartMotionCapture()
	{
		
	}
}
