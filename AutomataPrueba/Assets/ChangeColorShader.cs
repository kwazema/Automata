using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorShader : MonoBehaviour
{
    private Material material;
    private Coroutine coroutine;


    private float timer = 0;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    public void StartLerpColor(float timeLerp, Color color)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        
        coroutine = StartCoroutine(IE_LerpColor(timeLerp, color));
    }

    private IEnumerator IE_LerpColor(float timeLerp, Color color)
    {
        Color initColor = material.GetColor("_Color");
        timer = 0;

        while (timer <= timeLerp)
        {
            material.SetColor("_Color", Color.Lerp(initColor, color, timer / timeLerp));
            timer += Time.deltaTime;

            yield return null;
        }

    }
}
