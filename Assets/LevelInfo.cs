using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelInfo : MonoBehaviour
{
    public static LevelInfo instance { get; private set; }

    [SerializeField]protected  TextMeshProUGUI levelName;
    [SerializeField] protected TextMeshProUGUI sceneName;
    [SerializeField] protected string[] levelNames;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
            instance = this;


    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
            this.gameObject.SetActive(false);
    }
    public void UpdateLevelInfoUI(int levelID)
    {
        levelName.text = string.Format("Chapter {0}", levelID);
        sceneName.text = levelNames[levelID - 1];
    }

    public void BlockInput()
    {
        PlayerPrefs.SetInt("BlockInput",0);
    }

    public void PermitInput()
    {
        PlayerPrefs.SetInt("BlockInput", 1);
    }
}
