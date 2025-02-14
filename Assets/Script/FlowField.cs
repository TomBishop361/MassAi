using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities.UniversalDelegates;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Rendering;
public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2 gridSize { get; private set; }
    public float cellRadius { get; private set; }
    public Cell destinationCell;

    private float cellDiameter;

    List<float3> secondaryTargets = new List<float3>();
    List<int> secondaryInfluence = new List<int>();

    public FlowField(float _cellRadius, Vector2 _gridSize)
    {
        cellRadius = _cellRadius;
        cellDiameter = cellRadius * 2f;
        gridSize = _gridSize;
    }

    public void addSecondaryTarget(float3 position, int influence)
    {
        
        secondaryInfluence.Add(influence);
        secondaryTargets.Add(position);
        floodFillInfluence(position,influence);


    }

    public void CreateGrid()
    {
        grid = new Cell[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius, 0, cellDiameter * y + cellRadius);
                grid[x, y] = new Cell(worldPos, new Vector2(x, y));
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
                
            }
            
        }
    }


    public void floodFillInfluence(Vector3 Pos, int influenceRadius)
    {
        Cell SecondaryTarget = GetCellFromWorldPos(Pos);
        HashSet<Cell> visited = new HashSet<Cell>(); // O(1) lookup
        Queue<Cell> cellsToCheck = new Queue<Cell>();
        cellsToCheck.Enqueue(SecondaryTarget);
        visited.Add(SecondaryTarget);

        while (cellsToCheck.Count > 0)
        {
            Cell Current = cellsToCheck.Dequeue();
            visited.Add(Current);
            List<Cell> currentNeibours = GetNeighborCells(Current.gridIndex, GridDirection.AllDirections);
            foreach (Cell currentNeighbor in currentNeibours)
            {
                if (currentNeighbor.cost == byte.MaxValue || visited.Contains(currentNeighbor)) continue;

                float distSqr = math.distancesq(Pos, currentNeighbor.worldPos);
                if ((int)distSqr <= influenceRadius * influenceRadius)
                {
                    float weight = 3 - (distSqr / influenceRadius);
                    
                    Vector2 dir = (Vector2)Current.bestDirection * (1 - weight) + ( SecondaryTarget.gridIndex - Current.gridIndex);

                    dir = dir.normalized;
                    
                    Current.bestDirection = new Vector2(dir.x,dir.y);
                    cellsToCheck.Enqueue(currentNeighbor);                    
                    visited.Add(currentNeighbor);
                }
            }
        }

    }

  


    private List<Cell> GetNeighborCells(Vector2 nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighborCells = new List<Cell>();

        foreach (Vector2 currentDirection in directions)
        {
            Cell newNeighbor = GetCellAtRelativePos(nodeIndex, currentDirection);
            if (newNeighbor != null)
            {
                neighborCells.Add(newNeighbor);
            }
        }
        return neighborCells;
    }

    private Cell GetCellAtRelativePos(Vector2 orignPos, Vector2 relativePos)
    {
        Vector2 finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        }

        else { return grid[(int)finalPos.x, (int)finalPos.y]; }
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.z / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        float x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        float y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return grid[(int)x, (int)y];
    }
}




 
public class Cell
{
    public Vector3 worldPos;
    public Vector2 gridIndex;
    public byte cost;
    public ushort bestCost;
    public Vector2 bestDirection;

    public Cell(Vector3 _worldPos, Vector2 _gridIndex)
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
