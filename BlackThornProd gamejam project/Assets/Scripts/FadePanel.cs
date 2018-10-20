using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;



public class FadePanel : MonoBehaviour
{

    [SerializeField]
    private Image _fadePanel;
    public float fadeInTime = 0.5f;
    public float stayTime = 1f;
    public float fadeOutTime = 0.5f;



    private IEnumerator FadeInColor(Color color, float fadeTime)
    {

        float t = 0f;
        _fadePanel.color = color;
        _fadePanel.gameObject.SetActive(true);
        var currentColor = color;
        while (t <= fadeTime)
        {
            _fadePanel.color = Color.Lerp(currentColor, new Color(0, 0, 0, 0), t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }
        _fadePanel.color = new Color(0, 0, 0, 0);
        
        gameObject.SetActive(false);

    }

    private IEnumerator FadeOutColor(Color color, float fadeTime)
    {

        float t = 0f;
        _fadePanel.color = new Color(0, 0, 0, 0);
        var currentColor = new Color(0, 0, 0, 0);
        _fadePanel.gameObject.SetActive(true);
        while (t <= fadeTime)
        {
            _fadePanel.color = Color.Lerp(currentColor, color, t / fadeTime);
            t += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Delay(stayTime));
        
    }

    public void FadeInOutTransition()
    {

        StartCoroutine(FadeOutColor(Color.white, fadeOutTime));
                
    }

    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(FadeInColor(Color.white, fadeInTime));
    }


}
