using System.Collections.Generic;
using UnityEngine;

public class BoardPosition
{
    //list of pieces
    public List<Piece> pieces;
    //is it white's move?
    public bool wMove;
    //object representing the board
    GameObject board;

    //constructor, with parameters -- indices: square indices of piece positions; types: piece types (pawn, rook, etc.) 
    public BoardPosition(List<int> indices, List<int> types)
    {
        //set it to be white's move
        wMove = true;

        //create the board and pieces
        Create(indices, types);
    }

    //create the board and piececs, with parameters -- indices: square indices of piece positions; types: piece types (pawn, rook, etc.) 
    void Create(List<int> indices, List<int> types)
    {
        //create an object for the board, set it behind the pieces, and add a spirte renderer to it
        GameObject board = new GameObject();
        board.transform.position = new Vector3(0, 0, 1);
        board.AddComponent<SpriteRenderer>();

        //make sure the board can be seen throughout the file
        this.board = board;

        //get the central control object
        Central central = GameObject.Find("Central").GetComponent<Central>();

        //draw the board
        DrawBoard(central.lightCol, central.darkCol);

        //create a list of pieces
        pieces = new List<Piece>();

        //loop through the indices and types
        for (int i = 0; i < indices.Count; i++)
        {
            //create a new gameobject with a spriterenderer component
            GameObject obj = new GameObject();
            obj.AddComponent<SpriteRenderer>();

            //create a piece component on the gameobject according to the positions and types in the list
            Piece piece = Piece.Create(obj, types[i], indices[i]);

            //assign the board position of the piece to be this file
            piece.bpos = this;

            //add the new piece to the list of pieces
            pieces.Add(piece);
        }
    }

    //draw the board, with parameters -- lightCol: color for light squares; darkCol: color for dark squares; highlighCol: color for highlighted squares; highlighted: squares to highlight
    public void DrawBoard(Color lightCol, Color darkCol, Color highlightCol, List<int> highlighted)
    {
        //get the sprite renderer component on the board object
        SpriteRenderer spr_rend = board.GetComponent<SpriteRenderer>();

        //create a new texture
        Texture2D tex = new Texture2D(8, 8);

        //loop through all files and ranks
        for (int f = 0; f < 8; f++)
        {
            for (int r = 0; r < 8; r++)
            {
                //check if the square is light or dark
                bool isLight = (f + r) % 2 == 1;

                //check if the square is highlighted
                bool isHighlighted = false;
                if (highlighted != null)
                {
                    //if the index of the current square is in the list of highlighted squares, highlight it
                    if (highlighted.Contains(8 * r + f))
                    {
                        isHighlighted = true;
                    }
                }

                //set the square color according to whether it is light or dark, then whether it is highlighted
                Color squareColor = isHighlighted ? highlightCol : (isLight ? lightCol : darkCol);
                tex.SetPixel(f, r, squareColor);
            }
        }

        //make sure that the renderer doesn't interpolate the texture
        tex.filterMode = FilterMode.Point;

        //apply the changes to the texture
        tex.Apply();

        //create a new sprite with the texture, add it to the board's spirte renderer, then rescale it to fit the screen
        Sprite spr = Sprite.Create(tex, new Rect(0, 0, 8, 8), new Vector2(.5f, .5f));
        spr_rend.sprite = spr;
        spr_rend.transform.localScale = 100 * new Vector3(1, 1, 1);
    }

    //draw the board, with parameters -- lightCol: color for light squares; darkCol: color for dark squares
    public void DrawBoard(Color lightCol, Color darkCol)
    {
        //draw the board with an empty highlight list
        DrawBoard(lightCol, darkCol, Color.black, new List<int> { });
    }

    //update the board, with parameters -- move: move to update the board with, contains a piece and a target
    public void UpdateBoard(Move move)
    {
        //only do it if the move is acting on the board position defined by this file
        if(move.bpos == this) {
            //the piece has now moved
            move.piece.hasMoved = true;
            
            //set position of the piece
            move.piece.x = (int)move.target.x;
            move.piece.y = (int)move.target.y;

            //update the piece's actual position
            move.piece.UpdatePos(false);

            //run if the piece lands on an opponent piece
            if (move.landsOn != null)
            {
                //remove the captured piece from the piece list, then destroy the object itself
                pieces.Remove(move.landsOn);
                Object.Destroy(move.landsOn.gameObject);
            }

            //make it the other side's move
            wMove = !wMove;
        }
    }

    //end the game
    public void EndGame()
    {
        //destroy all pieces
        foreach (Piece piece in pieces)
        {
            Object.Destroy(piece.gameObject);
        }

        //destroy the board
        Object.Destroy(board);
    }
}
