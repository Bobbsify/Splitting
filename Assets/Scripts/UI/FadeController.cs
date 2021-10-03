using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private SpriteRenderer componentRenderer;

    private float fadeMod = 1;
    private bool doFade = false;
    private float fadeAmount = 10;

    Color currColor;

    private void Awake()
    {
        TryGetComponent(out componentRenderer);
        currColor = componentRenderer.color;
    }

    private void Update()
    {
        if (doFade)
        {
            currColor.a += fadeAmount * fadeMod * Time.deltaTime;
            componentRenderer.color = currColor;
            if (fadeMod > 0 ? currColor.a >= 255 : currColor.a <= 0)
            {
                currColor.a = fadeMod > 0 ? 255 : 0;
                doFade = false;
            }
        }
    }

    public void fadeIn()
    {
        fadeMod = 1;
        doFade = true;
        Debug.Log("DoFadeIn");
    }

    public void fadeOut()
    {
        fadeMod = -1;
        doFade = true;
        Debug.Log("DoFadeOut");
    }
}
