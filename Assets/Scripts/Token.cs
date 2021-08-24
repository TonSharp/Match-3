using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] private bool _isMoveable;

    public Vector2Int GridPos { get; set; }
}
