using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Arrow", menuName = "Arrow")]
public class ArrowCategory : ScriptableObject
{
    public int _transparent;
    public int _reloadTime;
    public int _speed;
    public int _ropeLength;
    public int _oxygenComsuming;

}
