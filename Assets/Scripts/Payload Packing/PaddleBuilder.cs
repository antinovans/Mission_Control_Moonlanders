using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class PaddleBuilder : MonoBehaviour
{
    public static PaddleBuilder instance;
    [SerializeField]
    private Texture2D[] paddleTexture;
    [SerializeField]
    private GameObject parent_paddle;
    private List<Sprite> _sprites;
    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
        _sprites = new List<Sprite>();
        foreach (var item in paddleTexture)
        {
            _sprites.AddRange(Resources.LoadAll<Sprite>(item.name));
        }
    }

    //|---------|
    //l         r         
    public Paddle BuildPadle(int playerNum, Vector2 position, float width)
    {

        GameObject left = new GameObject("left");
        left.AddComponent<SpriteRenderer>().sprite = _sprites[((playerNum * 3) % _sprites.Count) + 0];
        left.GetComponent<SpriteRenderer>().sortingOrder = 1;
        left.AddComponent<PolygonCollider2D>().usedByComposite = true;
        // Utils.SetLengthAndHeightInWorldPosition(left, 1.0f, height);
        float leftLengthInWorld = (int)left.GetComponent<SpriteRenderer>().sprite.textureRect.width / (float)LevelBuilder.PIXEL_PER_UNIT;
        left.layer = 6;
        // Debug.Log(leftLengthInWorld);
        GameObject right = new GameObject("right");
        right.AddComponent<SpriteRenderer>().sprite = _sprites[((playerNum * 3) % _sprites.Count)+ 2];
        right.GetComponent<SpriteRenderer>().sortingOrder = 1;
        right.AddComponent<PolygonCollider2D>().usedByComposite = true;
        // Utils.SetLengthAndHeightInWorldPosition(right, 1, height);
        float rightLengthInWorld = (int)right.GetComponent<SpriteRenderer>().sprite.textureRect.width / (float)LevelBuilder.PIXEL_PER_UNIT;
        right.layer = 6;

        Assert.IsTrue(leftLengthInWorld + rightLengthInWorld < width);

        GameObject mid = new GameObject("mid");
        mid.AddComponent<SpriteRenderer>().sprite = _sprites[((playerNum * 3) % _sprites.Count) + 1];
        mid.GetComponent<SpriteRenderer>().sortingOrder = 1;
        mid.AddComponent<PolygonCollider2D>().usedByComposite = true;
        // Utils.SetLengthAndHeightInWorldPosition(mid, width - 2, height);
        float midLengthInWorld = (int)mid.GetComponent<SpriteRenderer>().sprite.textureRect.width / (float)LevelBuilder.PIXEL_PER_UNIT;
        Utils.SetLengthAndHeightInWorldPosition(mid, width - leftLengthInWorld - rightLengthInWorld, 0, true);

        mid.layer = 6;


        Vector3 midPos = new Vector3(left.transform.position.x + leftLengthInWorld/2  + (width - leftLengthInWorld - rightLengthInWorld)/2, left.transform.position.y, 1);
        mid.transform.position = midPos;
        Vector3 rightPos = new Vector3(midPos.x + (width - leftLengthInWorld - rightLengthInWorld)/2 + rightLengthInWorld/2, left.transform.position.y, 1);
        right.transform.position = rightPos;

        GameObject master = Instantiate(parent_paddle);
        master.transform.position = mid.transform.position;

        mid.transform.SetParent(master.transform);
        left.transform.SetParent(master.transform);
        right.transform.SetParent(master.transform);

        master.transform.position = position;

        Paddle paddle_script = master.GetComponent<Paddle>();

        
        return paddle_script;
    }

    // private void NormalizeScale(GameObject obj)
    // {
    //     var bounds = obj.GetComponent<SpriteRenderer>().sprite.bounds;
    //     Vector3 newScale = new Vector3(1/bounds.size.x, 1/bounds.size.y, 1);
    //     obj.transform.localScale = newScale;
    // }
}
