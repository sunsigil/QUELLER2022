using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellcaster
{
    void Spawn();
    void Charge(float amount);
    void Cast();
}
