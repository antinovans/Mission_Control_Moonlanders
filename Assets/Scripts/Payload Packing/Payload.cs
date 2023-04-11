using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Shape {
    Sphere,
    Rectangle,
    Triangle,
    Hexagon
}
[RequireComponent(typeof(Rigidbody2D))]
public class Payload : MonoBehaviour
{
    public Shape shape;
    private int _id;
    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(transform, _id);
        Utils.SetLengthAndHeightInWorldPosition(gameObject, 1.5f, 1.5f, false);
        EventManager.instance.onPayloadDestroyEvent += DestroyThis;
        // _rb.mass = _mass;
        // transform.localScale = new Vector3(transform.localScale.x * _size, transform.localScale.y * _size , 1);
        // Destroy(gameObject, 5.0f);
    }

    private void OnDestroy() {
        EventManager.instance.onPayloadDestroyEvent -= DestroyThis;
        MultipleCameraController.instance.RemoveTransformFromDict(_id);
    }
    // public void SetSpriteColor(Color c)
    // {
    //     GetComponent<SpriteRenderer>().color = c;
    // }
    public void SetMass(float mass)
    {
        _rb.mass = mass;
    }
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale , 1);
    }
    public void DestroyThis(Payload pl)
    {
        if(this == pl)
            Destroy(gameObject);
    }
}
