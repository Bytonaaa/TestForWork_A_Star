using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private GameMaster _gameMaster;
    private Camera _camera;

    private Vector3Int? _lastTouchPoint;
    
    private void Awake()
    {
        _gameMaster = FindObjectOfType<GameMaster>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var cellCoordinate = _gameMaster.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));

            if (_lastTouchPoint != cellCoordinate)
            {
                _lastTouchPoint = cellCoordinate;
                _gameMaster.SetTileStateEnd(cellCoordinate);
            }
        }

        if (Input.GetMouseButton(1))
        {
            var cellCoordinate = _gameMaster.WorldToCell(_camera.ScreenToWorldPoint(Input.mousePosition));

            if (_lastTouchPoint != cellCoordinate)
            {
                _lastTouchPoint = cellCoordinate;
                _gameMaster.InverseTileState(cellCoordinate);
            }
        }
    }
}
