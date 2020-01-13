using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public int gridX, gridY;
    public bool walkable;

    public int gCost, hCost;
    public Node parent;



    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public override bool Equals(object obj)
    {
        Node other = obj as Node;
        if (other == null) return false;
        return (this.gridX == other.gridX) && (this.gridY == other.gridY);
    }

    public override string ToString()
    {
        return "Node: " + this.gridX + " " + this.gridY;
    }
}