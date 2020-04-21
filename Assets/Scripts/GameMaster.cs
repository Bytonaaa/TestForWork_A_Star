using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMaster : MonoBehaviour
{
    [SerializeField] 
    private Tilemap _tilemap;

    [SerializeField] 
    private Tile _emptyTile;

    [SerializeField] 
    private Tile _blockTile;

    [SerializeField] 
    private Tile _endTile;

    [SerializeField] 
    private Tile _pathTile;

    [SerializeField] 
    private int m_width;

    [SerializeField]
    private int m_height;
    
    private bool[,] _map;

    private AstarPlanarAlgorithm _astarPlanarAlgorithm;

    private Vector2Int? _startPoint;
    private Vector2Int? _endPoint;
    
    private bool IsCoordinateInMap(Vector3Int coordinate) =>
        !(coordinate.x < 0
        || coordinate.y < 0
        || coordinate.x >= _map.GetLength(0)
        || coordinate.y >= _map.GetLength(1));

    
    private void Awake()
    {
        _map = new bool[m_width, m_height];
        _astarPlanarAlgorithm = new AstarPlanarAlgorithm(new Graph2DRealisation(_map));
        ResetMap();
    }

    public void InverseTileState(Vector3Int coordinate)
    {
        if (!IsCoordinateInMap(coordinate))
        {
            return;
        }
        
        if (_map[coordinate.x, coordinate.y])
        {
            _tilemap.SetTile(coordinate, _blockTile);
            _map[coordinate.x, coordinate.y] = false;
        }
        else
        {
            _tilemap.SetTile(coordinate, _emptyTile);
            _map[coordinate.x, coordinate.y] = true;
        }
        
        ClearEndPointsIfNeed(coordinate);
    }

    public void SetTileStateEnd(Vector3Int coordinate)
    {
        if (!IsCoordinateInMap(coordinate))
        {
            return;
        }
        
        _tilemap.SetTile(coordinate, _endTile);
        _map[coordinate.x, coordinate.y] = true;
        
        if (_endPoint.HasValue)
        {
            if (_startPoint.HasValue)
            {
                _tilemap.SetTile(new Vector3Int(_startPoint.Value.x, _startPoint.Value.y, 0), _emptyTile);
            }
            _startPoint = _endPoint;
        }

        _endPoint = new Vector2Int(coordinate.x , coordinate.y);
    }

    private void SetTileWay(Vector3Int coordinate)
    {
        _tilemap.SetTile(coordinate, _pathTile);
    }
    
    private void ClearEndPointsIfNeed(Vector3Int coordinate)
    {
        var point2D = new Vector2(coordinate.x, coordinate.y);
        if (point2D == _startPoint)
        {
            _startPoint = null;
        }
        else if (point2D == _endPoint)
        {
            _endPoint = null;
        }
    }

    private void RemoveWayTilesOnMap()
    {
        for (int x = 0; x < m_width; ++x)
        {
            for (int y = 0; y < m_width; ++y)
            {
                var point = new Vector2Int(x, y);
                if (_startPoint != point 
                    && _endPoint != point 
                    && _map[x, y])
                {
                    _tilemap.SetTile(new Vector3Int(x, y, 0), _emptyTile);
                }
            }
        }
    }


    public void CalculatePath()
    {
        if (!_startPoint.HasValue)
        {
            Debug.LogWarning("Pls select start point by Left Click");
            return;
        }
        
        if (!_endPoint.HasValue)
        {
            Debug.LogWarning("Pls select end point by Left Click");
            return;
        }

        RemoveWayTilesOnMap();

        var path = _astarPlanarAlgorithm.PathResult(_startPoint.Value, _endPoint.Value);

        if (path == null)
        {
            Debug.LogWarning($"No way from { _startPoint} to {_endPoint}");
            return;
        }
        
        for (int i = 0; i < path.Count; ++i)
        {
            var point = path[i];
            if (point != _endPoint)
            {
                SetTileWay(new Vector3Int(point.x, point.y, 0));
            }
        }
    }

    public void ResetMap()
    {
        for (int x = 0; x < m_width; ++x)
        {
            for (int y = 0; y < m_width; ++y)
            {
                _tilemap.SetTile(new Vector3Int(x, y, 0), _emptyTile);
                _map[x, y] = true;
            }
        }

        _startPoint = null;
        _endPoint = null;
    }
    
    public void FillMapByRandom()
    {
        for (int x = 0; x < m_width; ++x)
        {
            for (int y = 0; y < m_width; ++y)
            {
                if (Random.value < 0.5f)
                {
                    InverseTileState(new Vector3Int(x, y, 0));
                }
            }
        }

        _startPoint = null;
        _endPoint = null;
    }

    public Vector3Int WorldToCell(Vector3 position) => _tilemap.WorldToCell(position);
    
}
