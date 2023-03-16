using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that can get damaged
/// </summary>
public interface IDamagable 
{
    LayerMask DamagableLayer { get; }
    int Health { get;}
    void OnDamage();

     
}