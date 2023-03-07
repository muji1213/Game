using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerUnit
{
    public void Jump();

    public void Shoot();

    public void Gravity();

    public void SetAnimation();

    public void PlayDieAnim(int type);

    public void PlayCoinEffect();

    public void LifeUP();

    public int CaluculateFrisbeeHP(int runTime);
}
