using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Car : MonoBehaviour
{
    public string nameCar;
    public int currentLap;
    public bool isEndLap;
    private TMP_Text textName;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text lapText;

    //private TMP_Text lapText;
    private CarController controller;

    private void Awake()
    {
        textName = GetComponentInChildren<TMP_Text>();

        controller = GetComponent<CarController>();

        isEndLap = false;

        currentLap = 1;

        if (textName != null)
        {
            textName.text = nameCar;
        }

        if (lapText != null && gameManager != null)
        {
            lapText.text = "LAP : " + currentLap.ToString() + " / " + gameManager.getLaps().ToString();
        }
    }

    private void Update()
    {
        controller.isRun = gameManager.isRunning;

        if (lapText != null && gameManager != null)
        {
            lapText.text = "LAP : " + currentLap.ToString() + " / " + gameManager.getLaps().ToString();
        }
    }
}
