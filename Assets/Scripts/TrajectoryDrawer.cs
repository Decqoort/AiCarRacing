using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CarTrajectoryTracker : MonoBehaviour
{
    public Transform carTransform;  // Объект, траекторию которого нужно отслеживать
    public float updateInterval = 0.1f;  // Интервал времени между обновлениями траектории

    private LineRenderer lineRenderer;
    private List<Vector3> positions;
    private float currentTime;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        positions = new List<Vector3>();
        currentTime = 0f;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= updateInterval)
        {
            // Сбрасываем таймер
            currentTime = 0f;

            // Добавляем текущую позицию машинки в список
            positions.Add(carTransform.position);

            // Обновляем LineRenderer
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        List<Vector3> elevatedPositions = new List<Vector3>();

        // Добавить смещение по Y ко всем точкам траектории
        foreach (Vector3 originalPosition in positions)
        {
            elevatedPositions.Add(new Vector3(originalPosition.x, originalPosition.y + 2.0f, originalPosition.z)); // Смещение +1 по оси Y
        }

        lineRenderer.positionCount = elevatedPositions.Count;
        lineRenderer.SetPositions(elevatedPositions.ToArray());
    }
}