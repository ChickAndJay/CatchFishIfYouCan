using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish")]
public class FishCategory : ScriptableObject
{
    public int _category;
    public Sprite _sprite;
    public int _hp;
    public int _speed;
    public int _gold;
    public int _jellyFishDamage;
    public float _standardSize;
    public float _maxSize;
}
