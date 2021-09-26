using System.Collections.Generic;
using UnityEngine;

public static class TargetsPool
{
    public static HashSet<ITarget> Targets { get; private set; } = new HashSet<ITarget>();
}
