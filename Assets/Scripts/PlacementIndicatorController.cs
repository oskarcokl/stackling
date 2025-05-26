using System;
using UnityEngine;

public class PlacementIndicatorController : MonoBehaviour
{
    [SerializeField] private GameObject placementIndicator;

    private Vector3 _objectPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placementIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!placementIndicator.activeSelf) return;
        
        if (Physics.Raycast(_objectPosition, Vector3.down, out RaycastHit hit))
        {
            Vector3 indicatorPosition = hit.point + Vector3.up * 0.01f;
            placementIndicator.transform.position = indicatorPosition;
        }
    }

    public void ShowIndicator()
    {
        placementIndicator.SetActive(true);
    }

    public void HideIndicator()
    {
        placementIndicator.SetActive(false);
    }

    public void SetPoisition(Vector3 position)
    {
        _objectPosition = position;
    }
}
