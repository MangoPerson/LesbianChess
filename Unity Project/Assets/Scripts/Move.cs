using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move
{
    public BoardPosition bpos;
    public Piece piece;
    public Vector2 target;
    public int index;

    public readonly Piece landsOn = null;

    public Move(Piece piece, Vector2 target)
    {
        bpos = piece.bpos;
        this.piece = piece;
        this.target = target;
        index = 8 * (int)target.x + (int)target.y;

        foreach (Piece check_p in bpos.pieces)
        {
            if (check_p != piece && check_p.x == target.x && check_p.y == target.y && (check_p.isWhite() != piece.isWhite()))
            {
                landsOn = check_p;
                break;
            }
        }
    }

    public Move(Piece piece, int index)
    {
        bpos = piece.bpos;
        this.piece = piece;
        this.index = index;
        target = new Vector2(index % 8, index / 8);

        foreach (Piece check_p in bpos.pieces)
        {
            if (check_p != piece && check_p.x == target.x && check_p.y == target.y && (check_p.isWhite() != piece.isWhite()))
            {
                landsOn = check_p;
                break;
            }
        }
    }

    public bool isLegal()
    {
        int difx = (int)Mathf.Abs(target.x - piece.x);
        int dify = (int)Mathf.Abs(target.y - piece.y);
        int difx_n = (int)(target.x - piece.x);
        int dify_n = (int)(target.y - piece.y);

        if (target.x == piece.x && target.y == piece.y)
        {
            return false;
        }

        if (target.x < 0 || target.y < 0 || target.x > 7 || target.y > 7)
        {
            return false;
        }

        if (bpos.wMove != (piece.type < 6))
        {
            return false;
        }

        foreach (Piece check_p in bpos.pieces)
        {
            if(check_p != piece && check_p.x == target.x && check_p.y == target.y && (check_p.isWhite() == piece.isWhite()))
            {
                return false;
            }
        }

        switch (piece.type % 6) {
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
            case 2:
                if(difx + dify != 3 || difx > 2 || dify > 2)
                {
                    return false;
                }
                break;
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

        return true;
    }
}
