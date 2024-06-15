using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct FinishState
{
    public float time;
    public string name;
}

public class GameManager : MonoBehaviour
{
    public static int counLaps;
    public static int countBots;
    private FinishState[] carsFinishList;
    public bool isRunning = false;
    public bool isStart = false;
    private Timer timer;

    [SerializeField] private GameObject carsList;
    private List<Transform> carsTransform;

    private void Awake()
    {
        timer = GetComponent<Timer>();
    }

    private void Start()
    {
        LoadScene();
        InitBots();
        InitPlayer();
        timer.RunTimer();
        isStart = true;
    }

    public int getLaps()
    {
        return counLaps;
    }

    private void LoadScene()
    {
        Transform carTransform_ = GetComponentInParent<Transform>().Find("Cars");

        carsTransform = new List<Transform>();

        foreach (Transform carTransform in carsList.transform)
        {
            carsTransform.Add(carTransform);
            carTransform.gameObject.SetActive(false);
        }
    }

    private void InitPlayer()
    {
        carsTransform[0].gameObject.SetActive(true);
    }

    private void InitBots()
    {
        for (int index = 1; index <= countBots; index++)
        {
            carsTransform[index].gameObject.SetActive(true);
        }
    }

    private void RunGame()
    {
        isRunning = true;
        timer.RunTimer();
    }

    private void StopGame()
    {
        isRunning = false;
        timer.StopTimer();
    }

    private void Update()
    {
        if (isStart && timer.gameTime > 0)
        {
            isRunning = true;
            isStart = false;
        }

        // Pause (Esc)
        if (Input.GetKey(KeyCode.Escape))
        {
            /*if (isRunning)
                StopGame();
            else
                RunGame();*/
            SceneManager.LoadScene(0);
        }

    }
}
