/**
* Provide simple path-finding algorithm with support in penalties.
* Heavily based on code from this tutorial: https://www.youtube.com/watch?v=mZfyt03LDH4
* This is just a Unity port of the code from the tutorial + option to set penalty + nicer API.
*
* Original Code author: Sebastian Lague.
* Modifications & API by: Ronen Ness.
* Since: 2016.
*/
using UnityEngine;
using System.Collections.Generic;

namespace PathFind
{
    /**
    * Main class to find the best path from A to B.
    * Use like this:
    * Grid grid = new Grid(width, height, tiles_costs);
    * List<Point> path = Pathfinding.FindPath(grid, from, to);
    */
    public class Pathfinding
    {
        // The API you should use to get path
        // grid: grid to search in.
        // startPos: starting position.
        // targetPos: ending position.
        public static List<Point> FindPath(Grid grid, Point startPos, Point targetPos)
        {
            // find path
            List<Node> nodesPath = _ImpFindPath(grid, startPos, targetPos);

            // convert to a list of points and return
            List<Point> ret = new List<Point>();
            if (nodesPath != null)
            {
                foreach (Node node in nodesPath)
                {
                    ret.Add(new Point(node.GridX, node.GridY));
                }
            }
            return ret;
        }

        // internal function to find path, don't use this one from outside
        private static List<Node> _ImpFindPath(Grid grid, Point startPos, Point targetPos)
        {
            Node startNode = grid.Nodes[startPos.X, startPos.Y];
            Node targetNode = grid.Nodes[targetPos.X, targetPos.Y];

            List<Node> openList = new List<Node>();
            HashSet<Node> openSet = new HashSet<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openList.Add(startNode);
            openSet.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(grid, startNode, targetNode);
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour) * (int)(10.0f * neighbour.Penalty);
                    var isNeighborInOpenList = openSet.Contains(neighbour);
                    if (newMovementCostToNeighbour < neighbour.GCost || !isNeighborInOpenList)
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!isNeighborInOpenList)
                        {
                            openList.Add(neighbour);
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return null;
        }

        private static List<Node> RetracePath(Grid grid, Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

        private static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }

            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

}