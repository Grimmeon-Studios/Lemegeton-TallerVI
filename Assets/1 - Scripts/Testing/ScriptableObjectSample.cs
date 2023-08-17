using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName ="Create New Card")]
public class ScriptableObjectSample : ScriptableObject
{
    public Sprite image;
    public int attack;
    public string description;

    public float Damage(float attackE)
    {
        return attack;
    }
}
