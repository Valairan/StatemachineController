using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
	[SerializeField]
	public PlayerBaseState currentState;

	#region States
	public IdleState idleState = new IdleState();
	public WalkState walkState = new WalkState();
	public RunState runState = new RunState();
	public JumpState jumpState = new JumpState();
	public HangState hangState = new HangState();
	#endregion

	#region Declarations
	public float g = -9.18f;
	#region Attached Components

	[Header("Attached components")]
	[SerializeField] private CharacterController playerCharacterController;
	public CharacterController controller { get { return playerCharacterController; } }
	[SerializeField] protected LineRenderer lineRenderer;
	#endregion
	#region Physics
	[Header("Physics")]
	[SerializeField] protected float movementSpeed;
	[SerializeField] protected bool canMove;
	[SerializeField] protected bool canJump;
	[SerializeField] protected bool canCrouch;
	[SerializeField] protected bool canClimb;
	[SerializeField] protected bool canSwim;
	[SerializeField] protected bool canFight;
	private Vector3[] curvePoints;
	public Vector3[] _curvePoints { get { return curvePoints; } }
	#endregion

	#region Attacehd Transforms
	[Header("Child Transforms")]
	[SerializeField] protected Transform RCP_GroundRayStart;
	[SerializeField] protected Transform RCP_LeftBrace;
	[SerializeField] protected Transform RCP_RightBrace;
	[SerializeField] protected Transform RCP_ClimbCheck1;
	[SerializeField] protected Transform RCP_ClimbCheck2;
	[SerializeField] protected Transform RCP_ClimbCheck3;
	[SerializeField] protected Transform RCP_ClimbCheck4;
	[SerializeField] protected Transform RCP_VaultCheck1;
	[SerializeField] protected Transform RCP_VaultCheck2;
	[SerializeField] protected Transform RCP_VaultCheck3;
	[SerializeField] protected Transform RCP_VaultCheck4;
	[SerializeField] protected Transform RCP_LeftVaultSpaceCheck;
	[SerializeField] protected Transform RCP_RighttVaultSpaceCheck;
	[SerializeField] protected Transform RCP_ForwardCollisionCheck1;
	[SerializeField] protected Transform RCP_ForwardCollisionCheck2;
	[SerializeField] protected Transform RCP_ForwardCollisionCheck3;
	[SerializeField] protected Transform RCP_ForwardCollisionCheck4;
	[SerializeField] float sphereCastRadius;
	#endregion

	public bool isGrounded;
	public bool isLeftBraced;
	public bool isRightBraced;
	public bool ClimbCheck1;
	public bool ClimbCheck2;
	public bool ClimbCheck3;
	public bool ClimbCheck4;
	public bool VaultCheck1;
	public bool VaultCheck2;
	public bool VaultCheck3;
	public bool VaultCheck4;
	public bool LeftVaultSpaceCheck;
	public bool RighttVaultSpaceCheck;
	public bool ForwardCollisionCheck1;
	public bool ForwardCollisionCheck2;
	public bool ForwardCollisionCheck3;
	public bool ForwardCollisionCheck4;

	RaycastHit RCH_GroundCheck;
	public RaycastHit RCH_groundCheck { get { return RCH_GroundCheck; } }
	RaycastHit RCH_LeftVaultSpace;
	RaycastHit RCH_RighftVaultSpace;
	RaycastHit RCH_LeftVaultSpace_upper;
	RaycastHit RCH_RighftVaultSpace_upper;
	RaycastHit RCH_LeftVaultSpace_middle;
	RaycastHit RCH_RighftVaultSpace_middle;
	RaycastHit RCH_ClimbCheck2;
	RaycastHit RCH_ClimbCheck3;
	RaycastHit RCH_ClimbCheck1;
	RaycastHit RCH_ClimbCheck4;
	RaycastHit RCH_VaultCheck1;
	RaycastHit RCH_VaultCheck2;
	RaycastHit RCH_VaultCheck3;
	RaycastHit RCH_VaultCheck4;

	private bool interactButton;
	public bool _interactButton { get { return interactButton; } }
	private bool jumpButton;
	public bool _jumpButton { get { return jumpButton; } }
	private bool mouse1;
	private bool mouse2;
	private bool crouchButton;
	public bool _crouchButton { get { return crouchButton; } }



	[HideInInspector] public Vector3 movementDirection;
	[HideInInspector] public Vector3 gravity;
	[HideInInspector] public float _movementSpeed { get { return movementSpeed; } }
	[HideInInspector] public bool _isGrounded { get { return isGrounded;  } }

	[Header("Graphics")]
	[SerializeField] Animator animator;
					 public Animator _animator { get { return animator; } }
	[SerializeField] SkinnedMeshRenderer playerMesh;
	#endregion


	void Start()
	{

		transitionToState(idleState);
		Debug.Log("Start has been called");
	}
	public void Update()
	{
		gatherInputs();
		gatherRaycasts();
		currentState.onStateUpdate();


		
	}



	private void LateUpdate()
	{
		//if(curvePoints != null)
			//lineRenderer.SetPositions(curvePoints);
		//lineRenderer.SetPositions(Curves.calculateCubicCurve(lr_p1.position, lr_p2.position, lr_p3.position, lr_p4.position, 10));
	}
	public void FixedUpdate()
	{
		handlePhysics();
	}
	public void transitionToState(PlayerBaseState state)
	{
		
		if(!(currentState == null))
		{
			currentState.onStateExit();
		}

		currentState = state;

		if (currentState.Player == null)
		{
			currentState.AssignPlayer(this);
		}
		state.onStateEnter();
	}

	
	void gatherInputs()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		jumpButton = Input.GetKey(KeyCode.Space);
		crouchButton = Input.GetKey(KeyCode.LeftControl);
		movementDirection = transform.right * horizontal + transform.forward * vertical;
		
	}

	void gatherRaycasts()
	{
		Physics.SphereCast(RCP_GroundRayStart.position, .05f, Vector3.down * .5f, out RCH_GroundCheck);
		//Physics.Raycast(RCP_GroundRayStart.position, Vector3.down, out RCH_GroundCheck,.5f );
		
		isLeftBraced = Physics.Raycast(RCP_LeftBrace.position, transform.forward, 1);
		isRightBraced = Physics.Raycast(RCP_RightBrace.position, transform.forward, 1);

		ForwardCollisionCheck1 = Physics.Raycast(RCP_ForwardCollisionCheck1.position, transform.forward, 1);
		ForwardCollisionCheck2 = Physics.Raycast(RCP_ForwardCollisionCheck2.position, transform.forward, 1);
		ForwardCollisionCheck3 = Physics.Raycast(RCP_ForwardCollisionCheck3.position, transform.forward, 1);
		ForwardCollisionCheck4 = Physics.Raycast(RCP_ForwardCollisionCheck4.position, transform.forward, 1);
		//Climb checks
		ClimbCheck1 = Physics.SphereCast(RCP_ClimbCheck1.position, sphereCastRadius, Vector3.down * 1.5f, out RCH_ClimbCheck1, 2);
		ClimbCheck2 = Physics.SphereCast(RCP_ClimbCheck2.position, sphereCastRadius, Vector3.down * 1.5f, out RCH_ClimbCheck2, 2);
		ClimbCheck3 = Physics.SphereCast(RCP_ClimbCheck3.position, sphereCastRadius, Vector3.down * 1.5f, out RCH_ClimbCheck3, 2);
		ClimbCheck4 = Physics.SphereCast(RCP_ClimbCheck4.position, sphereCastRadius, Vector3.down * 1.5f, out RCH_ClimbCheck4, 2);
		//Vault checks
		VaultCheck1 = Physics.SphereCast(RCP_VaultCheck1.position, sphereCastRadius, Vector3.down, out RCH_VaultCheck1, 1f);
		VaultCheck2 = Physics.SphereCast(RCP_VaultCheck2.position, sphereCastRadius, Vector3.down, out RCH_VaultCheck2, 1f);
		VaultCheck3 = Physics.SphereCast(RCP_VaultCheck3.position, sphereCastRadius, Vector3.down, out RCH_VaultCheck3, 1f);
		VaultCheck4 = Physics.SphereCast(RCP_VaultCheck4.position, sphereCastRadius, Vector3.down, out RCH_VaultCheck4, 1f);
		//Left and Right vault space checks
		LeftVaultSpaceCheck = Physics.SphereCast(RCP_LeftVaultSpaceCheck.position, sphereCastRadius, transform.forward, out RCH_LeftVaultSpace) 
			&& Physics.SphereCast(RCP_LeftVaultSpaceCheck.position + (Vector3.up * .5f), sphereCastRadius, transform.forward, out RCH_LeftVaultSpace_upper)
				&& Physics.SphereCast(RCP_LeftVaultSpaceCheck.position + (Vector3.up * .25f), sphereCastRadius, transform.forward, out RCH_LeftVaultSpace_middle);

		RighttVaultSpaceCheck = Physics.SphereCast(RCP_RighttVaultSpaceCheck.position, sphereCastRadius, transform.forward, out RCH_RighftVaultSpace_upper)
			&& Physics.SphereCast(RCP_RighttVaultSpaceCheck.position + (Vector3.up * .5f), sphereCastRadius, transform.forward, out RCH_RighftVaultSpace_upper)
				&& Physics.SphereCast(RCP_RighttVaultSpaceCheck.position + (Vector3.up * .25f), sphereCastRadius, transform.forward, out RCH_RighftVaultSpace_middle);


		//Handle all the raycasts
		if(Vector3.Distance(RCP_GroundRayStart.position, RCH_GroundCheck.point) < .49f)
		{
			gravity.y = 0;
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}


		Vector3 pos = new Vector3();

		if (ClimbCheck1)
		{
			pos = RCH_ClimbCheck1.point;
		}
		else
		{
			pos = RCP_ClimbCheck1.position;
		}
		if (VaultCheck1 && !ClimbCheck1)
		{
			pos = RCH_VaultCheck1.point;
		}
		else
		{
			//pos = RCP_VaultCheck1.position;
		}

		//Debug.Log("CC1 :" + ClimbCheck1 + "\nCC2 :" + ClimbCheck2 + "\n CC3 :" + ClimbCheck3 + "\n CC4 :" + ClimbCheck4);

		Vector3 midpoint = new Vector3();
		midpoint = (pos + transform.position) / 2;

		midpoint += (Vector3.up * 2) + (Vector3.back * .25f);

		//if (!currentState._currentStateName.Equals(currentStateName.Hang))
			curvePoints = Curves.calculateQuadraticCurve(transform.position, midpoint, pos, 10);
	}



	void handlePhysics()
	{

	}



	private void OnDrawGizmos()
	{
		if (RCP_GroundRayStart.Equals(null))
		{
			return;
		}
		Gizmos.color = Color.red;
		Gizmos.DrawRay(RCP_GroundRayStart.position, Vector3.down * .45f);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(RCP_LeftBrace.position, transform.forward * 1);
		Gizmos.DrawRay(RCP_RightBrace.position, transform.forward * 1);

		Gizmos.color = Color.green;
		Gizmos.DrawRay(RCP_ClimbCheck1.position, Vector3.down * 1.5f);
		Gizmos.DrawRay(RCP_ClimbCheck2.position, Vector3.down * 1.5f);
		Gizmos.DrawRay(RCP_ClimbCheck3.position, Vector3.down * 1.5f);
		Gizmos.DrawRay(RCP_ClimbCheck4.position, Vector3.down * 1.5f);
		Gizmos.DrawSphere(RCP_ClimbCheck1.position + Vector3.down * 1.5f, sphereCastRadius);
		Gizmos.DrawSphere(RCP_ClimbCheck2.position + Vector3.down * 1.5f, sphereCastRadius);
		Gizmos.DrawSphere(RCP_ClimbCheck3.position + Vector3.down * 1.5f, sphereCastRadius);
		Gizmos.DrawSphere(RCP_ClimbCheck4.position + Vector3.down * 1.5f, sphereCastRadius);
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(RCP_VaultCheck1.position, Vector3.down * 1);
		Gizmos.DrawRay(RCP_VaultCheck2.position, Vector3.down * 1);
		Gizmos.DrawRay(RCP_VaultCheck3.position, Vector3.down * 1);
		Gizmos.DrawRay(RCP_VaultCheck4.position, Vector3.down * 1);
		Gizmos.DrawSphere(RCP_VaultCheck1.position + Vector3.down, sphereCastRadius);
		Gizmos.DrawSphere(RCP_VaultCheck2.position + Vector3.down, sphereCastRadius);
		Gizmos.DrawSphere(RCP_VaultCheck3.position + Vector3.down, sphereCastRadius);
		Gizmos.DrawSphere(RCP_VaultCheck4.position + Vector3.down, sphereCastRadius);
		Gizmos.color = Color.white;
		//-----------------------------------
		Gizmos.DrawRay(RCP_LeftVaultSpaceCheck.position, transform.forward * 1);
		Gizmos.DrawRay(RCP_RighttVaultSpaceCheck.position, transform.forward * 1);

		Gizmos.DrawRay(RCP_LeftVaultSpaceCheck.position + (Vector3.up * .5f), transform.forward * 1);
		Gizmos.DrawRay(RCP_RighttVaultSpaceCheck.position + (Vector3.up * .5f), transform.forward * 1);

		Gizmos.DrawRay(RCP_LeftVaultSpaceCheck.position + (Vector3.up * .25f), transform.forward * 1);
		Gizmos.DrawRay(RCP_RighttVaultSpaceCheck.position + (Vector3.up * .25f), transform.forward * 1);

		Gizmos.DrawSphere(RCP_LeftVaultSpaceCheck.position + transform.forward, sphereCastRadius);
		Gizmos.DrawSphere(RCP_RighttVaultSpaceCheck.position + transform.forward, sphereCastRadius);

		Gizmos.DrawSphere(RCP_LeftVaultSpaceCheck.position + transform.forward + (Vector3.up * .5f), sphereCastRadius);
		Gizmos.DrawSphere(RCP_RighttVaultSpaceCheck.position + transform.forward + (Vector3.up * .5f), sphereCastRadius);

		Gizmos.DrawSphere(RCP_LeftVaultSpaceCheck.position + transform.forward + (Vector3.up * .25f), sphereCastRadius);
		Gizmos.DrawSphere(RCP_RighttVaultSpaceCheck.position + transform.forward + (Vector3.up * .25f), sphereCastRadius);
		//-----------------------------------
		Gizmos.DrawRay(RCP_ForwardCollisionCheck1.position, transform.forward * 1);
		Gizmos.DrawRay(RCP_ForwardCollisionCheck2.position, transform.forward * 1);
		Gizmos.DrawRay(RCP_ForwardCollisionCheck3.position, transform.forward * 1);
		Gizmos.DrawRay(RCP_ForwardCollisionCheck4.position, transform.forward * 1);
	}

}
