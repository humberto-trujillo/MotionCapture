using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour {
	
	public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
	public float delay = 1f;
	public float moveTime = 1f;
	public Transform startPosition;

	public Transform[] positions;

	private void Start()
	{
		StartCoroutine(TestFocusRoutine());	
	}

	public void FocusTo(Transform newPos)
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", newPos,
            "time", moveTime,
            "delay", delay,
            "easetype", easeType
        ));
        iTween.RotateTo(gameObject, iTween.Hash(
			"rotation", newPos,
            "time", moveTime,
            "delay", delay,
            "easetype", easeType
        ));
	}

	public void Reset()
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", startPosition,
            "time", moveTime,
            "delay", delay,
            "easetype", easeType
        ));
        iTween.RotateTo(gameObject, iTween.Hash(
			"rotation", startPosition,
            "time", moveTime,
            "delay", delay,
            "easetype", easeType
        ));
	}

	IEnumerator TestFocusRoutine()
	{
		foreach (var pos in positions)
		{
			yield return new WaitForSeconds(2f);
			FocusTo(pos);
			yield return new WaitForSeconds(2f);
			Reset();
        }
	}
}
