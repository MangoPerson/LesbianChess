using UnityEngine;

public class Piece : MonoBehaviour
{
    //Board position that the piece is tied to
    public BoardPosition bpos;

    //Whether the piece has moved
    public bool hasMoved = false;

    //piece type (pawn, rook, etc.). Values less than 6 are white
    public int type;

    //piece's position on the board, expressed in terms of bothh file/rank and square index. (index = 8y + x; x = index % 8; and y = index / 8)
    public int x, y, index;
    
    // Start is called before the first frame update
    void Start()
    {
        //get the central control
        Central central = GameObject.Find("Central").GetComponent<Central>();

        //get the sprite renderer for the piece
        SpriteRenderer spr_rend = gameObject.GetComponent<SpriteRenderer>();

        //set the sprite for the piece according to the piece type, selecting from the list of piece sprites defined in the central control
        spr_rend.sprite = central.sprites[type];
        spr_rend.transform.localScale = .135f * new Vector3(1, 1, 1);
        
        //set the piece's actual position according to its file/rank
        gameObject.transform.position = new Vector3(x - 3.5f, y - 3.5f, 0);
    }

    //create a piece with parameters -- obj: game object to put component on; type: piece type; index: piece position (in terms of the square index)
    public static Piece Create(GameObject obj, int type, int index)
    {
        //add a piece component to the gameobject
        Piece ret = obj.AddComponent<Piece>();

        //set its position
        ret.index = index;
        ret.x = index % 8;
        ret.y = index / 8;

        //set its type
        ret.type = type;

        //return the piece
        return ret;
    }

    //create a piece with parameters -- obj: game object to put component on; type: piece type; index: piece position (in terms of file/rank)
    public static Piece Create(GameObject obj, int type, int x, int y)
    {
        //add a piece component to the gameobject
        Piece ret = obj.AddComponent<Piece>();

        //set its position
        ret.index = 8 * y + x;
        ret.x = x;
        ret.y = y;

        //set its type
        ret.type = type;

        //return the piece
        return ret;
    }

    //update the piece's actual position according to its file/rank with parameters -- useIndex: whether to update the piece's position with its index, or with its file/rank
    public void UpdatePos(bool useIndex)
    {
        //check whether to use index or file/rank
        if (useIndex)
        {
            //set file/rank according to index
            x = index % 8;
            y = index / 8;
        }
        else
        {
            //set index according to file/rank
            index = 8 * y + x;
        }

        //set piece's actual position according to its file/rank
        gameObject.transform.position = new Vector3(x - 3.5f, y - 3.5f, 0);
    }

    //check if the piece is white
    public bool IsWhite() {
        //return true if the piecetype is less than 6
        return type < 6;
    }
}
