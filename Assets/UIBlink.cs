using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlink : MonoBehaviour
{
    public Image startButtonImg;
    public float minAlpha;
    public float maxAlpha;

    public bool fadeOut = true;
    // Start is called before the first frame update

    void Start()
    {
        startButtonImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            StartCoroutine(ColorFadeOut());
        }
        else
        {
            StartCoroutine(ColorFadeIn());
        }
    }

    IEnumerator ColorFadeIn()
    {
        startButtonImg.color = Color.Lerp(startButtonImg.color, new Color(255, 255, 255, maxAlpha), 0.01f);

        yield return new WaitForSeconds(1f);

        fadeOut = true;
    }

    IEnumerator ColorFadeOut()
    {
        startButtonImg.color =  Color.Lerp(startButtonImg.color, new Color(255, 255, 255, minAlpha), 0.01f);

        yield return new WaitForSeconds(1f);

        fadeOut = false;
    }
}
