using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPosition
{
    public List<Piece> pieces;
    public bool wMove;
    GameObject board;

    public BoardPosition(List<int> indices, List<int> types)
    {
        wMove = true;
        Create(indices, types);
    }

    public BoardPosition(List<int> indices, List<int> types, int en_passant, bool wMove)
    {
        this.wMove = wMove;
        Create(indices, types);
    }

    void Create(List<int> indices, List<int> types)
    {
        GameObject board = new GameObject();
        board.transform.position = new Vector3(0, 0, 1);
        board.AddComponent<SpriteRenderer>();
        this.board = board;

        Central central = GameObject.Find("Central").GetComponent<Central>();
        DrawBoard(central.lightCol, central.darkCol);

        pieces = new List<Piece>();
        for (int i = 0; i < indices.Count; i++)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<SpriteRenderer>();

            Piece piece = Piece.Create(obj, types[i], indices[i]);
            piece.bpos = this;
            pieces.Add(piece);
        }
    }

    public void DrawBoard(Color lightCol, Color darkCol, Color highlightCol, List<int> highlighted)
    {
        SpriteRenderer spr_rend = this.board.GetComponent<SpriteRenderer>();

        Texture2D tex = new Texture2D(8, 8);

        for (int f = 0; f < 8; f++)
        {
            for (int r = 0; r < 8; r++)
            {
                bool isLight = (f + r) % 2 == 1;
                bool isHighlighted = false;
                if (highlighted != null)
                {
                    if (highlighted.Contains(8 * r + f))
                    {
                        isHighlighted = true;
                    }
                }
                Color squareColor = isLight ? lightCol : darkCol;
                squareColor = isHighlighted ? highlightCol : squareColor;
                tex.SetPixel(f, r, squareColor);
            }
        }
        tex.filterMode = FilterMode.Point;

        tex.Apply();

        Sprite spr = Sprite.Create(tex, new Rect(0, 0, 8, 8), new Vector2(.5f, .5f));
        spr_rend.sprite = spr;
        spr_rend.transform.localScale = 100 * new Vector3(1, 1, 1);
    }

    public void DrawBoard(Color lightCol, Color darkCol)
    {
        DrawBoard(lightCol, darkCol, Color.black, new List<int> { });
    }

    public void UpdateBoard(Move move)
    {
        if(move.bpos == this) {
            move.piece.hasMoved = true;
            move.piece.x = (int)move.target.x;
            move.piece.y = (int)move.target.y;

            move.piece.UpdatePos(false);

            if (move.landsOn != null)
            {
                pieces.Remove(move.landsOn);
                Object.Destroy(move.landsOn.gameObject);
            }

            wMove = !wMove;
        }
    }

    public void EndGame()
    {
        foreach (Piece piece in pieces)
        {
            Object.Destroy(piece.gameObject);
        }

        Object.Destroy(board);
    }
}
