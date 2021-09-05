using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(TokenDestroyer))]
public class DrawTokenSelector : MonoBehaviour
{
    [SerializeField] private FreeSpaceManager _spaceManager;

    private LineRenderer _selectLine;
    private TokenDestroyer _destroyer;

    private List<Token> _selectedTokens;

    private void Start()
    {
        _selectLine = GetComponent<LineRenderer>();
        _destroyer = GetComponent<TokenDestroyer>();

        _selectedTokens = new List<Token>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && GameState.IsPlayMode)
            SelectTokens();

        if (Input.GetMouseButtonUp(0) && GameState.IsPlayMode)
            ResetSelection();
    }

    private void SelectTokens()
    {
        if (!GetTokenUnderMouse(out var token))
            return;

        if(token.gameObject.TryGetComponent(out IBooster booster) && _selectedTokens.Count == 0)
        {
            _destroyer.DestroyBooster(booster, token);
            return;
        }

        if (TokenTypeCasting.IsObstacle(token.Type))
            return;

        if (_selectedTokens.Count == 0)
            _selectedTokens.Add(token);

        else
        {
            if (token.Type != _selectedTokens.Last().Type)
                return;

            var neighbourPoses = GridCalculator.GetNeighbourPos(token.GridPos);

            if (!_selectedTokens.Contains(token) && neighbourPoses.Contains(_selectedTokens.Last().GridPos))
                _selectedTokens.Add(token);

            else if(_selectedTokens.Contains(token))
            {
                int index = _selectedTokens.IndexOf(token);
                _selectedTokens.RemoveRange(index + 1, _selectedTokens.Count - index - 1);
            }
        }

        LinePointToToken(_selectedTokens.Count - 1, _selectedTokens.Last());
    }

    private void ResetSelection()
    {
        _selectLine.positionCount = 1;

        int selectedCount = _selectedTokens.Count;
        Vector2Int lastPos = new Vector2Int();

        if (selectedCount > 0)
            lastPos = _selectedTokens.Last().GridPos;

        _destroyer.Destroy(_selectedTokens);

        if (selectedCount >= 5 && selectedCount <= 7)
            _spaceManager.Spawner.SpawnRocket(lastPos);

        if (selectedCount > 7)
            _spaceManager.Spawner.SpawnBomb(lastPos);

        _spaceManager.Manage();
    }

    private bool GetTokenUnderMouse(out Token token)
    {
        token = null;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit;

        if (hit = Physics2D.Raycast(mousePos, Vector3.forward))
            if (hit.collider.gameObject.TryGetComponent(out token))
                return true;

        return false;
    }

    private void LinePointToToken(int index, Token token)
    {
        if (_selectLine.positionCount <= index)
            _selectLine.positionCount = index + 1;

        _selectLine.SetPosition(index, token.gameObject.transform.position);
    }
}
