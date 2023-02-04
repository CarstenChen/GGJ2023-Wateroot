using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Obi;
using UnityEngine.SceneManagement;
using System;

public enum SoundMode
{
    ShortLoop,CutMusic,BGM
}
public class GameManager : MonoBehaviour
{
    protected static GameManager instance;
    public static GameManager Instance { get { return instance; } private set { } }

    [Header("Level Settings")]
    public string sceneName;
    public int levelNums = 4;
    public ObiCollider finishLine;
    public int nextSceneID;
    public Animator sceneFadeAnimator;
    public Animator levelInfoFadeAnimator;


    [Header("Fluid Settings")]
    public ObiSolver solver;
    public ObiEmitter emitter;

    [Header("Ambient Sound Settings")]
    public AudioClip ambientSound;
    public AudioSource audio;
    public float minVolume;
    public float maxVolume;
    public SoundMode soundMode;
    public float lasting;
    public Vector2 interval;

    [Header("Drip Sound Settings")]
    public AudioClip levelPassDripSound;
    public static float levelPassVolume=0.4f;
    public AudioSource dripAudio;

    protected bool levelPased;
    protected float autoQuitCount;
    protected float autoQuiteTime = 3000f;

    protected float tick = 2f;
    protected float soundStopTime=0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        solver.OnCollision += Solver_OnCollision;
        emitter.OnEmitParticle += Emitter_OnEmitParticle;

        //GameObject.DontDestroyOnLoad(GameObject.Find("Canvas"));
        //GameObject.DontDestroyOnLoad(GameObject.Find("EventSystem"));

        if (soundMode == SoundMode.BGM)
        {
            PlayCutBGM();
        }
    }

    void Emitter_OnEmitParticle(ObiEmitter em, int particleIndex)
    {
        int k = emitter.solverIndices[particleIndex];
        solver.userData[k] = solver.colors[k];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(SceneLoader.instance.QuitLevel());
        }

        PreventFluidDisappear();

        tick -= Time.deltaTime;
        if (tick <= 0)
        {
            if (soundMode == SoundMode.ShortLoop)
                PlayShortLoopSound();
            else if (soundMode == SoundMode.CutMusic)
                PlayCutMusic();
            tick = UnityEngine.Random.Range(interval.x, interval.y);

        }
       

    }

    void PreventFluidDisappear()
    {
        autoQuitCount += Time.deltaTime;

        if (autoQuitCount >= autoQuiteTime)
        {
            StartCoroutine(SceneLoader.instance.QuitLevel());
        }
    }

    private void Solver_OnCollision(ObiSolver s, ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();
        foreach (Oni.Contact contact in e.contacts)
        {
            // look for actual contacts only:
            if (contact.distance < 0.01f)
            {
                var col = world.colliderHandles[contact.bodyB].owner;

                if (finishLine == col)
                {
                    if (!levelPased)
                    {
                        if (nextSceneID > levelNums)
                        {
                            StartCoroutine(SceneLoader.instance.QuitLevel());
                        }
                        else
                        {
                            StartCoroutine(SceneLoader.instance.LoadScene());
                            PlayLevelPassSound();
                            levelPased = true;
                        }

                    }

                }
            }
        }
    }

    //IEnumerator LoadScene()
    //{
    //    if (sceneFadeAnimator != null)
    //    {
    //        sceneFadeAnimator.SetBool("FadeIn", true);
    //        sceneFadeAnimator.SetBool("FadeOut", false);
    //    }
    //    if (levelInfoFadeAnimator != null)
    //    {
    //        levelInfoFadeAnimator.SetBool("FadeIn", true);
    //        levelInfoFadeAnimator.SetBool("FadeOut", false);
    //    }

    //        yield return new WaitForSeconds(1f);

    //    Debug.Log("You Win!");
    //    PlayerPrefs.SetInt("HighestLevel", nextSceneID);
    //    AsyncOperation async = SceneManager.LoadSceneAsync(string.Format("Level{0}", nextSceneID));
    //    async.completed += OnLoadedScene;
    //}


    //    private void OnLoadedScene(AsyncOperation obj)
    //{
    //    if (sceneFadeAnimator != null)
    //    {
    //        sceneFadeAnimator.SetBool("FadeIn", false);
    //        sceneFadeAnimator.SetBool("FadeOut", true);

            
    //    }

    //    StartCoroutine(LevelInfoFadeOut());
    //}

    //IEnumerator LevelInfoFadeOut()
    //{
    //    yield return new WaitForSeconds(3);
    //    if (levelInfoFadeAnimator != null)
    //    {
    //        levelInfoFadeAnimator.SetBool("FadeIn", false);
    //        levelInfoFadeAnimator.SetBool("FadeOut", true);
    //    }
    //}

    //IEnumerator QuitLevel()
    //{
    //    if (sceneFadeAnimator != null)
    //    {
    //        sceneFadeAnimator.SetBool("FadeIn", true);
    //        sceneFadeAnimator.SetBool("FadeOut", false);
    //    }


    //    yield return new WaitForSeconds(1f);

    //    AsyncOperation async = SceneManager.LoadSceneAsync(string.Format("StartScene"));
    //    async.completed += OnLoadedScene;
    //}

    void PlayShortLoopSound()
    {
        audio.clip = ambientSound;
        //audio.volume = UnityEngine.Random.Range(minVolume, maxVolume);
        audio.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audio.Play();
        StartCoroutine(ShortLoopSoundFade(0, UnityEngine.Random.Range(minVolume, maxVolume), 0.01f, 0.002f, lasting));             
    }

    void PlayCutMusic()
    {
        //audio.volume = UnityEngine.Random.Range(minVolume, maxVolume);
        audio.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audio.time = soundStopTime;
        audio.Play();
        StartCoroutine(ShortLoopSoundFade(0, UnityEngine.Random.Range(minVolume, maxVolume), 0.01f, 0.002f, lasting));
    }
    void PlayCutBGM()
    {
        audio.volume = UnityEngine.Random.Range(minVolume, maxVolume);
        audio.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audio.time = 0;
        audio.Play();
        audio.loop = true;
    }
    IEnumerator ShortLoopSoundFade(float min, float max,float fadeInSpeed, float fadeOutSpeed, float interval)
    {
        for (float i = min; i < max; i += fadeInSpeed)
        {
            audio.volume = i;
            yield return null;
        }

        yield return new WaitForSeconds(interval);
        for (float i = max; i > min; i -= fadeOutSpeed)
        {
            audio.volume = i;
            yield return null;
        }

        soundStopTime = audio.time;
    }

    void PlayLevelPassSound()
    {
        dripAudio.clip = levelPassDripSound;
        dripAudio.volume = levelPassVolume;
        dripAudio.loop = false;
        dripAudio.Play();
    }
}
