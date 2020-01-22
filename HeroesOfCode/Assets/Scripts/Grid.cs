using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class Grid : MonoBehaviour
{
    [Header("Grid Settings")] public float nodeRadius;
    private float nodeDiameter;

    [Tooltip("The amount of rows in the grid")]
    public int gridSizeX;

    [Tooltip("The amount of columns in the grid")]
    public int gridSizeY;

    public LayerMask unwalkableMask;

    private Vector2 gridWorldSize;

    // A prefab for highlighting a cell on a map
    public GameObject cell;

    private GameObject cellInstance;

    // A matrix to keep the info about the Nodes in the Grid
    private Node[,] grid;

    private Node lastHighlightedNode;

    // A class used for A* algorithm calculation
    private Pathfinding pathfinding;
    [HideInInspector] public List<Node> path;
    [HideInInspector] public List<GameObject> highlightedCellsList;
    [HideInInspector] public Vector3[] worldPath;

//    Color pathColor = new Color(0, 0, 0, .2f);
    Color pathColor = new Color();


    void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();

        //TODO: Find a better color
        ColorUtility.TryParseHtmlString("#BAE3E1", out pathColor);
        pathColor.a = .7f;
    }

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridWorldSize.x = gridSizeX * nodeDiameter;
        gridWorldSize.y = gridSizeY * nodeDiameter;
        GetComponent<BoxCollider2D>().size = gridWorldSize;
        GameEvents.current.OnCellCancelled += ErasePathHighlight;
        GameEvents.current.OnMovementFinished += ErasePathHighlight;
        CreateGrid();
    }

    // Generates a grid according to the parameters and the unwalkable mask
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        // The first node in the grid is located in the bottom left corner
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
        // Ignored when the user is confirming the target cell and when the player object is moving
        if (!ConfirmCellMenu.confirmCellMenuIsOn)
        {
            // Instantiate a cell prefab to highlight the target cell
            Vector3 mousePosition = GetMousePosition();
            if (!cellInstance)
            {
                lastHighlightedNode = NodeFromWorldPoint(mousePosition);
                // Prevents from highlighting player's cell
                bool playerNode = NodeFromWorldPoint(pathfinding.Seeker.position).Equals(lastHighlightedNode);
                if (lastHighlightedNode.walkable && !playerNode)
                {
                    cellInstance = Instantiate(cell,
                        new Vector3(lastHighlightedNode.worldPosition.x, lastHighlightedNode.worldPosition.y, 0),
                        Quaternion.identity);

                    cellInstance.transform.parent = gameObject.transform;
                }
            }
            // There can only be one instance of the target cell
            else
            {
                Node currentNode = NodeFromWorldPoint(mousePosition);
                if (!lastHighlightedNode.Equals(currentNode))
                {
                    Destroy(cellInstance);
                }
            }

            // The A* algorithm is executed to calculate the shortest path to the target cell
            // as long as it's walkable

            // To prevent automatic cell confirm when the cell is under confirm button
            if (Input.GetMouseButtonUp(0))
            {
                Node targetNode = NodeFromWorldPoint(mousePosition);
                // Prevents from clicking on player's cell
                bool playerNode = NodeFromWorldPoint(pathfinding.Seeker.position).Equals(targetNode);
                if (targetNode.walkable && !playerNode)
                {
                    path = pathfinding.FindPath(targetNode, this);
                    HighlightPath();
                    worldPath = pathfinding.SimplifyPath(path);

                    GameEvents.current.CellSelected();
                }
            }
        }
    }

    // Recalculates mouse position to Screen World coordinates
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

    // Returns the neighbours of a specific node (8 at most).
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                // To check that the current node is not outside of the grid
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

    // Calculates the Node coordinates in the grid from the world coordinates
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

    // Generates a set of cell prefabs over the Grid according to the path calculated by A* algorithm.
    // Each cell has a transparent color and that's how the path is highlighted to the user.
    public void HighlightPath()
    {
        ErasePathHighlight();

        // Highlighting player's cell
        GameObject startCell = Instantiate(cell,
            new Vector3(pathfinding.Seeker.position.x, pathfinding.Seeker.position.y, 0),
            Quaternion.identity);
        startCell.transform.parent = gameObject.transform;
        startCell.GetComponent<SpriteRenderer>().color = pathColor;
        highlightedCellsList.Add(startCell);

        foreach (Node node in path)
        {
            GameObject pathCell = Instantiate(cell, new Vector3(node.worldPosition.x, node.worldPosition.y, 0),
                Quaternion.identity);

            pathCell.transform.parent = gameObject.transform;
            pathCell.GetComponent<SpriteRenderer>().color = pathColor;

            highlightedCellsList.Add(pathCell);
        }
    }

    // Destroys set of cell prefabs (that were generated by HighlightPath) if there's any.
    public void ErasePathHighlight()
    {
        if (highlightedCellsList.Any())
        {
            foreach (GameObject o in highlightedCellsList)
            {
                Destroy(o);
            }

            highlightedCellsList.Clear();
        }
    }
}