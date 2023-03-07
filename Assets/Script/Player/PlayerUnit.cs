using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerUnit
{
    public void Jump();

    public void JumpDown();

    public void Shoot();

    public void Gravity();

    public void SetAnimation(bool boolean);

    public void PlayCoinEffect();

    public void LifeUP();

    public int CaluculateFrisbeeHP(int runTime);
}
