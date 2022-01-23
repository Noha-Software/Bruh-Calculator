using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballistics : MonoBehaviour
{
	public float g = 9.81f;

    public Vector2 initialVelocity;
	public Vector2 startPosition;
    [Min(0)] public float time;
    public float length;
	public float range;
	[Range(0, 90)] public float angle;

	public void CalculateEverything()
	{
		print(CalculateHeight(startPosition, initialVelocity, time, g));
		print(CalculateLength(initialVelocity, time, g));
		print(CalculateRange(initialVelocity, angle, g));
		print(CalculateMaximumRange(initialVelocity, g));
	}

    public float CalculateHeight(Vector2 pos, Vector2 v, float t, float g = 9.81f)
	{
		g = -Mathf.Abs(g);
        return pos.y + v.y * t + (g / 2) * t * t;
	}
    public float CalculateLength(Vector2 v, float t, float g = 9.81f)
	{
		if (t == 0)
		{
			t = -(v.y + Mathf.Sqrt(v.y * v.y - 2 * g)) / g;
		}
        return v.x * t;
	}
    public float CalculateRange(Vector2 v, float angle, float g = 9.81f)
	{
		return v.x * v.x * Mathf.Sin(2 * angle) / g;
	}
    public float CalculateMaximumRange(Vector2 v, float g = 9.81f)
	{
		return v.x * v.x / g;
	}
}