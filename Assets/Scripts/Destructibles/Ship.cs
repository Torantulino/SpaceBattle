using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for all Units that can move.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ship : Unit {

    [SyncVar(hook = "OnParticlesChanged")]
    private bool _particles;
    private float _particlesTime;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        _particles = false;
        _particlesTime = 0f;
    }

    // Update is called once per frame
	void Update ()
    {	
        if(!isLocalPlayer)
            return;
        
        // Updating particles for engines
        if(_particlesTime > 0f)
            _particlesTime -= Time.deltaTime;
        else
            _particles = false;
	}

    /// <summary>
    /// Tries to use engines of the Ship.
    /// </summary>
    public void Thrust(Vector3 direction)
    {
        body.AddRelativeForce(power * direction.normalized, ForceMode.Force);

        _particlesTime = .2f;
        _particles = true;
    }

    /// <summary>
    /// Rotates the Ship on the Z axis.
    /// Prevents Ship from rotating too fast.
    /// </summary>
    public void Roll(float degrees)
    {
        // Clamping degrees so Ship doesn't rotate too fast
        degrees = Mathf.Clamp(degrees, -180f, 180f);
        transform.Rotate(new Vector3(0f, 0f, degrees) * Time.fixedDeltaTime);
        //todo upgrade Roll
        //Quaternion rotation = transform.rotation * Quaternion.Euler(0f, 0f, degrees);
        //body.MoveRotation(rotation);
    }

    /// <summary>
    /// Called when particles value was synchronized.
    /// </summary>
    void OnParticlesChanged(bool particles)
    {
        engines.ForEach(e => e.Particles.GetComponent<ParticleSystem>().EnableEmission(particles));
    }

}
