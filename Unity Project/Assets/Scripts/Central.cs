using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    //colors for light squares, dark squares, and highlighted squares
    public Color lightCol, darkCol, highlightCol;

    //sprites for pieces
    public Sprite[] sprites;

    //positions and types for starting position
    public List<int> start_indices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63 };
    public List<int> start_types = new List<int> { 3, 2, 1, 4, 4, 1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 9, 8, 7, 10, 10, 7, 8, 9 };

    //current board position
    public BoardPosition currentPos;

    //piece currently being moved
    Piece selected;

    //piece that was last moved
    Piece lastSelected;

    //current highlighted squares
    List<int> highlights;

    // Start is called before the first frame update
    void Start()
    {
        //create the board position using the given piece information
        currentPos = new BoardPosition(start_indices, start_types);
    }

    // Update is called once per frame
    void Update()
    {
        //initialize the list of highlighted squares
        highlights = new List<int>(0);
        //get the main camera for mouse position to world point conversion
        Camera cam = Camera.main;
        //convert the mouse position to a point in the world, setting its z coordinate to 0
        Vector3 wpos = cam.ScreenToWorldPoint(Input.mousePosition);
        wpos.z = 0;

        //run when the left button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            //loop through all pieces currently on the board
            foreach (Piece piece in currentPos.pieces)
            {
                //if the piece is sufficiently close to the mouse position, make it the selected piece
                if (Vector3.Distance(wpos, piece.transform.position) < .4)
                {
                    selected = piece;
                    break;
                }
            }
        }

        //run when there is a selected piece
        if (selected != null)
        {
            //set the position of the piece to the mouse position, making sure it appears in front of the other sprites
            wpos.z = -1;
            selected.transform.position = wpos;

            //highlight the starting square of the selected piece
            highlights.Add(selected.index);

            //run when the left mouse button is released
            if (Input.GetMouseButtonUp(0))
            {
                //target square of the move according to where the mouse currently currently is
                Vector2 target = new Vector2((4 + Mathf.Round(wpos.x - .5f)), (4 + Mathf.Round(wpos.y - .5f)));
                //instantiate the move object
                Move move = new Move(selected, target);

                //run if the move is legal
                if (move.IsLegal())
                {
                    //set the last moved piece to be the selected piece
                    lastSelected = selected;
                    //update the board position
                    currentPos.UpdateBoard(move);
                }
                else
                {
                    //reset the position of the selected piece
                    selected.UpdatePos(false);
                }

                //make the piece no longer selected
                selected = null;

                //TEST IF THE GAME SHOULD END
                //initialize counts for black and white queens
                int qw = 0;
                int qb = 0;

                //loop through all pieces currently on the board
                foreach (Piece piece in currentPos.pieces)
                {
                    //if a black or white queen is found, add to the count
                    if (piece.type == 4) qw += 1;
                    if (piece.type == 10) qb += 1;
                }

                //run if either side has no queens
                if (qb == 0 || qw == 0)
                {
                    //clear all pieces, as well as the board
                    currentPos.EndGame();

                    //clear the list of highlighted sqares
                    highlights.Clear();

                    //clear the last moved piece
                    lastSelected = null;

                    //create a new board
                    currentPos = new BoardPosition(start_indices, start_types);
                }
            }
        }

        //highlight the square of the last moved piece
        if (lastSelected != null) highlights.Add(lastSelected.index);

        //Draw the board
        currentPos.DrawBoard(lightCol, darkCol, highlightCol, highlights);
    }
}
