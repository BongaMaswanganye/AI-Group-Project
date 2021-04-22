//Bonga Maswanganye
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    #region variables
    public int NodeX;
    public int NodeY;
    public bool IsWall;

    public Vector3 Position;
    public Node Parent;

    public int Gcost, Hcost;
    public int Fcost { get { return Gcost + Hcost; } }

    #endregion

    public Node(bool isWall,Vector3 Pos, int X, int Y)
    {
        IsWall = isWall;
        Position = Pos;
        NodeX = X;
        NodeY = Y;
    }

}

