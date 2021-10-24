using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class HangState : PlayerBaseState
{
	
	bool currentlyHanging;
	private float interpolationFactor;
	private float interpolationFactorFaster;
	private List<Vector3> points;
	LineRenderer lr;
	int speed = 0;

	public override void onStateEnter()
	{
		lr = _player.GetComponent<LineRenderer>();
		AssignPlayer(_player);
		setAnimatorToFalse();
		_player._animator.SetBool("WalkState", true);
		_currentStateName = currentStateName.Hang;

		Debug.Log("Entered Hang state");
		currentlyHanging = false;
		points = _player._curvePoints.ToList();
		points.RemoveAt(10);
		interpolationFactor = 1f;
		Debug.Log(points.Count + " " + currentlyHanging);


		foreach (Vector3 item in points)
		{
			Debug.Log(item);
		}
	}

	public override void onStateExit()
	{
	}

	public override void onStateUpdate()
	{
		lr.SetPositions(points.ToArray());
		if (currentlyHanging == false)
		{
			ListLerp();
		}
		if (_player._crouchButton)
		{
			_player.transitionToState(_player.idleState);
		}
	}

	void ListLerp()
	{
		if (points.Count > 0)
		{
			//Debug.Log("Point : " + points[0].x + " " + points[0].y + " " + points[0].z);
			//_player.controller.SimpleMove(points[0] * Time.deltaTime * speed);
			//_player.controller.Move((points[0] - _player.transform.position) * Time.deltaTime * speed);
			//_player.transform.position = Vector3.Lerp(_player.transform.position , points[0], Time.deltaTime * speed);
			//_player.controller.Move(Vector3.MoveTowards(_player.transform.position, points[0], Time.deltaTime * .5f) * Time.deltaTime * speed);
			_player.transform.localPosition = Vector3.MoveTowards(_player.transform.position, points[0], Time.deltaTime * .5f);

			//_player.transform.position = points[speed];

			if (Vector3.Distance(_player.transform.position, points[0]) > 0.01f)
			{
				points.RemoveAt(0);
				speed++;
			}
		}
		else
		{
			currentlyHanging = true;
		}
	}
}
