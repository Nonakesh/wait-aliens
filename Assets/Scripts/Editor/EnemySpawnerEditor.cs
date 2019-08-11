using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var enemyTypes = serializedObject.FindProperty(nameof(EnemySpawner.EnemyTypes));
        EditorGUILayout.PropertyField(enemyTypes, true);

        var enemyNames = new string[enemyTypes.arraySize];
        for (int i = 0; i < enemyNames.Length; i++)
        {
            enemyNames[i] = enemyTypes.GetArrayElementAtIndex(i).objectReferenceValue.name;
        }
        
        EditorExtensions.CustomArrayProperty(serializedObject.FindProperty(nameof(EnemySpawner.Waves)), (p, i) => WaveEditor(p, i, enemyTypes.arraySize, enemyNames));
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(EnemySpawner.DifficultyIncreaseTime)), true);
        
        serializedObject.ApplyModifiedProperties();
    }

    private void WaveEditor(SerializedProperty property, int index, int enemyCount, string[] enemyNames)
    {
        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName);
        
        if (property.isExpanded)
        {
            using (new EditorGUI.IndentLevelScope(1))
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(EnemyWave.Difficulty)), true);
        
                EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(EnemyWave.SpawnTime)), true);
        
                EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(EnemyWave.SpawnAmount)), true);
        
                EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(EnemyWave.WaveCooldown)), true);
        
                EditorExtensions.FixedSizeArray(property.FindPropertyRelative(nameof(EnemyWave.Enemies)), enemyCount, enemyNames);
            }
        }
    }
}
