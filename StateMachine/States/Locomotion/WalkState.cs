using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerBaseState
{

	public override void onStateEnter()
	{
		_currentStateName = currentStateName.Hang;
		AssignPlayer(_player);
		setAnimatorToFalse();
		_player._animator.SetBool("WalkState", true);

	}

	public override void onStateExit()
	{

	}

	public override void onStateUpdate()
	{
		//applyGravity();
		if (!_player.isGrounded)
		{
			applyGravity();
		}
		else
		{
			_player.controller.Move((_player.movementDirection) * _player._movementSpeed * Time.deltaTime);
			maintainDistanceFromGround();

		}

		if (_player.movementDirection.magnitude == 0)
		{
			_player.transitionToState(_player.idleState);
		}
		
	}
}
