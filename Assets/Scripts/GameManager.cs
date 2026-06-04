using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State UI Objects (Requirement 1 & 3)")]
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;
    public GameObject VictoryScreenStateObject;

    [Header("Score System (Requirement 2)")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int currentScore = 0;

    [Header("Audio Configuration (Exposed to Designers)")]
    public AudioClip backgroundMusicClip;
    public AudioClip playerShootClip;
    public AudioClip targetTakeDamageClip;

    private AudioSource musicAudioSource;

    private List<Health> activeObstacles = new List<Health>();
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupBackgroundMusic(); // ADDED: Configures background audio automatically

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Launches straight into the Main Menu layout inside your scene
        SetGameState("MainMenu");
    }

    // ADDED: Builds a loop audio channel for background tracking
    private void SetupBackgroundMusic()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.playOnAwake = true;
        musicAudioSource.volume = 0.4f; // Safe background level
       
        // Tells it to bypass standard pause loops if needed
        musicAudioSource.ignoreListenerPause = true;
       
        if (backgroundMusicClip != null)
        {
            musicAudioSource.Play();
        }
    }

    // ADDED: Universal helper so any weapon or damage zone can trigger a clip instantly
    public void PlaySoundEffect(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);