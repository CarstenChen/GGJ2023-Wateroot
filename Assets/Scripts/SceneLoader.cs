using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance { get; private set; }
    public Animator sceneFadeAnimator;
    public Animator levelInfoFadeAnimator;
    private void Awake()
    {
        if(instance ==null)
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(GameObject.Find("SceneLoaderCanvas"));
        GameObject.DontDestroyOnLoad(GameObject.Find("EventSystem"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LoadScene()
    {
        sceneFadeAnimator.ResetTrigger("FadeIn");
        levelInfoFadeAnimator.ResetTrigger("FadeIn");

        if (sceneFadeAnimator != null)
        {
            sceneFadeAnimator.SetTrigger("FadeIn");
        }
        if (levelInfoFadeAnimator != null)
        {
            LevelInfo.instance.UpdateLevelInfoUI(GameManager.Instance.nextSceneID);
            levelInfoFadeAnimator.gameObject.SetActive(true);
            levelInfoFadeAnimator.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(1f);

        Debug.Log("You Win!");

        if (PlayerPrefs.HasKey("HighestLevel"))
        {
            if (PlayerPrefs.GetInt("HighestLevel") < GameManager.Instance.nextSceneID)
                PlayerPrefs.SetInt("HighestLevel", GameManager.Instance.nextSceneID);
        }
        else
        {
            PlayerPrefs.SetInt("HighestLevel", GameManager.Instance.nextSceneID);
        }

        

        AsyncOperation async = SceneManager.LoadSceneAsync(string.Format("Level{0}", GameManager.Instance.nextSceneID));
        async.completed += OnLoadedScene;
    }

    public void LoadFirstScene(int levelID)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(string.Format("Level{0}", levelID));
        async.completed += OnLoadedScene;
    }


    public void OnLoadedScene(AsyncOperation obj)
    {
        sceneFadeAnimator.ResetTrigger("FadeOut");
        levelInfoFadeAnimator.ResetTrigger("FadeOut");

        if (sceneFadeAnimator != null)
        {
            sceneFadeAnimator.SetTrigger("FadeOut");
        }
        if (levelInfoFadeAnimator != null)
        {
            LevelInfo.instance.UpdateLevelInfoUI(GameManager.Instance.nextSceneID-1);
            levelInfoFadeAnimator.gameObject.SetActive(true);
            levelInfoFadeAnimator.SetTrigger("FadeOut");
        }
        
    }

    public IEnumerator QuitLevel()
    {
        sceneFadeAnimator.ResetTrigger("FadeIn");

        if (sceneFadeAnimator != null)
        {
            sceneFadeAnimator.SetTrigger("FadeIn");
            //sceneFadeAnimator.SetBool("FadeOut", false);
        }


        yield return new WaitForSeconds(1f);

        AsyncOperation async = SceneManager.LoadSceneAsync(string.Format("StartScene"));
        async.completed += OnLoadedScene;
    }
}
