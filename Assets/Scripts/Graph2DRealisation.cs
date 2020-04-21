using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph2DRealisation : Graph2D
{
    //По диагонали мы не ходим. Для расчета расстояния между точками используем манхетенское расстояние.
    
    private readonly bool[,] _map; //Значение _map[x, y] = false, означает, что в позиции (x, y) установлен блок, через который нельзя пройти. True означет, что пройти возможно.

    private readonly Vector2Int[] _neighborsVectors =
    {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };

    public Graph2DRealisation(bool[,] map)
    {
        _map = map;
    }
    
    public List<Vector2Int> GetNeighbors(Vector2Int point)
    {
        var neighbors = new List<Vector2Int>();
        
        for (int i = 0; i < _neighborsVectors.Length; ++i)
        {
            var nextNeighbor = point + _neighborsVectors[i];

            if (nextNeighbor.x < 0
                || nextNeighbor.y < 0
                || nextNeighbor.x >= _map.GetLength(0)
                || nextNeighbor.y >= _map.GetLength(1)
                || !_map[nextNeighbor.x, nextNeighbor.y])
            {
                continue;
            }
            
            neighbors.Add(nextNeighbor);
        }

        return neighbors;
    }
    
    private int GetManhattanDistance(Vector2Int @from, Vector2Int to)
    {
        var vecDiff = (to - from);
        return Mathf.Abs(vecDiff.x) + Mathf.Abs(vecDiff.y);
    }

    public int WayCost(Vector2Int @from, Vector2Int to)
    {
        return GetManhattanDistance(from, to);
    }

    public int HeuristicWayCost(Vector2Int @from, Vector2Int to)
    {
        return GetManhattanDistance(from, to);
    }
}
