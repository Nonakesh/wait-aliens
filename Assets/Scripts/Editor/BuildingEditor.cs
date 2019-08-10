using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    private void OnSceneGUI()
    {
        var building = target as Building;
        var startPos = building.transform.position;

        float scale;
        if (GameGrid.Instance == null)
        {
            scale = FindObjectOfType<GameGrid>().Scale;
        }
        else
        {
            scale = GameGrid.Instance.Scale;
        }

        Handles.DrawLines(new[]
        {
            startPos,
            startPos + scale * building.Length * Vector3.forward,
            
            startPos + scale * building.Length * Vector3.forward,
            startPos + scale * building.Width * Vector3.right + scale * building.Length * Vector3.forward,
            
            startPos + scale * building.Width * Vector3.right + scale * building.Length * Vector3.forward,
            startPos + scale * building.Width * Vector3.right,
            
            startPos + scale * building.Width * Vector3.right,
            startPos
        });
    }
}