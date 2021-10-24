using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Curves
{
    // Start is called before the first frame update
    public static Vector3[] calculateQuadraticCurve(Vector3 p1, Vector3 p2, Vector3 p3, int segments)
	{
		Vector3[] curvePoints = new Vector3[segments+1];
		for (int i = 0; i < segments+1; i++)
		{
			float t = (float) i /  (float) segments;
			curvePoints[i] = p2 + (1 - t) * (1 - t) * (p1 - p2) + t * t * (p3 - p2);
		}

		return curvePoints;
	}

	public static Vector3[] calculateCubicCurve(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, int segments)
	{
		Vector3[] curvePoints = new Vector3[segments + 1];
		for (int i = 0; i < segments + 1; i++)
		{
			float t = (float)i / (float)segments;
			curvePoints[i] = ((1 - t)*(1 - t) *(1 - t) * p1) + (3 * (1 - t) * (1 - t) * t * p2) + (3 * (1 - t) * t * t * p3) + (t * t * t * p4);
		}

		return curvePoints;
	}
}
