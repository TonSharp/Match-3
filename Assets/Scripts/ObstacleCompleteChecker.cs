using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCompleteChecker : MonoBehaviour
{
    [SerializeField] private GameObject confirmIco;

    public void Compele()
    {
        confirmIco.SetActive(true);
    }
    public void Reset()
    {
        confirmIco.SetActive(false);
    }
}
