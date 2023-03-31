using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action<Payload> onPayloadDestroyEvent;
    public event Action<Shape, bool> onPointsChangeEvent;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void DestroyPayload(Payload payload, Shape receiverType)
    {
        onPointsChangeEvent?.Invoke(payload.shape, payload.shape == receiverType);
        onPayloadDestroyEvent?.Invoke(payload);
    }
}
