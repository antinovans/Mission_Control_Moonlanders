using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(BoxCollider2D))]
public class Paddle : MonoBehaviour
{
    private KeyCode leftkey;
    private KeyCode rightkey;
    private float rotationSpeed = 2000f; // The speed at which the paddle rotates

    private Rigidbody2D _rb;
    // private int _id;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        // _id = MultipleCameraController.instance.GetDictSize();
        // MultipleCameraController.instance.AddTransformToList(transform, _id);
    }

    private void FixedUpdate() {
        RotatePaddle();    
    }
    

    private void RotatePaddle()
    {
        float rotationDirection = 0;

        // Check for left or right arrow key input
        if (Input.GetKey(leftkey))
        {
            // Debug.Log("left key pressed");
            rotationDirection = 1;
        }
        else if (Input.GetKey(rightkey))
        {
            rotationDirection = -1;
        }

        // Rotate the paddle using AddTorque
        _rb.AddTorque(_rb.mass * rotationDirection * rotationSpeed * Time.deltaTime, ForceMode2D.Force);
        /*transform.Rotate(0, 0, rotationDirection * rotationSpeed * Time.deltaTime);*/
    }
    public void SetLeftRotateKey(KeyCode k)
    {
        leftkey = k;
    }
    public void SetRightRotateKey(KeyCode k)
    {
        rightkey = k;
    }
    public void SetAngularDrag(float drag)
    {
        _rb.angularDrag = drag;
    }
    public void SetMass(float mass)
    {
        _rb.mass = mass;
    }
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    // public void SetPosition(Vector2 position)
    // {
    //     transform.position = position;
    // }
}
