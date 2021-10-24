using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerBaseState
{
	public override void onStateEnter()
	{
		AssignPlayer(_player);
		setAnimatorToFalse();
		_player._animator.SetBool("WalkState", true);
		_currentStateName = currentStateName.Run;
	}

	public override void onStateExit()
	{
		throw new System.NotImplementedException();
	}

	public override void onStateUpdate()
	{
		applyGravity();
		maintainDistanceFromGround();
		_player.controller.Move(_player.movementDirection * _player._movementSpeed * Time.deltaTime);



		if (_player.movementDirection.magnitude == 0)
		{
			_player.transitionToState(_player.idleState);
		}
	}
}
