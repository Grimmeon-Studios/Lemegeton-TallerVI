using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGeneration), true)]
public class RandonDungeonGenEditor : Editor
{
    AbstractDungeonGeneration generator;

    private void Awake()
    {
        generator = (AbstractDungeonGeneration)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
