using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadCollector : MonoBehaviour
{
    public Shape shape;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale , 1);
    }
    private void OnTriggerExit2D(Collider2D other) {
        Payload payload  = other.gameObject.GetComponent<Payload>();
        if(payload != null)
        {
            EventManager.instance.DestroyPayload(payload, shape);
        }
    }
    public void SetSpriteColor(Color c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
}
