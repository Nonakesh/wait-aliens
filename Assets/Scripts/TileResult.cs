public struct TileResult
{
    public bool IsOutside;
    public Building Building;

    public bool IsFree => !IsOutside && Building == null;
    
    public TileResult(bool isOutside) : this()
    {
        IsOutside = isOutside;
    }

    public TileResult(Building building)
    {
        IsOutside = false;
        Building = building;
    }
}