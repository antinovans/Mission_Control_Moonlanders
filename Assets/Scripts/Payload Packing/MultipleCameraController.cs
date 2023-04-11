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
    // [SerializeField]
    private int _dictSize = 0;
    public float minZoom = 40f;
    public float maxZoom = 2f;
    // public float zoomLimiter = 50f;
    private Vector3 _velocity;
    private Camera _cam;
    private Bounds _bound;
    private void Awake() {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        _centerPoint = Vector3.zero;
        _targets = new Dictionary<int, Transform>();
        _cam = GetComponent<Camera>();
    }
    private void Start() {
        minZoom = PaddleManager.instance.playerNum * 1.8f; 
    }
    private void LateUpdate() {
        if(_targets.Count == 0)
            return;
        UpdateCameraPosition();
        UpdateCameraFOV();
    }



    private void UpdateCameraPosition()
    {
        GetCenterPoint();
        Vector3 newPosition = _centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, smoothTime);
    }
    private void UpdateCameraFOV()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance());
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, newZoom, Time.deltaTime);
    }

    float GetGreatestDistance()
    {
        return _bound.size.x;
    }

    private void GetCenterPoint()
    {
        if(_targets.Count == 0)
            return;
        if(_targets.Count == 1)
            _centerPoint = _targets[0].position;
        _bound = new Bounds();
        foreach (var item in _targets)
        {
            _bound.Encapsulate(item.Value.position);
        }
        _centerPoint = _bound.center;
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
    private void OnDrawGizmos() {
        if(Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(_bound.min, Vector3.one);
            Gizmos.DrawCube(_bound.max, Vector3.one);
            Gizmos.color = Color.yellow;
            foreach (var item in _targets)
            {
                Gizmos.DrawSphere(item.Value.position, 1);
            }
        }
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
