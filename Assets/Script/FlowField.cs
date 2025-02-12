using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set; }
    public float cellRadius { get; private set; }
    public Cell destinationCell;

    private float cellDiameter;

    List<float3> secondaryTargets = new List<float3>();
    List<int> secondaryInfluence = new List<int>();

    public FlowField(float _cellRadius, Vector2Int _gridSize)
    {
        cellRadius = _cellRadius;
        cellDiameter = cellRadius * 2f;
        gridSize = _gridSize;
    }

    public void addSecondaryTarget(float3 position, int influence)
    {
        
        secondaryInfluence.Add(influence);
        secondaryTargets.Add(position);
        CreateFlowField();
        
    }

    public void CreateGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);
                grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
            }
        }
    }

    public void CreateCostField()
    {
        Vector3 cellHalfExtents = Vector3.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impassible", "RoughTerrain");
        foreach (Cell current in grid)
        {
            Collider[] obstacles = Physics.OverlapBox(current.worldPos, cellHalfExtents, Quaternion.identity, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider col in obstacles)
            {
                if (col.gameObject.layer == 8)
                {
                    current.IncreaseCost(255);
                    continue;
                }
                else if (!hasIncreasedCost && col.gameObject.layer == 9)
                {
                    current.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }
    }

    public void CreateIntegrationField(Cell _destinationCell)
    {
        destinationCell = _destinationCell;

        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();

        cellsToCheck.Enqueue(destinationCell);

        while (cellsToCheck.Count > 0)
        {
            Cell current = cellsToCheck.Dequeue();
            List<Cell> currentNeighbors = GetNeighborCells(current.gridIndex, GridDirection.CardinalDirections);
            foreach (Cell currentNeighbor in currentNeighbors)
            {
                if (currentNeighbor.cost == byte.MaxValue) { continue; }
                if (currentNeighbor.cost + current.bestCost < currentNeighbor.bestCost)
                {
                    currentNeighbor.bestCost = (ushort)(currentNeighbor.cost + current.bestCost);
                    cellsToCheck.Enqueue(currentNeighbor);
                }
            }
        }
    }



    //Time to Optimise
    public void CreateFlowField()
    {
        
        foreach (Cell current in grid)
        {
            List<Cell> currentNeighbors = GetNeighborCells(current.gridIndex, GridDirection.AllDirections);

            int bestCost = current.bestCost;

            foreach (Cell currentNeighbor in currentNeighbors)
            {
                if (currentNeighbor.bestCost < bestCost)
                {
                    bestCost = currentNeighbor.bestCost;
                    current.bestDirection = GridDirection.GetDirectionFromV2I(currentNeighbor.gridIndex - current.gridIndex);
                }                
                if (secondaryInfluence.Count > 0)
                {
                    for (int i = 0; i < secondaryTargets.Count; i++)
                    {
                        float3 secondary = secondaryTargets[i];
                        int influenceRadius = secondaryInfluence[i];
                        float Dist = math.distance(current.worldPos, secondary);
                        if (Dist <= influenceRadius)
                        {
                            Cell secCell = GetCellFromWorldPos(secondary);
                            float2 directionVector = new float2(secCell.gridIndex.x - current.gridIndex.x, secCell.gridIndex.y - current.gridIndex.y);
                            directionVector = math.normalize(directionVector);

                            current.bestDirection = GridDirection.GetDirectionFromV2I(new Vector2Int(Mathf.RoundToInt(directionVector.x), Mathf.RoundToInt(directionVector.y)));

                        }

                    }
                }

            }
        }
    }

    private List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighborCells = new List<Cell>();

        foreach (Vector2Int currentDirection in directions)
        {
            Cell newNeighbor = GetCellAtRelativePos(nodeIndex, currentDirection);
            if (newNeighbor != null)
            {
                neighborCells.Add(newNeighbor);
            }
        }
        return neighborCells;
    }

    private Cell GetCellAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }

        else { return grid[finalPos.x, finalPos.y]; }
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.z / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return grid[x, y];
    }
}


 
public class Cell
{
    public Vector3 worldPos;
    public Vector2Int gridIndex;
    public byte cost;
    public ushort bestCost;
    public GridDirection bestDirection;

    public Cell(Vector3 _worldPos, Vector2Int _gridIndex)
    {
        worldPos = _worldPos;
        gridIndex = _gridIndex;
        cost = 1;
        bestCost = ushort.MaxValue;
        bestDirection = GridDirection.None;
    }

    public void IncreaseCost(int amnt)
    {
        if (cost == byte.MaxValue) { return; }
        if (amnt + cost >= 255) { cost = byte.MaxValue; }
        else { cost += (byte)amnt; }
    }
}
