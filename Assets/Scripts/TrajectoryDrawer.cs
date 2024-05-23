using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CarTrajectoryTracker : MonoBehaviour
{
    public Transform carTransform;  // ������, ���������� �������� ����� �����������
    public float updateInterval = 0.1f;  // �������� ������� ����� ������������ ����������

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
            // ���������� ������
            currentTime = 0f;

            // ��������� ������� ������� ������� � ������
            positions.Add(carTransform.position);

            // ��������� LineRenderer
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        List<Vector3> elevatedPositions = new List<Vector3>();

        // �������� �������� �� Y �� ���� ������ ����������
        foreach (Vector3 originalPosition in positions)
        {
            elevatedPositions.Add(new Vector3(originalPosition.x, originalPosition.y + 2.0f, originalPosition.z)); // �������� +1 �� ��� Y
        }

        lineRenderer.positionCount = elevatedPositions.Count;
        lineRenderer.SetPositions(elevatedPositions.ToArray());
    }
}