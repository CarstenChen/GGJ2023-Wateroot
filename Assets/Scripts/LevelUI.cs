using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
public class LevelUI : MonoBehaviour
{
    public int levelNums = 4;
    public GameObject levelButtonPrefab;
    public Button[] levelButtons;
    public Sprite[] sprites;
    public Sprite[] highlightSprites;
    public Sprite[] disableSprites;
    public string[] levelName;

    protected int currentSelectedIndex=0;

    // Start is called before the first frame update
    void Awake()
    {
        levelButtons = new Button[levelNums];
        for (int i = 1; i <= levelNums; i++)
        {
            int temp = i;

            GameObject go = Instantiate(levelButtonPrefab, this.transform);
            go.name = string.Format("Level{0}", i);

            Button btn = go.GetComponentInChildren<RectTransform>().GetComponentInChildren<Button>();
            levelButtons[i - 1] = btn;
            go.GetComponentInChildren<Text>().text = levelName[temp - 1];
            btn.GetComponent<Image>().sprite = sprites[temp - 1];
            btn.GetComponent<Image>().SetNativeSize();
            SpriteState state = btn.spriteState;
            state.highlightedSprite = highlightSprites[temp - 1];
            state.pressedSprite = highlightSprites[temp - 1];
            state.selectedSprite = highlightSprites[temp - 1];
            state.disabledSprite = disableSprites[temp - 1];
            btn.spriteState = state;

            if (i > PlayerPrefs.GetInt("HighestLevel"))
            {
                Debug.Log(PlayerPrefs.GetInt("HighestLevel"));
                if (i != 1)
                    levelButtons[i - 1].interactable = false;
            }

            AddEventListener(levelButtons[i - 1], () => { SceneLoader.instance.LoadFirstScene(temp);PlayerPrefs.SetInt("EnterLevelID", temp);/*SceneManager.LoadScene(string.Format("Level{0}",temp)); LevelInfo.instance.UpdateLevelInfoUI(temp);*/ });
            Debug.Log(string.Format("已添加事件{0}", i));
        }
    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("EnterLevelID")!=0)
        {
            levelButtons[PlayerPrefs.GetInt("EnterLevelID")-1].Select();
            currentSelectedIndex = PlayerPrefs.GetInt("EnterLevelID")- 1;
            StartCoroutine(SetSpriteNativeSize(levelButtons[currentSelectedIndex].GetComponent<Image>()));
        }
        else
        {
            levelButtons[0].Select();
            currentSelectedIndex = 0;
            StartCoroutine(SetSpriteNativeSize(levelButtons[currentSelectedIndex].GetComponent<Image>()));
        }
    }
    private void Update()
    {
        ButtonNavigation();
    }
    public void AddEventListener(Button button, UnityAction function)
    {
        if (button == null)
        {
            Debug.Log("按钮" + button.name + "丢失脚本");
            return;
        }

        button.onClick.AddListener(function);
    }

    public void ButtonNavigation()
    {
        if(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!PlayerPrefs.HasKey("HighestLevel")) return;

            levelButtons[currentSelectedIndex].GetComponent<Image>().sprite = sprites[currentSelectedIndex];
            StartCoroutine(SetSpriteNativeSize(levelButtons[currentSelectedIndex].GetComponent<Image>()));
            //levelButtons[currentSelectedIndex].GetComponent<Image>().SetNativeSize();
            currentSelectedIndex = currentSelectedIndex > 0 ? currentSelectedIndex-1 :PlayerPrefs.GetInt("HighestLevel")-1;

            levelButtons[currentSelectedIndex].Select();
            StartCoroutine(SetSpriteNativeSize(levelButtons[currentSelectedIndex].GetComponent<Image>()));
            //levelButtons[currentSelectedIndex].GetComponent<Image>().SetNativeSize();
        }
        else if (Input.GetKeyDown(KeyCode.D) ||Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!PlayerPrefs.HasKey("HighestLevel")) return;

            levelButtons[currentSelectedIndex].GetComponent<Image>().sprite = sprites[currentSelectedIndex];
            StartCoroutine(SetSpriteNativeSize(levelButtons[currentSelectedIndex].GetComponent<Image>()));
            //levelButtons[currentSelectedIndex].GetComponent<Image>().SetNativeSize();
            Debug.Log(PlayerPrefs.GetInt("HighestLevel") - 1);
            currentSelectedIndex = currentSelectedIndex < PlayerPrefs.GetInt("HighestLevel")-1 ? currentSelectedIndex+1 : 0;

            levelButtons[currentSelectedIndex].Select();
            StartCoroutine(SetSpriteNativeSize(levelButtons[currentSelectedIndex].GetComponent<Image>()));
            //levelButtons[currentSelectedIndex].GetComponent<Image>().SetNativeSize();
        }
    }

    IEnumerator SetSpriteNativeSize(Image img)
    {
        yield return null;
        img.SetNativeSize();
    }
}
