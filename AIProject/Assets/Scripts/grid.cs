//Bonga Maswanganye
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{
    public LayerMask Obstacle;
    public Transform StartPosition;
    public Vector2 GridSize;
    public float NodeRadius;
    public float NodeDistance;

    private float NodeDiameter;
    private int GridX, GridY;
    
    private Node[,] Grid;
    public List<Node> FinalPath;

    public Node StartingNode;
    public Node EndingNode;

    private void Start()
    {
        NodeDiameter = NodeRadius * 2;
        GridX = Mathf.RoundToInt(GridSize.x / NodeDiameter);
        GridY = Mathf.RoundToInt(GridSize.y / NodeDiameter);
        CreateGrid();
    }

    public void CreateGrid()
    {
        Grid = new Node[GridX, GridY];
        Vector3 Bottomleft = (transform.position - Vector3.right * GridSize.x / 2 - Vector3.forward * GridSize.y / 2);
        for (int x = 0; x<GridX;x++)
        {
            for (int y = 0; y<GridY;y++)
            {
                Vector3 WorldPos = Bottomleft + Vector3.right * (x * NodeDiameter + NodeRadius) + Vector3.forward * (y * NodeDiameter + NodeRadius);
                bool wall = true;
                if (Physics.CheckSphere(WorldPos,NodeRadius,Obstacle))
                {
                    wall = false;
                }
                Grid[x,y] = new Node(wall, WorldPos, x, y);

            }
        }
    }

    public Node NodeWorldPosition(Vector3 WorldPosition)
    {
        float xpoint = ((WorldPosition.x + GridSize.x / 2) / GridSize.x);
        float ypoint = ((WorldPosition.z + GridSize.y / 2) / GridSize.y);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);

        int x = Mathf.RoundToInt((GridX - 1) * xpoint);
        int y = Mathf.RoundToInt((GridY - 1) * ypoint);

        Debug.LogError("("+Grid[x,y].NodeX+","+Grid[x,y].NodeY+")");
        return Grid[x,y];
    }

    public List<Node> GetNeighbor(Node StartNode)
    {
        List<Node> Neighbors = new List<Node>();
        int xcheck;
        int ycheck;
        //right
        xcheck = StartNode.NodeX + 1;
        ycheck = StartNode.NodeY;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }
        //left
        xcheck = StartNode.NodeX - 1;
        ycheck = StartNode.NodeY;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }
        //up
        xcheck = StartNode.NodeX;
        ycheck = StartNode.NodeY+1;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }
        //down
        xcheck = StartNode.NodeX;
        ycheck = StartNode.NodeY-1;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }
        //up-right
        xcheck = StartNode.NodeX + 1;
        ycheck = StartNode.NodeY+1;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }
        //up-left
        xcheck = StartNode.NodeX - 1;
        ycheck = StartNode.NodeY + 1;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }
        //down-right
        xcheck = StartNode.NodeX + 1;
        ycheck = StartNode.NodeY - 1;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }
        //down-left
        xcheck = StartNode.NodeX - 1;
        ycheck = StartNode.NodeY - 1;
        if (xcheck >= 0 && xcheck < GridX)
        {
            if (ycheck >= 0 && ycheck < GridY)
            {
                Neighbors.Add(Grid[xcheck, ycheck]);
            }
        }

        Debug.Log("Starting At: (" + StartNode.NodeX + ", " + StartNode.NodeY + "). Neighbors: ");
        foreach (Node n in Neighbors)
        {
            Debug.Log("(" + n.NodeX + ", " + n.NodeY + ")");

        }
        return Neighbors;
    }
    private void OnDrawGizmos()
    {
        //draw info
        Gizmos.DrawWireCube(transform.position, new Vector3(GridSize.x, 1, GridSize.y));
        if (Grid != null)
        {
            foreach(Node N in Grid)
            {
                if (N.IsWall)
                {
                    Gizmos.color = Color.blue;
                }

                else
                {
                    Gizmos.color = Color.red;
                }
                if (N == StartingNode)
                {
                    Gizmos.color = Color.yellow;
                }

                if (FinalPath != null)
                {
                    if (FinalPath.Contains(N))
                    {
                        Gizmos.color = Color.green;
                        if (N == EndingNode)
                        {
                            Gizmos.color = Color.white;
                        }
                    }
                }

                Gizmos.DrawCube(N.Position, Vector3.one * (NodeDiameter - NodeDistance));
            }
        }
    }

}
