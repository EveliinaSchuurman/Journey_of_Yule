using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompanionType { SNOWMAN, ELF, GINGERBREAD_MAN }

[CreateAssetMenu(fileName = "New Companion", menuName = "Scriptable Objects/Companion")]
public class Companion : ScriptableObject
{
    public CompanionType coType;
    public Sprite coPicture;
    public string coName;
    public float coPower;
    public int coMaxHearts;
}
