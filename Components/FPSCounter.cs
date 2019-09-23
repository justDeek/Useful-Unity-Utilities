using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	public Color _color = new Color32(1, 35, 56, 255);
	public Vector2 _position = new Vector2(17, 11);
	public Vector2 _size = new Vector2(100, 20);

	const float fpsMeasurePeriod = 0.5f;
	private int m_FpsAccumulator = 0;
	private float m_FpsNextPeriod = 0;
	private int m_CurrentFps;
	const string display = "{0} FPS";
	private string fps = "";

	void OnGUI()
	{
		GUI.color = _color;
		GUI.Label(new Rect(_position.x, _position.y, _size.x, _size.y), fps);
	}

	private void Start()
	{
		m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
	}


	void Update()
	{
		// measure average frames per second
		m_FpsAccumulator++;
		if(Time.realtimeSinceStartup > m_FpsNextPeriod)
		{
			m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
			m_FpsAccumulator = 0;
			m_FpsNextPeriod += fpsMeasurePeriod;
			fps = string.Format(display, m_CurrentFps);
		}
	}
}