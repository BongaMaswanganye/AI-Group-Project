
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    grid Grid;
    GameObject Player;
    public Transform Start;
    public Transform Target;
    bool pathfound;
    private void Awake()
    {
        Grid = GetComponent<grid>();
        Player = Start.gameObject;
        pathfound = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            FindPath(Start.position, Target.position);
            if (pathfound)
            {
                StartCoroutine(lerpPlayer());
            }
        }

    }

    public void FindPath(Vector3 StartPos, Vector3 EndPos)
    {
        pathfound = false;
       // Debug.Log("finding Path");
        Node StartPosition = Grid.NodeWorldPosition(StartPos);
        Start.position = StartPosition.Position;
        Grid.StartingNode = StartPosition;
        Node EndPosition = Grid.NodeWorldPosition(EndPos);
        Target.position = EndPosition.Position;
        Grid.EndingNode = EndPosition;

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(StartPosition);
        
        while (openList.Count > 0)
        {
            Node CurrentPos = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].Fcost < CurrentPos.Fcost || openList[i].Fcost == CurrentPos.Fcost && openList[i].Hcost < CurrentPos.Hcost)
                {
                    CurrentPos = openList[i];
                }
            }
            openList.Remove(CurrentPos);

            if (!closedList.Contains(CurrentPos))
            {
                closedList.Add(CurrentPos);
            }

           // Debug.Log("nodes in closed list: "+ closedList.Count);
            //Debug.Log("Nodes in Open list: " + openList.Count);

            if (CurrentPos == EndPosition)
            {
                GetFinalPath(StartPosition, EndPosition);
                //Debug.Log("Path Found");
                pathfound = true;

                return;
            }

            foreach (Node Neighbor in Grid.GetNeighbor(CurrentPos))
            {
                if (!Neighbor.IsWall || closedList.Contains(Neighbor))
                {
                    continue;
                }
                int MoveCost = CurrentPos.Gcost + GetManhattenDistance(CurrentPos, Neighbor);
                if (MoveCost < Neighbor.Gcost || !openList.Contains(Neighbor))
                {
                    Neighbor.Gcost = MoveCost;
                    Neighbor.Hcost = GetManhattenDistance(CurrentPos, Neighbor);
                    Neighbor.Parent = CurrentPos;
                    if (!openList.Contains(Neighbor))
                    {
                        openList.Add(Neighbor);
                    }
                }
            }
        }
        //Debug.Log("didnt find target");
        
    }

    int GetManhattenDistance(Node A, Node B)
    {
        int x = Mathf.Abs(A.NodeX - B.NodeX);
        int y = Mathf.Abs(A.NodeY - B.NodeY);

        return x + y;
    }
    IEnumerator lerpPlayer()
    { 
        int t = 0;
        while (Player.transform.position != Grid.EndingNode.Position)
        {
            MovePlayer(Grid.FinalPath[t].Position);
            Debug.Log(t);
            t++;
            yield return new WaitForSeconds(1);

        }


    }

    void MovePlayer(Vector3 NewLocation)
    {
        Player.transform.position = NewLocation;
        Start.position = NewLocation;
    }

    void GetFinalPath(Node StartNode, Node EndNode)
    {
        List<Node> Path = new List<Node>();
        Node CurrentNode = EndNode;

        while (CurrentNode != StartNode)
        {
            Path.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }
        //final path is reversed so its not backwards
        Path.Reverse();
        Debug.Log("final Path found, Final Path Count: " + Path.Count);
        Grid.FinalPath = Path;

    }
}
