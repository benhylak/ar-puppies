using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogMovementController : MonoBehaviour {

	public Camera camera;
	private NavMeshAgent agent;

	public GameObject head;

	private int Move;
	bool BackWards =false;
	private Animator anim;
	bool ChangePose = false;
	int Pose = 0;

	public float forwardSpeed = 2.0f;
	private float rotateSlerpAmt = 2.2f;

	private float CrossfadeVal = 0.25f;

	public bool LookAt = true;
	public bool outOfRange = false;

	public Quaternion lastRotation = Quaternion.identity;

	private string _animatorName;

	private DogAnimator _dogAnimator;

	private float acceptableError = .12f;

	public bool ReachedDestination
	{
		get
		{
			if(agent.enabled && agent.isActiveAndEnabled)
			{
				return Vector3.Distance(agent.destination, transform.position) < acceptableError;
			}
			else return true;
		}
	}

	public enum MoveType
	{
		IDLE = 1,
		SNIFF_AND_WALK = 2,
		WALK = 3,
		GALLOP = 4,
		RUN = 5,
		BUTT_WIPE
	}

	void Start () 
	{
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent>();

		_dogAnimator = GetComponent<DogAnimator>();
		_animatorName = anim.name;

		//anim.StopPlayback();

		//head = GameObject.Find("Head");
		// AnimatorName = anim.name;
		// print ("name " + AnimatorName);
	}
	
	private void TryLookAtCamera()
	{
		var dir = camera.transform.position - head.transform.position;
		var targetRot = Quaternion.LookRotation(dir) * Quaternion.Euler(15f, 0, 0);
		targetRot *= Quaternion.Euler(0f, -90f, -90f); //flip for orientation

		//constrain
		var identity = (head.transform.parent != null) ? head.transform.parent.rotation : Quaternion.identity;
		var a = Quaternion.Angle(identity, targetRot);

		//deadband of 60<->90, prevents flickering
		if(a > 90f) outOfRange = true;
		else if(a < 60f) outOfRange = false;

		if (!outOfRange)
		{
			if(lastRotation == Quaternion.identity) //initialize lastRotation if a perfect identity quaternion
			{
				lastRotation = head.transform.rotation;
			}

			lastRotation = Quaternion.Slerp(lastRotation, targetRot, 0.3f);
			head.transform.rotation = lastRotation;
		}
	}

	void LateUpdate()
	{
		if(LookAt) TryLookAtCamera();
	
		if(!LookAt || outOfRange)
		{
			lastRotation = Quaternion.Slerp(lastRotation, head.transform.rotation, 0.2f);
		}

		head.transform.rotation = lastRotation;
	}
	
    void Update()
    {
		//todo use NavMeshPath.corners to rotate quickly around

		if(Input.GetMouseButtonDown(0))
		{
			_dogAnimator.Sit();

			// Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			// RaycastHit hit;

			// if(Physics.Raycast(ray, out hit))
			// {
			// 	NavigateTo(hit.point, MoveType.WALK);
			// }
		}

		if(agent.isActiveAndEnabled && Vector3.Distance(transform.position, agent.steeringTarget) > acceptableError)
		{
			var steeringDirection = (agent.steeringTarget - transform.position).normalized;

			Quaternion rotationGoal= Quaternion.LookRotation(steeringDirection);

			if(Vector3.Magnitude(steeringDirection) > 0.25f) //error
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, Time.deltaTime * 2.2f);
			}

			//if we are, more or less, facing the right direction, walk normal speed. else, half of normal speed.
			if(Quaternion.Angle(transform.rotation, rotationGoal) < 30)
			{
				agent.speed = forwardSpeed;
			}
			else
			{
				agent.speed = forwardSpeed/2;
			}
		}
		else if(agent.isActiveAndEnabled && ReachedDestination)
		{
			if(_dogAnimator._desiredPose == DogAnimator.PoseType.SIT)
			{
				_dogAnimator.Sit();
			}
			else
			{
				_dogAnimator.Idle();
			}

			agent.enabled = false;
		}

		if(Input.GetKeyDown("space"))
		{
			RandomMove();
		}
	}

	public void RandomMove()
	{
		RandomMove(MoveType.WALK);
	}

	public void Lay()
	{
		agent.enabled = false;
		_dogAnimator.Lay();
	}

	public async void Sniff()
	{
		_dogAnimator.Sniff();
	}

	public void IdleBark()
	{	
		_dogAnimator.IdleBark();
	}

	public void RandomMove(MoveType moveType)
	{
		agent.enabled = true;

		float maxDist = 0f;
		Vector3 maxDirection = Vector3.zero;
		RaycastHit maxHit; 

		for(int i=0; i<4; i++)
		{
			int degrees = Random.Range(20, 340);

			Vector3 proposedForward = Quaternion.Euler(0, degrees, 0) * this.transform.forward;

			RaycastHit hit;

			if(Physics.Raycast(new Ray(this.transform.position, proposedForward), out hit))
			{
				if(hit.distance > maxDist)
				{
					maxDirection = proposedForward;
					maxDist = hit.distance;
					maxHit = hit;
				}
			}
		}

		Debug.DrawRay(this.transform.position, maxDirection * maxDist, Color.red, 5f);

		float distanceToWalk = ((float)Random.Range(2, 9))/10f * maxDist; //go between 20 -> 90% of the max distance

		Vector3 nextDest = this.transform.position + maxDirection.normalized * distanceToWalk;

		Debug.DrawRay(this.transform.position, maxDirection * distanceToWalk, Color.green, 5f);

		this.NavigateTo(nextDest, moveType);
	}

	public void Sit()
	{
		agent.enabled = false;
		_dogAnimator.Sit();
	}

	public void NavigateToCamera(MoveType moveType)
	{
		var position = camera.transform.position + camera.transform.forward.normalized * 2f;
		position.y = 0;

		this.NavigateTo(position, moveType);
	}

	public void NavigateTo(Vector3 position, MoveType moveType)
	{
		agent.enabled = true;
		agent.SetDestination(position);

		switch(moveType)
		{
			case MoveType.RUN:
				forwardSpeed = 2f;
				rotateSlerpAmt = 3.4f;
				_dogAnimator.Run();
				break;

			case MoveType.SNIFF_AND_WALK:
				forwardSpeed = 0.5f;
				rotateSlerpAmt = 2.2f;
				_dogAnimator.SniffAndWalk();
				break;

			case MoveType.BUTT_WIPE:
				forwardSpeed = 0.35f;
				rotateSlerpAmt = 4f;
				_dogAnimator.ButtWipe();
				break;

			default: //walk
				forwardSpeed = 0.6f;
				rotateSlerpAmt = 2.2f;
				_dogAnimator.Walk();

				break;
		}

		//implement get speed func?

		//anim.CrossFade (_animatorName + animationName, CrossfadeVal);
	}

	// void jumpTo()
}
