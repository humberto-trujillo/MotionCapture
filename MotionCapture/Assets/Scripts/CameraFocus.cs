using UnityEngine;

public class CameraFocus : MonoBehaviour {
	
	public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
	public float delay = 1f;
	public float moveTime = 1f;
	public Transform startPosition;

	[Header("Positions")]
	[SerializeField] Transform Head;
	[SerializeField] Transform Torso;
	[SerializeField] Transform RightForearm;
	[SerializeField] Transform RightShoulder;
	[SerializeField] Transform LeftForearm;
	[SerializeField] Transform LeftShoulder;
	[SerializeField] Transform RightUpperLeg;
	[SerializeField] Transform RightLowerLeg;
	[SerializeField] Transform LeftUpperLeg;
	[SerializeField] Transform LeftLowerLeg;

	public void FocusTo(BoneType boneType)
	{
		switch (boneType)
		{
			case BoneType.Head:
				FocusTo(Head);
				break;
			case BoneType.Torso:
				FocusTo(Torso);
                break;
			case BoneType.RightForearm:
				FocusTo(RightForearm);
                break;
			case BoneType.RightShoulder:
				FocusTo(RightShoulder);
                break;
			case BoneType.LeftForearm:
				FocusTo(LeftForearm);
                break;
			case BoneType.LeftShoulder:
				FocusTo(LeftShoulder);
                break;
			case BoneType.RightUpperLeg:
				FocusTo(RightUpperLeg);
                break;
			case BoneType.RightLowerLeg:
				FocusTo(RightLowerLeg);
                break;
			case BoneType.LeftUpperLeg:
				FocusTo(LeftUpperLeg);
				break;
			case BoneType.LeftLowerLeg:
				FocusTo(LeftLowerLeg);
                break;
			default:
				break;
		}
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
}
