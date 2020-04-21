using System;
using System.Collections.Generic;
using UnityEngine;

public class AstarPlanarAlgorithm
{
    private readonly Graph2D _graph2D;

    public AstarPlanarAlgorithm(Graph2D graph2D)
    {
        _graph2D = graph2D;
    }

    public List<Vector2Int> PathResult(Vector2Int start, Vector2Int end)
    {
        MinHeap<int, Vector2Int> pointsQueue = new MinHeap<int, Vector2Int>();
        HashSet<Vector2Int> closedPoints = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> backwardPointDict = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> distanceToPointDict = new Dictionary<Vector2Int, int>();

        var heuristic = _graph2D.HeuristicWayCost(start, end);
        pointsQueue.Add(heuristic, start);
        distanceToPointDict[start] = 0;

        while (!pointsQueue.IsEmpty())
        {
            var currentPoint = pointsQueue.Pop().m_value;
            var currentPointDistance = distanceToPointDict[currentPoint];
            
            if (currentPoint == end)
            {
                var result = new List<Vector2Int>();
                var backwardPoint = end;
                while (backwardPoint != start)
                {
                    result.Add(backwardPoint);
                    backwardPoint = backwardPointDict[backwardPoint];
                }

                result.Reverse();
                return result;
            }
            
            closedPoints.Add(currentPoint);

            foreach (var neighbor in _graph2D.GetNeighbors(currentPoint))
            {
                if (closedPoints.Contains(neighbor))
                {
                    continue;
                }

                var distanceToNeighbor = currentPointDistance + _graph2D.WayCost(currentPoint, neighbor);

                if (!distanceToPointDict.TryGetValue(neighbor, out int currentDistanceToNeighbor))
                {
                    //|| distanceToNeighbor < currentDistanceToNeighbor
                    //В общем случае это условие необходимо. И если оно выполняется, мы должны обновить ключ элемента neighbor в pointsQueue
                    //и соответсвенно заново балансировать бинарное дерево.
                    //Но т.к. в нашем случае эвристическая функция является консистентной, то neighbor появится в pointsQueue только 1 раз.
                    backwardPointDict[neighbor] = currentPoint;
                    distanceToPointDict[neighbor] = distanceToNeighbor;
                    var fValue = distanceToNeighbor + _graph2D.HeuristicWayCost(currentPoint, end);

                    pointsQueue.Add(fValue, neighbor);
                }
            }
        }
        
        return null;
    }
    
}
