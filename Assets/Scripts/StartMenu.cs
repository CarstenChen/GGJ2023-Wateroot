using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class StartMenu : MonoBehaviour
{
    public UIFadeAnimation startPanelFadeAnimation;
    public UIFadeAnimation levelPanelFadeAnimation;
    private Button[] startPanelButtons;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        startPanelButtons = startPanelFadeAnimation.GetComponentsInChildren<Button>();
        if(startPanelFadeAnimation!=null)
        startPanelFadeAnimation.PanelFadeIn();
        AddEventListener(startPanelButtons[0], levelPanelFadeAnimation.PanelFadeIn);


    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !levelPanelFadeAnimation.gameObject.activeSelf)
        {
            QuitGame();
        }
    }

    public void AddEventListener(Button button, UnityAction function)
    {
        if(button == null)
        {
            Debug.Log("°´Å¥" + button.name + "¶ªÊ§½Å±¾");
            return;
        }

        button.onClick.AddListener(function);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
