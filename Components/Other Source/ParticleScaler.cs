//from http://prof.johnpile.com/2015/11/02/unity-scaling-particles/

/*
There are some significant caveats about this including:
1. It requires that you set your initial speed in this script (not the normal location within the particle system parameters).
2. You must use “local”, not “world” particles (that is, relative to the game object).
3. The game object’s scale is a Vector3 but the particle size is a scalar (float). As a result, this code uses an average of the 3 coordinates to choose the scaling factor. Based on some observations with the way the particle system works on this particular sample particle, it may be worth just using the x and z coordinates.
4. This does not scale other behaviors that you may want scaled. If you find something doesn’t look correct in your particular use-case, just follow the same idea as listed below to make further modifications.
5. This traverses the list of particles every frame .. you may want to do something a little smarter.. although I’m not sure you have a lot of other options. :)
6. Lastly, and most importantly… from an artistic perspective, do you REALLY want to do this? 
That is, an individual particle may look great when it is small. But if you increase the scale it is 
likely that the individual particles will look pixelated. More often, you’re better off increasing the 
number of particles for a bigger effect. But admittedly, there are rare occasions where the code below 
does exactly what the artist wanted… bigger individual particles.
 */

using UnityEngine;
using System.Collections;

#pragma warning disable CS0618

[RequireComponent(typeof(ParticleSystem))]
[ExecuteInEditMode()]
public class ParticleScaler : MonoBehaviour
{

	ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	public float m_Size = 1.0f;
	public float m_StartSpeed = 1.0f;

	private void LateUpdate()
	{
		InitializeIfNeeded();

		// GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles(m_Particles);

		float currentScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3.0f;
		m_System.startSpeed = m_StartSpeed * currentScale;
		for(int i = 0; i < numParticlesAlive; i++)
		{
			m_Particles[i].startSize = currentScale;
		}

		m_System.SetParticles(m_Particles, numParticlesAlive);

	}

	void InitializeIfNeeded()
	{

		if(m_System == null)
			m_System = GetComponent<ParticleSystem>();

		if(m_Particles == null || m_Particles.Length < m_System.maxParticles)
			m_Particles = new ParticleSystem.Particle[m_System.maxParticles];
	}

}