using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenLVLSwitcher : MonoBehaviour
{
    [SerializeField] private Sprite _first, _second;

    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SwitchTexture(int lvl)
    {
        switch(lvl)
        {
            case 1:
                _renderer.sprite = _first;
                break;

            case 2:
                _renderer.sprite = _second;
                break;

            default:
                _renderer.sprite = null;
                break;
        }
    }
}
