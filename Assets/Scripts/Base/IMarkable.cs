using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for marking funtionality
/// </summary>
public interface IMarkable 
{
    bool IsMarked { get; }
    void Mark();
    void UnMark();
}
