using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerBaseState
{
	public float timeSpentInState;
	public override void onStateEnter()
	{
		timeSpentInState = 0;
		setAnimatorToFalse();
		_player._animator.SetBool("IdleState", true);
		_currentStateName = currentStateName.Idle;
	}

	public override void onStateExit()
	{
		
	}

	public override void onStateUpdate()
	{
		applyGravity();
		maintainDistanceFromGround();
		if(_player.movementDirection.magnitude > 0)
		{
			_player.transitionToState(_player.walkState);

		}
		if(_player._jumpButton /*&& _player.ClimbCheck1*/)
		{
			_player.transitionToState(_player.hangState);
		}
/*		if (_player._jumpButton)
		{
			_player.transitionToState(_player.jumpState);
		}
*/
		timeSpentInState += Time.deltaTime;
	}

	

}
