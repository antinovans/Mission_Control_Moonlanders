using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void SetLengthAndHeightInWorldPosition(GameObject obj, float width, float height)
    {
        // NormalizeScale(obj);
        var bounds = obj.GetComponent<SpriteRenderer>().bounds;
        var currentWidth = bounds.max.x - bounds.min.x;
        var currentHeight = bounds.max.y - bounds.min.y;
        Vector3 newScale = new Vector3(obj.transform.localScale.x * width/currentWidth, 
                            obj.transform.localScale.y * height/currentHeight, 1);
        obj.transform.localScale = newScale;
    }
}
