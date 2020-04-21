using System.Collections.Generic;
using UnityEngine;

public interface Graph2D
{
    List<Vector2Int> GetNeighbors(Vector2Int point);
    int WayCost(Vector2Int from, Vector2Int to);
    int HeuristicWayCost(Vector2Int from, Vector2Int to);
}
