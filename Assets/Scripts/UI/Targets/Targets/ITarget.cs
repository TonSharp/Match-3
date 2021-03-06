using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    public TargetType GetTargetType();
    public string Serialize();

    public bool IsReady { get; set; }
}
