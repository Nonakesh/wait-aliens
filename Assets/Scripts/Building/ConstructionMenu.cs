using System;
using System.Collections;
using System.Collections.Generic;
using PathFind;
using UnityEngine;

public class ConstructionMenu : MonoBehaviour
{
    public Material ValidPlacementMaterial;
    public Material InvalidPlacementMaterial;

    public BuildingCost[] Buildings;

    private BuildingCost currentBuilding;

    private void Update()
    {
        if (!TimeManager.Paused)
        {
            currentBuilding = null;
            return;
        }
        
        for (int i = 0; i <= 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                StartPlacingBuilding(i);
            }
        }

        PlaceBuilding();
    }

    public void StartPlacingBuilding(int index)
    {
        if (index < 0 || index >= Buildings.Length)
        {
            throw new ArgumentOutOfRangeException();
        }

        currentBuilding = Buildings[index];
    }

    private void PlaceBuilding()
    {
        if (currentBuilding == null)
        {
            return;
        }

        // Right click aborts
        if (Input.GetMouseButtonDown(1))
        {
            currentBuilding = null;
            return;
        }

        // Get the buildings position on the plane
        var (isValid, point, pos) = MousePosToBuildingCenter(currentBuilding.Building, Input.mousePosition);

        if (!isValid)
        {
            return;
        }

        // Draw the buildings grid
        bool buildingPlacementValid = true;
        for (int x = 0; x < currentBuilding.Building.Width; x++)
        {
            for (int y = 0; y < currentBuilding.Building.Length; y++)
            {
                var testPoint = new Point(point.X + x, point.Y + y);
                var testPos = GameGrid.Instance.PointToPosition(testPoint);

                var isFree = GameGrid.Instance.GetTile(testPoint).IsBuildable;
                buildingPlacementValid &= isFree;

                // Ugly direct mode! :D
                var material = isFree ? ValidPlacementMaterial : InvalidPlacementMaterial;

                var scale = GameGrid.Instance.Scale;
                var mesh = new Mesh
                {
                    vertices = new[]
                    {
                        testPos,
                        testPos + Vector3.forward * scale,
                        testPos + Vector3.right * scale + Vector3.forward * scale,
                        testPos + Vector3.right * scale,
                    },
                    triangles = new[]
                    {
                        0, 1, 2,
                        0, 2, 3
                    }
                };

                Graphics.DrawMesh(mesh, Matrix4x4.identity, material, 0);
            }
        }

        // Left click to build
        if (buildingPlacementValid && Input.GetMouseButtonDown(0))
        {
            // If there are enough resources, build the building, otherwise do nothing
            if (ResourceManager.TryTakeResources(currentBuilding.Costs))
            {
                Instantiate(currentBuilding.Building, pos, Quaternion.identity);
                currentBuilding = null;
            }
        }
    }

    private (bool, Point, Vector3) MousePosToBuildingCenter(Building building, Vector3 mousePos)
    {
        var plane = new Plane(Vector3.up, Vector3.zero);

        float planeDist;
        var ray = Camera.main.ScreenPointToRay(mousePos);
        if (plane.Raycast(ray, out planeDist))
        {
            var planePos = ray.origin + ray.direction * planeDist;

//            planePos.x -= building.Width * 0.5f * GameGrid.Instance.Scale;
//            planePos.z -= building.Length * 0.5f * GameGrid.Instance.Scale;

            var gridPoint = GameGrid.Instance.PositionToPoint(planePos);
            return (true, gridPoint, GameGrid.Instance.PointToPosition(gridPoint));
        }

        return (false, null, Vector3.zero);
    }
}