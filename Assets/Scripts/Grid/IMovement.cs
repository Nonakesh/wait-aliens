using System.Collections.Generic;
using PathFind;
using UnityEngine;

public interface IMovement
{
    Point Target { get; set; }
    
    Health Health { get; }
    
    List<Point> Path { get; set; }
}