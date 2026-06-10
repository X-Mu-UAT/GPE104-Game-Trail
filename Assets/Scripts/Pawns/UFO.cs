using UnityEngine;

/// <summary>
/// UFO variation class that inherits from your existing Enemy base file.
/// Tracks and steers toward the player using vector math and emits a 3D spatial humming sound.
/// </summary>
[RequireComponent(typeof(AudioSource))] // Automatically adds an AudioSource to the UFO prefab if missing
public class UFO : Enemy
{
    [Header("UFO Tracking Configuration")]
    [Tooltip("The tag used to find the player ship in the scene.")]
    [SerializeField] private string playerTag = "Player";

    [Header("Doppler Audio Settings")]
    [Tooltip("The looping hum sound effect clip for this UFO.")]
    [SerializeField] private AudioClip hummingTrackClip;

    private Transform playerTransform;
    private AudioSource spatialAudioSource;

    protected override void Start()
    {
        // 1. MUST EXECUTE: Runs your existing Enemy.cs setup (sets max health, shows sliders, registers with GM)
        base.Start();

        // 2. Set up the specialized 3D sound component
        ConfigureSpatialAudio();

        // 3. Find where the player is in the map
        LocatePlayerShip();
    }

    protected override void Update()
    {
        // If the player died and respawned, look for them again
        if (playerTransform == null)
        {
            LocatePlayerShip();
        }

        // Requirement: Slowly move to seek the player using vector math
        ExecuteSeekMovement();

        // MUST EXECUTE: Runs your existing Enemy.cs loop (keeps health bar straight, handles screen warp maps)
        base.Update();
    }

    /// <summary>
    /// Calculates the vector math direction to the player and moves the UFO towards them.
    /// </summary>
    private void ExecuteSeekMovement()
    {
        if (playerTransform == null) return;

        // Vector Math: Target Position minus Current Position gets the direction vector
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Move the UFO along that vector using the 'speed' variable inherited from Enemy.cs
        transform.position += directionToPlayer * speed * Time.deltaTime;
    }

    /// <summary>
    /// Finds the player ship in the scene using its tag.
    /// </summary>
    private void LocatePlayerShip()
    {
        GameObject playerObj = GameObject.FindWithTag(playerTag);
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    /// <summary>
    /// Configures the attached AudioSource to handle 3D distance calculations and the Doppler effect.
    /// </summary>
    private void ConfigureSpatialAudio()
    {
        spatialAudioSource = GetComponent<AudioSource>();
        spatialAudioSource.clip = hummingTrackClip;
        spatialAudioSource.loop = true;

        // Requirement: Change sound from flat 2D to localized 3D spatial panning
        spatialAudioSource.spatialBlend = 1.0f;

        // Requirement: Tell Unity to use its native Doppler simulation calculations when moving
        spatialAudioSource.dopplerLevel = 1.0f;

        // Set reasonable distances for the sound drop-off (tweak these to fit your screen scale)
        spatialAudioSource.minDistance = 1.0f;
        spatialAudioSource.maxDistance = 15.0f;
        spatialAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        // Play the hum immediately
        if (hummingTrackClip != null)
        {
            spatialAudioSource.Play();
        }
    }
}