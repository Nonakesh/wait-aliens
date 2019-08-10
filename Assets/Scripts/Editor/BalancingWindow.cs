using System.Linq;
using UnityEditor;
using UnityEngine;

public class BalancingWindow : EditorWindow
{
    private Health[] healths;
    private bool[] foldout;

    private Vector2 scroll;
    
    [MenuItem("Window/Balancing Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        BalancingWindow window = (BalancingWindow) GetWindow(typeof(BalancingWindow));
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Update"))
        {
            healths = AssetDatabase.FindAssets("t:Prefab")
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Select(x => AssetDatabase.LoadAssetAtPath<Health>(x))
                .Where(x => x != null)
                .ToArray();

            foldout = new bool[healths.Length];
        }

        EditorGUILayout.Space();

        scroll = EditorGUILayout.BeginScrollView(scroll);
        for (var i = 0; i < healths.Length; i++)
        {
            var health = healths[i];
            foldout[i] = EditorGUILayout.Foldout(foldout[i], health.name);

            using (new EditorGUI.IndentLevelScope(2))
            {
                if (foldout[i])
                {
                    EditorGUILayout.LabelField("Health", EditorStyles.boldLabel);
                    health.MaxHealth = EditorGUILayout.FloatField("Health", health.MaxHealth);
                    health.RegenerationPerSecond = EditorGUILayout.FloatField("RegenerationPerSecond", health.RegenerationPerSecond);
                    foreach (var resourceDrop in health.Drops)
                    {
                        EditorGUILayout.EnumPopup("Type", resourceDrop.Type);
                        resourceDrop.Amount = EditorGUILayout.IntField("Amount", resourceDrop.Amount);
                    }
                    EditorGUILayout.Space();
                    
                    var movement = health.GetComponent<FloorMovement>();
                    if (movement != null)
                    {
                        EditorGUILayout.LabelField("FloorMovement", EditorStyles.boldLabel);
                        movement.MovementSpeed = EditorGUILayout.FloatField("MovementSpeed", movement.MovementSpeed);
                        EditorGUILayout.Space();
                    }

                    var generator = health.GetComponent<ResourceGenerator>();
                    if (generator != null)
                    {
                        EditorGUILayout.LabelField("ResourceGenerator", EditorStyles.boldLabel);
                        generator.Type = (ResourceType) EditorGUILayout.EnumPopup("Type", generator.Type);
                        generator.ResourceDrop = EditorGUILayout.IntField("ResourceDrop", generator.ResourceDrop);
                        generator.TickDuration = EditorGUILayout.FloatField("TickDuration", generator.TickDuration);
                        EditorGUILayout.Space();
                    }
                    
                    foreach (var turret in health.GetComponentsInChildren<TurretBehaviour>())
                    {
                        EditorGUILayout.LabelField("TurretBehaviour", EditorStyles.boldLabel);
                        turret.Damage = EditorGUILayout.FloatField("Damage", turret.Damage);
                        turret.RateOfFire = EditorGUILayout.FloatField("RateOfFire", turret.RateOfFire);
                        turret.ViewDistance = EditorGUILayout.FloatField("ViewDistance", turret.ViewDistance);
                        turret.RotationSpeed = EditorGUILayout.FloatField("RotationSpeed", turret.RotationSpeed);
                        EditorGUILayout.Space();
                    }
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }
}