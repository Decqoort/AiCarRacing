using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float startTimerDelay = 5;
    public float gameTime = 0;
    [SerializeField] public TMP_Text timeTextStart;
    [SerializeField] public TMP_Text timeTextGame;
    private bool isRunning = false;

    private void Start()
    {
        if (startTimerDelay > 0) timeTextStart.text = startTimerDelay.ToString();
        float time = Mathf.Round(gameTime);
        string minutes = ((int)time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        timeTextGame.text = minutes + ":" + seconds;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        startTimerDelay -= Time.deltaTime;
        if (startTimerDelay > 0) timeTextStart.text = Mathf.Round(startTimerDelay).ToString();
        if (startTimerDelay <= 0)
        {
            timeTextStart.text = "START !!!";
            if (startTimerDelay < -1) timeTextStart.enabled = false;

            gameTime += Time.deltaTime;
            float time = Mathf.Round(gameTime);
            string minutes = ((int)time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");
            timeTextGame.text = minutes + ":" + seconds;
        }
    }

    public void RunTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
