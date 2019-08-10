using PathFind;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameGrid))]
public class GameGridEditor : Editor
{
    private void OnSceneGUI()
    {
        var grid = target as GameGrid;

        var list = grid.GetGridLines();

        Handles.color = new Color(1, 1, 1, 0.3f);
        Handles.DrawDottedLines(list, 2);

        // If the game is running
        if (!Application.IsPlaying(grid))
        {
            return;
        }
        
        for (int x = 0; x < grid.Length; x++)
        {
            for (int y = 0; y < grid.Width; y++)
            {
                var point = new Point(x, y);
                
                if (grid.GetTile(point).Building != null)
                {
                    var pos = grid.PointToPosition(point);
                    
                    Handles.color = new Color(1, 0, 0, 0.1f);
                    Handles.DrawAAConvexPolygon(
                        pos, 
                        pos + Vector3.forward * grid.Scale, 
                        pos + Vector3.right * grid.Scale + Vector3.forward * grid.Scale, 
                        pos + Vector3.right * grid.Scale);
                }
            }
        }
    }
}
