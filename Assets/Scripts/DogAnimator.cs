using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DogAnimator : MonoBehaviour {

	private Animator anim;
	private string AnimatorName;
	private int Move;
	public PoseType _desiredPose;
	bool ChangePose = false;
	public bool StateChangeComplete = true;
	//public GameObject target;
	private string CurrentButtonPressed = "Stand";

	private GameObject AggressiveButton;
	private GameObject LayButton;
	private GameObject StandButton;
	private GameObject SitButton;
	private GameObject ConsumeButton;

	private float CrossfadeVal = 0.25f;

	private PoseType _currentPose = PoseType.STANDS;

	public enum PoseType
	{
		STANDS,
		MOVING,
		CONSUME,
		AGGRESSIVE,
		SIT, 
		LAY
	}

	public void Lay()
	{
		_desiredPose = PoseType.LAY;

		if(_currentPose != _desiredPose)
		{
			TransitionPose();
		}
		else
		{
			anim.CrossFade (AnimatorName + "Lay", 0.3f);
		}
	}

	public void Idle()
	{
		_desiredPose = PoseType.STANDS;

		if(_currentPose != PoseType.STANDS)
		{
			TransitionPose();
		}
		else
		{
			anim.CrossFade(AnimatorName + "Idle", 0.25f);
		}
	}

	public async void IdleBark()
	{
		_desiredPose = PoseType.STANDS;

		switch(_currentPose)
		{
			case PoseType.AGGRESSIVE:
			case PoseType.LAY:
			case PoseType.SIT:
				TransitionPose();

				while(anim.IsInTransition(0))
				{
					await Task.Delay(30);
				}

				break;
		}

		anim.CrossFade (AnimatorName + "IdleBarkingLong", 0.3f);
	}

	public async void ButtWipe()
	{
		_desiredPose = PoseType.SIT;

		if(_currentPose != _desiredPose)
		{
			TransitionPose();

			while(anim.IsInTransition(0))
			{
				await Task.Delay(30);
			}
		}

		anim.CrossFade (AnimatorName + "WipeAss", 0.3f);
	}

	public async void Walk()
	{
		_desiredPose = PoseType.MOVING;

		switch(_currentPose)
		{
			case PoseType.AGGRESSIVE:
			case PoseType.LAY:
			case PoseType.SIT:
				TransitionPose();

				while(anim.IsInTransition(0))
				{
					await Task.Delay(30);
				}

				break;
		}

		anim.CrossFade (AnimatorName + "Walk", 0.3f);
	}

	public async void Run()
	{
		_desiredPose = PoseType.MOVING;

		switch(_currentPose)
		{
			case PoseType.AGGRESSIVE:
			case PoseType.LAY:
			case PoseType.SIT:
				TransitionPose();

				while(anim.IsInTransition(0))
				{
					await Task.Delay(30);
				}

				break;
		}

		anim.CrossFade (AnimatorName + "Run", 0.3f);
	}

	public void Sit()
	{
		_desiredPose = PoseType.SIT;

		if(_currentPose != _desiredPose)
		{
			TransitionPose();
		}
		else
		{				
			anim.CrossFade (AnimatorName + "SitIdle", 0.3f);
		}
	}

	public async void SniffAndWalk()
	{
		_desiredPose = PoseType.MOVING;

		switch(_currentPose)
		{
			case PoseType.AGGRESSIVE:
			case PoseType.LAY:
			case PoseType.SIT:
				TransitionPose();

				while(anim.IsInTransition(0))
				{
					await Task.Delay(30);
				}

				break;
		}

		anim.CrossFade (AnimatorName + "WalkSniff", 0.3f);
	}


	public async void Sniff()
	{
		_desiredPose = PoseType.STANDS;

		switch(_currentPose)
		{
			case PoseType.AGGRESSIVE:
			case PoseType.LAY:
			case PoseType.SIT:
				TransitionPose();

				while(anim.IsInTransition(0))
				{
					await Task.Delay(30);
				}

				break;
		}

		anim.CrossFade (AnimatorName + "IdleSniff", 0.3f);
	}

	void Start () 
	{
		anim = GetComponent<Animator> ();
		AnimatorName = anim.name;
	}

	void Update () 
	{
		// if (ChangePose) 
		// {
		// 	print ("Change Pose");
		// 	ChangePose = false;

		// 	string animationName = "";

			

		// 	_currentPose = _desiredPose;
		// }
	}

	private void TransitionPose()
	{
		string animationName = "";

		switch(_currentPose)
		{
			case PoseType.STANDS:
			case PoseType.MOVING:
			case PoseType.CONSUME:
			{
				animationName += "Idle";
				break;
			}

			case PoseType.AGGRESSIVE:
			{
				animationName += "Aggressive";
				break;
			}

			case PoseType.SIT:
			{
				animationName += "Sit";
				break;
			}

			case PoseType.LAY:
			{
				animationName += "Lay";
				break;
			}
		}

		animationName += "To";

		switch(_desiredPose)
		{
			case PoseType.STANDS:
				animationName += "Idle";
				break;

			case PoseType.MOVING:
				animationName += "Idle";
				break;
			
			case PoseType.AGGRESSIVE:
				animationName += "Aggressive";
				break;
			
			case PoseType.LAY:
				animationName += "Lay";
				break;
			
			case PoseType.SIT:
				animationName += "Sit";
				break;
			
			case PoseType.CONSUME:
				animationName += "Eat";
				break;
		}

		print("Changing pose w/ transition: " + animationName);

		_currentPose = _desiredPose;

		anim.CrossFade (AnimatorName+animationName, CrossfadeVal);
	}

	bool BackWards =false;

	
	void ResetButtonNames()
	{
		GameObject ButtonToReset = GameObject.Find(CurrentButtonPressed);
		ButtonToReset.GetComponentInChildren<Text> ().text = CurrentButtonPressed;
		print ("change button name and it is now " + ButtonToReset.GetComponentInChildren<Text> ().text);
		ButtonToReset.GetComponentInChildren<ChangeButtonText> ().ValuetoGet = 0;
	}
}
