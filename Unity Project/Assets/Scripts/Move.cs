using UnityEngine;

public class Move
{
    //board position that the move acts on
    public BoardPosition bpos;

    //piece being moved
    public Piece piece;

    //target square in terms of file/rank
    public Vector2 target;

    //opponent piece that the piece lands on
    public readonly Piece landsOn = null;

    //constructor with parameters -- piece: piece to be moved; target: target square in terms of file/rank
    public Move(Piece piece, Vector2 target)
    {
        //set object fields
        bpos = piece.bpos;
        this.piece = piece;
        this.target = target;

        //CHECK IF THE MOVE LANDS ON AN OPPONENT PIECE
        //loop through all pieces
        foreach (Piece check_p in bpos.pieces)
        {
            //check if the piece is in the same position as the target, and is the opposite color
            if (check_p.x == target.x && check_p.y == target.y && (check_p.IsWhite() != piece.IsWhite()))
            {
                landsOn = check_p;
                break;
            }
        }
    }

    //check if the move is legal
    public bool IsLegal()
    {
        //signed and unsigned differences in the x and y positions of the starting square and the target square
        int difx = (int)Mathf.Abs(target.x - piece.x);
        int dify = (int)Mathf.Abs(target.y - piece.y);
        int difx_n = (int)(target.x - piece.x);
        int dify_n = (int)(target.y - piece.y);

        //ILLEGAL IF:

        //the target square is the same as the starting square
        if (target.x == piece.x && target.y == piece.y)
        {
            return false;
        }

        //the target is outside the board
        if (target.x < 0 || target.y < 0 || target.x > 7 || target.y > 7)
        {
            return false;
        }

        //the piece's color doesn't match whose turn it is
        if (bpos.wMove != (piece.type < 6))
        {
            return false;
        }

        //the piece lands on a piece of the same color
        foreach (Piece check_p in bpos.pieces)
        {
            if(check_p != piece && check_p.x == target.x && check_p.y == target.y && (check_p.IsWhite() == piece.IsWhite()))
            {
                return false;
            }
        }

        //SPECIAL RULES FOR DIFFERENT PIECE TYPES
        switch (piece.type % 6) {
            //pawn
            case 0:
                if (target.y != piece.y + 1 - piece.type / 3 && !(target.y == piece.y + 2 * (1 - piece.type / 3) && !piece.hasMoved))
                {
                    return false;
                }
                if ((difx == 1 && landsOn == null) || (difx == 0 && landsOn != null) || difx > 1 || (target.y == piece.y + 2 * (1 - piece.type / 3) && landsOn != null))
                {
                    return false;
                }
                break;
            //bishop
            case 1:
                if (difx != dify)
                {
                    return false;
                }
                else
                {
                    for(float j = 0; j < difx; j++)
                    {
                        float i_x = difx_n * (j / difx);
                        float i_y = dify_n * (j / difx);
                        foreach (Piece check_p in bpos.pieces)
                        {
                            if (check_p.x == piece.x + i_x && check_p.y == piece.y + i_y && check_p != piece)
                            {
                                return false;
                            }
                        }
                    }
                }
                break;
            //night
            case 2:
                if(difx + dify != 3 || difx > 2 || dify > 2)
                {
                    return false;
                }
                break;
            //rook
            case 3:
                if (difx != 0 && dify != 0)
                {
                    return false;
                }
                else
                {
                    if (dify == 0)
                    {
                        for (float j = 0; j < difx; j++)
                        {
                            float i = j * Mathf.Sign(difx_n);
                            foreach (Piece check_p in bpos.pieces)
                            {
                                if (check_p.x == piece.x + i && check_p.y == piece.y && check_p != piece)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (difx == 0)
                    {
                        for (float j = 0; j < dify; j++)
                        {
                            float i = j * Mathf.Sign(dify_n);
                            foreach (Piece check_p in bpos.pieces)
                            {
                                if (check_p.x == piece.x && check_p.y == piece.y + i && check_p != piece)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                break;
            //queen
            case 4:
                if (difx == dify)
                {
                    for (float j = 0; j < difx; j++)
                    {
                        float i_x = difx_n * (j / difx);
                        float i_y = dify_n * (j / difx);
                        foreach (Piece check_p in bpos.pieces)
                        {
                            if (check_p.x == piece.x + i_x && check_p.y == piece.y + i_y && check_p != piece)
                            {
                                return false;
                            }
                        }
                    }
                }
                else if (difx == 0 || dify == 0)
                {
                    if (dify == 0)
                    {
                        for (float j = 0; j < difx; j++)
                        {
                            float i = j * Mathf.Sign(difx_n);
                            foreach (Piece check_p in bpos.pieces)
                            {
                                if (check_p.x == piece.x + i && check_p.y == piece.y && check_p != piece)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    if (difx == 0)
                    {
                        for (float j = 0; j < dify; j++)
                        {
                            float i = j * Mathf.Sign(dify_n);
                            foreach (Piece check_p in bpos.pieces)
                            {
                                if (check_p.x == piece.x && check_p.y == piece.y + i && check_p != piece)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
                break;
        }
        
        //if none of the illegality checks stop the move, it is legal
        return true;
    }
}
