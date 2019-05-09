using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Part {

	[SerializeField]
	private float _power;

	//todo change to ParticleSystem if GameObject isn't needed 
	public GameObject Particles;

	/// <summary>
	/// Power of this engine.
	/// </summary>
	public float Power
	{
		get { return _power; }
	}

	// Use this for initialization
	new void Start () {
		base.Start();

	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
	}
}
