using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{

    private SpriteRenderer cloudRenderer;

    public float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cloudRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(0, y);
        cloudRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}

