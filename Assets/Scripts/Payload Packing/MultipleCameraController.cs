using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleCameraController : MonoBehaviour
{
    public static MultipleCameraController instance;
    public Dictionary<int, Transform> _targets;
    private Vector3 _centerPoint;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    private int _dictSize = 0;
    // public float minZoom = 40f;
    // public float maxZoom = 10f;
    private Vector3 _velocity;
    private void Awake() {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        _centerPoint = Vector3.zero;
        _targets = new Dictionary<int, Transform>();
    }
    private void LateUpdate() {
        if(_targets.Count == 0)
            return;
        UpdateCameraPosition();
        // UpdateCameraFOV();
    }

    // private void UpdateCameraFOV()
    // {
    //     throw new NotImplementedException();
    // }

    private void UpdateCameraPosition()
    {
        GetCenterPoint();
        Vector3 newPosition = _centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, smoothTime);
    }

    private void GetCenterPoint()
    {
        if(_targets.Count == 0)
            return;
        if(_targets.Count == 1)
            _centerPoint = _targets[0].position;
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        foreach (var item in _targets)
        {
            bounds.Encapsulate(item.Value.position);
        }
        _centerPoint = bounds.center;
    }
    public void AddTransformToList(Transform t, int key)
    {
        _targets.Add(key, t);
        _dictSize++;
    }
    public int GetDictSize()
    {
        return _dictSize;
    }
    public void RemoveTransformFromDict(int key)
    {
        _targets.Remove(key);
    }
    // private float GetGreatestDistance()
    // {
    //     Bounds bounds = new Bounds(_targets[0].position, Vector3.zero);
    //     foreach (var item in _targets)
    //     {
    //         bounds.Encapsulate(item.position);
    //     }
    //     return bounds.size.x;
    // }
}
