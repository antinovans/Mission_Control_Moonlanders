using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider2D))]
public class Paddle : MonoBehaviour
{
    public KeyCode leftkey;
    public KeyCode rightkey;
    public float rotationSpeed = 2000f; // The speed at which the paddle rotates

    private Rigidbody2D rb;
    private int _id;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(transform, _id);
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
        // rb.AddTorque(rb.mass * rotationDirection * rotationSpeed * Time.deltaTime, ForceMode2D.Force);
        transform.Rotate(0, 0, rotationDirection * rotationSpeed * Time.deltaTime);
    }
    public void SetScale(int scale)
    {
        Vector3 newScale = new Vector3(transform.localScale.x * scale, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale;
    }
    public void SetSpriteColor(Color c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
    public void SetLeftRotateKey(KeyCode k)
    {
        leftkey = k;
    }
    public void SetRightRotateKey(KeyCode k)
    {
        rightkey = k;
    }
}
