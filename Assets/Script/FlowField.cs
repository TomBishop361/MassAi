
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class FlowField
{
    public Cell[,] CurrentGrid;
    public Cell[,] MainGrid;
    public Vector2 gridSize { get; private set; }
    public float cellRadius { get; private set; }
    public Cell destinationCell;

    private float cellDiameter;

    List<Vector3> secondaryTargets = new List<Vector3>();
    List<Cell[,]> secondaryInfluence = new List<Cell[,]>();

    public FlowField(float _cellRadius, Vector2 _gridSize)
    {
        cellRadius = _cellRadius;
        cellDiameter = cellRadius * 2f;
        gridSize = _gridSize;
    }

 
   public void CreateMainGrid(Vector3 DestinationCell)
    {        
        CreateGrid(out CurrentGrid, gridSize, Vector2.zero);
        destinationCell = GetCellFromWorldPos(DestinationCell);
        CreateCostField(CurrentGrid);
        CreateIntegrationField(destinationCell, CurrentGrid,gridSize);
        CreateFlowField(CurrentGrid,gridSize);
        StoreUnalteredGrid();      

        
    }

    private void StoreUnalteredGrid()
    {
        MainGrid = new Cell[(int)gridSize.x, (int)gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                MainGrid[x, y] = CurrentGrid[x, y].Clone();

            }
        }
    }


    public void addSecondaryTarget(Vector3 position, int influence)
    {        
        secondaryTargets.Add(position);

        Cell[,] miniGrid;
        Vector2 posIndex = getCellIndexFromWorldPos(position);
        Vector2 miniGridSize = new Vector2(influence * 2, influence * 2);
        CreateGrid(out miniGrid, miniGridSize, new Vector2(posIndex.x - influence, posIndex.y - influence));
        CreateCostField(miniGrid);
        CreateIntegrationField(miniGrid[influence, influence], miniGrid, miniGridSize);
        CreateFlowField(miniGrid, miniGridSize);
        addGridLayer(miniGrid, position, influence);
        secondaryInfluence.Add(miniGrid);
    }

    //Add weight that = 1-dist/radius
    public void addGridLayer(Cell[,] minigrid, Vector3 position, int radius)
    {        
        Vector2 Pos = getCellIndexFromWorldPos(position) - new Vector2(radius,radius);
        for (int x = 0; x < radius*2 ; x++)
        {          
             
            for (int y = 0; y < radius*2; y++)
            {
                if (!isCellValid(Pos)) continue;

                if (minigrid[x, y].cost != 255)
                {
                    Cell currentgridcell = CurrentGrid[(int)Pos.x, (int)Pos.y];
                    float infWeight = 1 - Vector3.Distance(position, currentgridcell.worldPos) / radius;
                    if (infWeight > currentgridcell.weight)
                    {                        
                        currentgridcell.bestDirection = minigrid[x, y].bestDirection;
                        currentgridcell.weight = infWeight;
                    }                   
                    
                }
                Pos = Pos+ new Vector2(0, 1);
            }
            Pos = Pos+ new Vector2(1, -radius*2);
        }
    }


    public void removeSecondaryTarget(Vector3 position, int radius)
    {   
        if (!secondaryTargets.Contains(position)) return;

        //iterate over grid in influence and restore to original grid.
        Vector2 pos = getCellIndexFromWorldPos(position) - new Vector2(radius, radius);
        for (int x = 0; x < radius*2; x++)
        {
            for(int y = 0;y < radius*2; y++)
            {
                Cell CurrentCell = CurrentGrid[(int)pos.x, (int)pos.y];
                CurrentCell.bestDirection = MainGrid[(int)pos.x, (int)pos.y].bestDirection;
                CurrentCell.weight = float.MinValue;
                pos = pos + new Vector2(0, 1);
            }
            pos = pos + new Vector2(1, -radius * 2);
        }

        //Remove From both lists
        int index = secondaryTargets.IndexOf(position);
        secondaryTargets.RemoveAt(index);
        secondaryInfluence.RemoveAt(index);

        // Go through buildings within range and recalculate their flow
        foreach(Vector3 target in secondaryTargets)
        {
            if((position-target).sqrMagnitude < (12 * 12)*2) //This should be compared to the highest radius building^2 * 2 (atm this is 12^2 * 2)
            {
                int targetIndex = secondaryTargets.IndexOf(target);
                addGridLayer(secondaryInfluence[targetIndex], target, radius);
            }
        }
        
    }


    public void CreateGrid(out Cell[,] _grid, Vector2 _gridSize, Vector2 Offset)
    {
        _grid = new Cell[(int)_gridSize.x, (int)_gridSize.y];

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                Vector3 worldPos = new Vector3((cellDiameter * x + cellRadius) + Offset.x, 0, (cellDiameter * y + cellRadius)+Offset.y);
                
                _grid[x, y] = new Cell(worldPos, new Vector2(x, y));
            }
        }
    }


    // need to add offset for 2nd targets so that overlap boxes are correctly indicating position of obstacles on the mini grid
    public void CreateCostField(Cell[,] _grid)
    {
        Vector3 cellHalfExtents = Vector3.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impassible", "RoughTerrain", "Mountain","Wood","Stone");
        foreach (Cell current in _grid)
        {

            Collider[] obstacles = new Collider[5];
            Physics.OverlapBoxNonAlloc(current.worldPos, cellHalfExtents,obstacles, Quaternion.identity, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider col in obstacles)
            {
                if(col == null) continue;
                if (col.gameObject.layer == 8 || col.gameObject.layer == 7 || col.gameObject.layer == 14 )
                {
                    current.IncreaseCost(255);
                    continue;
                }
                else if (!hasIncreasedCost)
                {
                    if (col.gameObject.layer == 9 || col.gameObject.layer == 17)
                    {
                        current.IncreaseCost(3);
                        hasIncreasedCost = true;
                    }
                }
            }
        }
    }

    public void CreateIntegrationField(Cell _destinationCell, Cell[,] _grid, Vector2 _gridSize)
    {
        destinationCell = _destinationCell;

        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();

        cellsToCheck.Enqueue(destinationCell);


        while (cellsToCheck.Count > 0)
        {
            Cell current = cellsToCheck.Dequeue();
            List<Cell> currentNeighbors = GetNeighborCells(current.gridIndex, GridDirection.CardinalDirections, _grid, _gridSize);
            foreach (Cell currentNeighbor in currentNeighbors)
            {
                if (currentNeighbor.cost == byte.MaxValue)  continue; 
                if (currentNeighbor.cost + current.bestCost < currentNeighbor.bestCost)
                {
                    currentNeighbor.bestCost = (ushort)(currentNeighbor.cost + current.bestCost);
                    cellsToCheck.Enqueue(currentNeighbor);
                }
            }
        }
    }

    
    public void CreateFlowField(Cell[,] _grid, Vector2 _gridSize)
    {
        
        foreach (Cell current in _grid)
        {
            if (current.cost == byte.MaxValue) continue; 
            List<Cell> currentNeighbors = GetNeighborCells(current.gridIndex, GridDirection.AllDirections,_grid, _gridSize);

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

    public bool isCellValid(Vector2 pos)
    {
        if (pos.x < 0 || pos.x >= gridSize.x || pos.y < 0 || pos.y >= gridSize.y) return false;
        else return true;
    }

    public Cell findNearestDirection(Cell cell)
    {
        List<Cell> neighbours = GetNeighborCells(cell.gridIndex, GridDirection.AllDirections, CurrentGrid, gridSize);

        foreach (Cell neighbor in neighbours)
        {
            if (neighbor.cost == byte.MaxValue) continue;
            return neighbor;
        }
        return cell;
    }

    private List<Cell> GetNeighborCells(Vector2 nodeIndex, List<GridDirection> directions, Cell[,] _grid, Vector2 _gridSize)
    {
        List<Cell> neighborCells = new List<Cell>();

        foreach (Vector2 currentDirection in directions)
        {
            Cell newNeighbor = GetCellAtRelativePos(nodeIndex, currentDirection, _grid, _gridSize);
            if (newNeighbor != null)
            {
                neighborCells.Add(newNeighbor);
            }
        }
        return neighborCells;
    }


    private Cell GetCellAtRelativePos(Vector2 orignPos, Vector2 relativePos, Cell[,] _grid, Vector2 _gridSize)
    {
        Vector2 finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= _gridSize.x || finalPos.y < 0 || finalPos.y >= _gridSize.y)
        {
            return null;
        }

        else { return _grid[(int)finalPos.x, (int)finalPos.y]; }
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.z / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        float x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        float y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return CurrentGrid[(int)x, (int)y];
    }

    public Vector2 getCellIndexFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.z / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        float x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        float y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        Vector2 index = new Vector2((int)x, (int)y);
        return index;
    }
}

 
public class Cell
{
    public Vector3  worldPos;
    public Vector2 gridIndex;
    public byte cost;
    public ushort bestCost;
    public Vector2 bestDirection;
    public bool isInfluenced;
    public float weight;

    public Cell(Vector3 _worldPos, Vector2 _gridIndex)
    {
        worldPos = _worldPos;
        gridIndex = _gridIndex;
        cost = 1;
        bestCost = ushort.MaxValue;
        bestDirection = GridDirection.None;
        isInfluenced = false;
        weight = float.MinValue;
    }

    public void IncreaseCost(int amnt)
    {
        if (cost == byte.MaxValue) { return; }
        if (amnt + cost >= 255) { cost = byte.MaxValue; }
        else { cost += (byte)amnt; }
    }

    public Cell Clone()
    {
        Cell newCell = new Cell (worldPos, gridIndex);
        newCell.cost = cost;
        newCell.bestCost = bestCost;  
        newCell.bestDirection = bestDirection;  
        return newCell;
    }
}
