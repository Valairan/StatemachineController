using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState 
{
	public currentStateName _currentStateName;
	protected PlayerBehavior _player;
	protected Vector3 gravity = new Vector3();
	public PlayerBehavior Player { get { return _player; } }
	public virtual void AssignPlayer(PlayerBehavior playerController)
	{
		_player = playerController;
	}
	public abstract void onStateEnter();
	public abstract void onStateUpdate();
	public abstract void onStateExit();

	public void setAnimatorToFalse()
	{
		foreach (AnimatorControllerParameter parameter in _player._animator.parameters)
		{
			if (parameter.type == AnimatorControllerParameterType.Bool)
				_player._animator.SetBool(parameter.name, false);
		}
	}

	public void applyGravity()
	{
		if (!_player._isGrounded && _player.RCH_groundCheck.distance > .5f)
		{ 

			gravity.y += _player.g * Time.deltaTime;
			_player.controller.Move(_player.controller.velocity + (gravity * Time.deltaTime));
		}
		if (_player._isGrounded)
		{
			gravity = Vector3.zero;
		}
	}

	public void maintainDistanceFromGround()
	{
		if (_player._isGrounded && _player.RCH_groundCheck.distance < .48f)
		{
			_player.controller.Move(Vector3.up * Time.deltaTime * 2);
		}
	}
}

public enum currentStateName
{
	Idle,
	Walk,
	Run,
	Hang,
	Jump
}