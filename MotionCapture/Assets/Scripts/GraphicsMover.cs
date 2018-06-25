using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsMover : MonoBehaviour 
{
	public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
	public float delay = 1f;
	public float moveTime = 1f;

	public RectTransform endPosition;
	public Vector3 scaleBy;

	public void MoveOffScreen()
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", endPosition,
            "time", moveTime,
            "delay", delay,
            "easetype", easeType
        ));
	}

	public void MoveToScreen()
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", endPosition,
            "time", moveTime,
            "delay", delay,
            "easetype", easeType
        ));
	}

	public void ScaleGraphic()
	{
		iTween.ScaleTo(gameObject, iTween.Hash(
			"scale", scaleBy,
            "time", moveTime,
            "delay", delay,
            "easetype", easeType
        ));
	}

	public void ResetTransform()
	{
		transform.localScale = Vector3.one;
	}

}
