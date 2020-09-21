using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public static FieldController Instance;

    public readonly int DistanceBetweenCell = 1;

    [SerializeField]
    private ObstacleController obstacleController;

    private Cell[,] _field;

    private Int2 _startFieldPoint;
    private Int2 _endFieldPoint;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialized(Int2 startFieldPoint, Int2 endFieldPoint)
    {
        
        _startFieldPoint = startFieldPoint;
        _endFieldPoint = endFieldPoint;
        CreateField();
    }

    public void CreateField()
    {
        int xSize = Mathf.Abs(_startFieldPoint.X) + Mathf.Abs(_endFieldPoint.X)+1;
        int zSize = Mathf.Abs(_startFieldPoint.Z) + Mathf.Abs(_endFieldPoint.Z)+1;

        _field = new Cell[xSize, zSize];

        for(int z=0; z < zSize; z++)
        {
            for(int x=0; x< xSize; x++)
            {
                var newCell = new Cell(new Int2(_startFieldPoint.X + x, _startFieldPoint.Z - z ));
                _field[x, z] = newCell;
            }
        }

        UpdateObstacles();

       // DebugField();
    }

    public void UpdateObstacles()
    {
        foreach(var obstacle in obstacleController.Obstacles)
        {
            var obstacleCell = GetCellByPosition(obstacle.Position);
            if(obstacleCell != null)
                obstacleCell.SetObstacle(true);
        }
    }

    public void RemoveObstacle(Obstacle obstacle)
    {
        var obstacleCell = GetCellByPosition(obstacle.Position);
        if (obstacleCell != null)
            obstacleCell.SetObstacle(false);
        obstacleController.Obstacles.Remove(obstacle);
    }

    public List<Cell> GetAdjacentCell(Cell currentCell)
    {
        List<Cell> result = new List<Cell>();

        Cell[] adjacentPoints = new Cell[4];
        adjacentPoints[0] = new Cell(new Int2(currentCell.Position.X + DistanceBetweenCell, currentCell.Position.Z));
        adjacentPoints[1] = new Cell(new Int2(currentCell.Position.X - DistanceBetweenCell, currentCell.Position.Z));
        adjacentPoints[2] = new Cell(new Int2(currentCell.Position.X, currentCell.Position.Z + DistanceBetweenCell));
        adjacentPoints[3] = new Cell(new Int2(currentCell.Position.X, currentCell.Position.Z - DistanceBetweenCell));

        foreach (var point in adjacentPoints)
        {
            var cell = GetCellByPosition(point.Position);
            if (cell == null)
                continue;
            if (cell.IsObstacle)
                continue;

            result.Add(cell);
        }

        return result;
    }

    public Cell GetCellByPosition(Vector3 position)
    {
        Int2 newPosition = new Int2((int)position.x, (int)position.z);
        return GetCellByPosition(newPosition);
    }

    public Cell GetCellByPosition(Int2 position)
    {
        //Проверяем вышли ли за границу
        if (position.X < _startFieldPoint.X || position.X > _endFieldPoint.X || position.Z > _startFieldPoint.Z || position.Z < _endFieldPoint.Z)
            return null;

        foreach(var node in _field)
        {
            if (position == node.Position)
                return node;
        }

        return null;
    }

    private void DebugField()
    {
        for (int z = 0; z < _field.GetUpperBound(1) + 1; z++)
        {
            string s = "";
            for (int x = 0; x < _field.GetUpperBound(0) + 1; x++)
            {
                s += " " + _field[x, z];
            }
            Debug.Log(s);
        }
    }
}


public class Cell
{
    public Int2 Position { get; private set; }

    public bool IsObstacle { get; private set; }

    public Cell(Int2 position)
    {
        Position = position;
    }

    public void SetObstacle(bool value)
    {
        IsObstacle = value;
    }

    public override string ToString()
    {
        string result = string.Format("|x: {0}; y: {1}|", Position.X, Position.Z);
        return result;
    }
}

public struct Int2
{
    public int X;
    public int Z;

    public Int2(int x, int z)
    {
        X = x;
        Z = z;
    }

    public static bool operator ==(Int2 value1, Int2 value2)
    {
        return ((value1.X == value2.X) && (value1.Z == value2.Z));
    }

    public static bool operator !=(Int2 value1, Int2 value2)
    {
        return ((value1.X != value2.X) || (value1.Z != value2.Z));
    }

    public override string ToString()
    {
        string result = string.Format("|x: {0}; z: {1}|", X, Z);
        return result;
    }
}
