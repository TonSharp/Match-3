using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTargetManager : MonoBehaviour
{
    [SerializeField] private GameObject confirmedImage;
    
    public void Confirm()
    {
        confirmedImage.SetActive(true);
    }
}
