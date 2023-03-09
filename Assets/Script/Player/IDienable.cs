using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDienable<T>
{
    public IEnumerator Die(T type);
}
