using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITokenTarget
{
    public string GetTokenType();
    public bool IsReady { get; set; }
}
