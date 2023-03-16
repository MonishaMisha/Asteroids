using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that can break in to pieces
/// </summary>
public interface IBreakable 
{
    LayerMask BreakableLayer { get; }
    bool IsBreakable { get; }
    void OnBreakage();

}
