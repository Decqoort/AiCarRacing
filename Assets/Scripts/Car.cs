using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Car : MonoBehaviour
{
    public string nameCar;
    public int currentLap;
    private TMP_Text textName;
    [SerializeField] private GameManager gameManager;
    private CarController controller;

    private void Awake()
    {
        textName = GetComponentInChildren<TMP_Text>();

        controller = GetComponent<CarController>();

        if (textName != null)
        {
            textName.text = nameCar;
        }
    }

    private void Update()
    {
        controller.isRun = gameManager.isRunning;
    }
}
