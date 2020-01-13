using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Grid : MonoBehaviour
{
    public float nodeRadius;
    private float nodeDiameter;

    public int gridSizeX, gridSizeY;

    public LayerMask unwalkableMask;
    private Vector2 gridWorldSize;
    public GameObject cell;
    private GameObject cellInstance;
    private Node[,] grid;
    private Node lastHighlightedNode;
    private Pathfinding pathfinding;
    [HideInInspector] public List<Node> path;
    [HideInInspector] public List<GameObject> highlightedCellsList;
    Color pathColor = new Color(0, 0, 0, .2f);

    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
    }

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridWorldSize.x = gridSizeX * nodeDiameter;
        gridWorldSize.y = gridSizeY * nodeDiameter;
        GetComponent<BoxCollider2D>().size = gridWorldSize;
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft =
            transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                                     Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    void OnMouseOver()
    {
        if (!ConfirmCellMenu.confirmCellMenuIsOn)
        {
            Vector3 mousePosition = GetMousePosition();
            if (!cellInstance)
            {
                lastHighlightedNode = NodeFromWorldPoint(mousePosition);
                if (lastHighlightedNode.walkable)
                {
                    cellInstance = Instantiate(cell,
                        new Vector3(lastHighlightedNode.worldPosition.x, lastHighlightedNode.worldPosition.y, 0),
                        Quaternion.identity);

                    cellInstance.transform.parent = gameObject.transform;
                }
            }
            else
            {
                Node currentNode = NodeFromWorldPoint(mousePosition);
                if (!lastHighlightedNode.Equals(currentNode))
                {
                    Destroy(cellInstance);
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                Node targetNode = NodeFromWorldPoint(mousePosition);
                if (targetNode.walkable)
                {
                    path = pathfinding.FindPath(targetNode, this);
                    HighlightPath();
                    GameEvents.current.CellSelected();
                }
            }
        }
    }

    public Vector3 GetMousePosition()
    {
        var v3 = Input.mousePosition;
        v3.z = transform.position.z;
        return Camera.main.ScreenToWorldPoint(v3);
    }

//    void OnMouseExit()
//    {
//        if (cellInstance)
//        {
//            Destroy(cellInstance);
//        }
//    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public void HighlightPath()
    {
        if (highlightedCellsList.Any())
        {
            foreach (GameObject o in highlightedCellsList)
            {
                Destroy(o);
            }

            highlightedCellsList.Clear();
        }

        foreach (Node node in path)
        {
            GameObject pathCell = Instantiate(cell, new Vector3(node.worldPosition.x, node.worldPosition.y, 0),
                Quaternion.identity);

            pathCell.transform.parent = gameObject.transform;
            pathCell.GetComponent<SpriteRenderer>().color = pathColor;

            highlightedCellsList.Add(pathCell);
        }
    }
}