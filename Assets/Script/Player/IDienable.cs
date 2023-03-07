using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDienable
{
    public IEnumerator Die(int type);
}
