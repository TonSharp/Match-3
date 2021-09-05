using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Token : MonoBehaviour
{
    [SerializeField] private bool _isMoveable;
    [SerializeField] private TokenType _type;
    [SerializeField] private int _lvl = 1;

    public int LVL
    {
        get => _lvl;
    }

    public TokenType Type
    {
        get => _type;
    }

    public bool IsMoveable
    {
        get => _isMoveable;
    }

    public Vector2Int GridPos;

    private bool _isMoving = false;
    private Vector2 _movePos;


    private void Update()
    {
       if(_isMoving)
        {
            if(transform.position.ToV2() == _movePos)
                _isMoving = false;

            transform.position = Vector2.Lerp(transform.position, _movePos, Time.deltaTime * 5);
        }
    }

    public void MoveToPos(Vector2Int gridPos, Tilemap map)
    {
        GridPos = gridPos;

        _movePos = map.GetCellCenterWorld(gridPos.ToV3Int());
        _isMoving = true;
    }

    public void Downgrade()
    {
        _lvl--;

        if (_lvl == 0)
            Destroy(gameObject);

        if(TryGetComponent(out TokenLVLSwitcher switcher))
            switcher.SwitchTexture(_lvl);
    }
}
