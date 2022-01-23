using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    public Color lightCol, darkCol, highlightCol;
    public Sprite[] sprites;

    public List<int> start_indices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63 };
    public List<int> start_types = new List<int> { 3, 2, 1, 4, 4, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 9, 8, 7, 10, 10, 7, 8, 9 };

    public BoardPosition currentPos;

    Piece selected;
    Piece lastSelected;
    List<int> highlights;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = new BoardPosition(start_indices, start_types);
    }

    // Update is called once per frame
    void Update()
    {
        highlights = new List<int>(0);
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

            highlights.Add(selected.index);

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 target = new Vector2((4 + Mathf.Round(wpos.x - .5f)), (4 + Mathf.Round(wpos.y - .5f)));
                Move move = new Move(selected, target);

                if (move.isLegal())
                {
                    lastSelected = selected;
                    currentPos.UpdateBoard(move);
                }
                else
                {
                    selected.UpdatePos(false);
                }

                selected = null;

                bool endGame = true;
                int qw = 0;
                int qb = 0;
                foreach (Piece piece in currentPos.pieces)
                {
                    if (piece.type == 4) qw += 1;
                    if (piece.type == 10) qb += 1;
                }
                if (qb == 0 || qw == 0)
                {
                    currentPos.EndGame();
                    highlights.Clear();
                    lastSelected = null;
                    selected = null;
                    currentPos = new BoardPosition(start_indices, start_types);
                }
            }
        }

        if (lastSelected != null) highlights.Add(lastSelected.index);
        currentPos.DrawBoard(lightCol, darkCol, highlightCol, highlights);
    }
}
