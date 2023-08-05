using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
	public float xAxisSpeed;
	public float yAxisSpeed;
	public float zAxisSpeed;


	public Vector2 xLimits = new Vector2(-1,1);
	public Vector2 yLimits = new Vector2(-1, 1);
	public Vector2 zLimits = new Vector2(-1, 1);


	void Update()
	{
		transform.Translate(xAxisSpeed * Time.deltaTime, yAxisSpeed * Time.deltaTime, zAxisSpeed * Time.deltaTime, Space.Self);

		if (xAxisSpeed != 0)
		{
			if (transform.localPosition.x < xLimits.x) { transform.localPosition = new Vector3(xLimits.y, transform.localPosition.y, transform.localPosition.z); }
			if (transform.localPosition.x > xLimits.y) { transform.localPosition = new Vector3(xLimits.x, transform.localPosition.y, transform.localPosition.z); }
		}

		if (yAxisSpeed != 0)
		{
			if (transform.localPosition.y < yLimits.x) { transform.localPosition = new Vector3(transform.localPosition.y, yLimits.y, transform.localPosition.z); }
			if (transform.localPosition.y > yLimits.y) { transform.localPosition = new Vector3(transform.localPosition.y, yLimits.x, transform.localPosition.z); }
		}
	}
}
