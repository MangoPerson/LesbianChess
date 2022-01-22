using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    public Color lightCol, darkCol;
    public Sprite[] sprites;

    public BoardPosition currentPos;

    Piece selected;

    // Start is called before the first frame update
    void Start()
    {
        List<int> start_indices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63 };
        List<int> start_types = new List<int> { 3, 2, 1, 4, 4, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 9, 8, 7, 10, 10, 7, 8, 9 };

        BoardPosition bpos = new BoardPosition(start_indices, start_types);

        currentPos = bpos;
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Camera.main;
        Vector3 wpos = cam.ScreenToWorldPoint(Input.mousePosition);
        wpos.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            foreach (Piece piece in currentPos.pieces)
            {
                if (Vector3.Distance(wpos, piece.transform.position) < .4)
                {
                    selected = piece;
                    break;
                }
            }
        }

        if (selected != null)
        {
            wpos.z = -1;
            selected.transform.position = wpos;

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 target = new Vector2((4 + Mathf.Round(wpos.x - .5f)), (4 + Mathf.Round(wpos.y - .5f)));
                Move move = new Move(selected, target);

                if (move.isLegal())
                {
                    currentPos.UpdateBoard(move);
                }
                else
                {
                    selected.UpdatePos(false);
                }

                selected = null;
            }
        }

    }
}
