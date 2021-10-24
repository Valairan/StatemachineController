using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerBaseState
{
	public override void onStateEnter()
	{
		AssignPlayer(_player);
		setAnimatorToFalse();
		_player._animator.SetBool("WalkState", true);
		_currentStateName = currentStateName.Jump;
		Debug.Log("Jump state");
	}

	public override void onStateExit()
	{
	}

	public override void onStateUpdate()
	{
		
	}
}
