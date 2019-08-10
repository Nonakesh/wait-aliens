namespace PathFind
{
    /**
    * A node in the grid map
    */
    public class Node
    {
        // node starting params
        public bool Walkable;
        public int GridX;
        public int GridY;
        public float Penalty;

        // calculated values while finding path
        public int GCost;
        public int HCost;
        public Node Parent;

        // create the node
        // _price - how much does it cost to pass this tile. less is better, but 0.0f is for non-walkable.
        // _gridX, _gridY - tile location in grid.
        public Node(float price, int gridX, int gridY)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            Walkable = price != 0.0f;
            Penalty = price;
            GridX = gridX;
            GridY = gridY;
        }

        public int FCost => GCost + HCost;
    }
}