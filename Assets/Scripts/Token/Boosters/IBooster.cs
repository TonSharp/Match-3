using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
    public interface IBooster
    {
        public Func<Vector2Int, Tilemap, List<Token>> Activate();
    }
}
