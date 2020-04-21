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
        //Наверно можно backwardPoint и distanceToPoint объединить, или использовать вместо них массив двумерный.
        //Но, думаю, для тестового и так сойдет :)

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

                if (!distanceToPointDict.TryGetValue(neighbor, out int currentDistanceToNeighbor) ||
                    distanceToNeighbor < currentDistanceToNeighbor)
                {

                    backwardPointDict[neighbor] = currentPoint;
                    distanceToPointDict[neighbor] = distanceToNeighbor;
                    var fValue = distanceToNeighbor + _graph2D.HeuristicWayCost(currentPoint, end);

                    pointsQueue.Add(fValue, neighbor); 
                    //В этом месте может случится так, что neighbor продублируется в pointsQueue.
                    //Поэтому нужно усовершенствовать MinHeap. добавить метод, который будет изменять вес у уже добавленного элемента и обновлять структуру MinHeap,
                    //чтобы сохранялись свойства двоичной кучи.
                    //Но в нашем конкретном случае этого не случится, потому что функции WayCost и HeuristicWayCost являются консистентными.
                    //Из-за этого условие (distanceToNeighbor < currentDistanceToNeighbor) всегда будет == False.
                    
                    //Для тестового решил не делать :)
                }
            }
        }
        
        return null;
    }
    
}
