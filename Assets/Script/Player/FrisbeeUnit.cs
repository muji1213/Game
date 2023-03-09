using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FrisbeeUnit
{
    public void SetHP(int HP);

    public void ReduceLife();

    public void Accelerate();

    public void Gravity();

    public void Clamp();

    public void Invincible(int time);

    public void GetCoin(int score);

    public void PlayDamageEffect(Vector3 pos);
}
