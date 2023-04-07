using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Paddle BuildPadle(int playerNum, Vector2 position, float width, float height)
    {

        GameObject left = new GameObject("left");
        left.AddComponent<SpriteRenderer>().sprite = _sprites[((playerNum * 3) % _sprites.Count) + 0];
        left.AddComponent<PolygonCollider2D>().usedByComposite = true;
        Utils.SetLengthAndHeightInWorldPosition(left, 1.0f, height);
        left.layer = 6;

        GameObject mid = new GameObject("mid");
        mid.AddComponent<SpriteRenderer>().sprite = _sprites[((playerNum * 3) % _sprites.Count) + 1];
        mid.AddComponent<PolygonCollider2D>().usedByComposite = true;
        Utils.SetLengthAndHeightInWorldPosition(mid, width - 2, height);
        mid.layer = 6;

        GameObject right = new GameObject("right");
        right.AddComponent<SpriteRenderer>().sprite = _sprites[((playerNum * 3) % _sprites.Count)+ 2];
        right.AddComponent<PolygonCollider2D>().usedByComposite = true;
        Utils.SetLengthAndHeightInWorldPosition(right, 1, height);
        right.layer = 6;

        Vector3 midPos = new Vector3(left.transform.position.x + 0.5f  + (width - 2)/2, left.transform.position.y, 1);
        mid.transform.position = midPos;
        Vector3 rightPos = new Vector3(midPos.x + (width - 2)/2 + 0.5f, left.transform.position.y, 1);
        right.transform.position = rightPos;

        //create a parent gameobject that holds paddle pieces
        // GameObject master = new GameObject("player " + playerNum);
        // master.transform.position = mid.transform.position;
        // master.AddComponent<CompositeCollider2D>();   //composite collider that encompasses colliders in child objs
        // master.AddComponent<Rigidbody2D>();
        // Rigidbody2D rb =  master.GetComponent<Rigidbody2D>();
        // rb.constraints = RigidbodyConstraints2D.FreezePosition;
        // master.layer = 6;   //set layer to "Paddle" layer
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
