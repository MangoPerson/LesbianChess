using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public BoardPosition bpos;
    public bool hasMoved = false;
    public int type;
    public int x, y;
    public int index;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject central = GameObject.Find("Central");

        SpriteRenderer spr_rend = gameObject.GetComponent<SpriteRenderer>();
        spr_rend.sprite = central.GetComponent<Central>().sprites[type];
        spr_rend.transform.localScale = .135f * new Vector3(1, 1, 1);
        
        gameObject.transform.position = new Vector3(x - 3.5f, y - 3.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Piece Create(GameObject obj, int type, int index)
    {
        Piece ret = obj.AddComponent<Piece>();
        ret.index = index;
        ret.x = index % 8;
        ret.y = index / 8;
        ret.type = type;

        return ret;
    }

    public static Piece Create(GameObject obj, int type, int x, int y)
    {
        Piece ret = obj.AddComponent<Piece>();
        ret.index = 8 * y + x;
        ret.x = x;
        ret.y = y;
        ret.type = type;

        return ret;
    }

    public void UpdatePos(bool useIndex)
    {
        if (useIndex)
        {
            x = index % 8;
            y = index / 8;
        }
        else
        {
            index = 8 * y + x;
        }
        gameObject.transform.position = new Vector3(x - 3.5f, y - 3.5f, 0);
    }

    public bool isWhite() {
        return type < 6;
    }
}
