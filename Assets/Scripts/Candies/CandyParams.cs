using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu]
public class CandyParams : ScriptableObject
{
    public float chanse;

    public int candyId;

    public Sprite candySprite,
        blurSprite;
}
