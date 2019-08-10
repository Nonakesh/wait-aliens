public struct TileResult
{
    public bool IsWalkable;
    public bool IsOutside;
    public Building Building;
    public int UnitCount;

    public bool IsBuildable => !IsOutside && Building == null && UnitCount == 0;

    public TileResult(bool isOutside) : this()
    {
        IsOutside = isOutside;
        IsWalkable = false;
        Building = null;
    }

    public TileResult(Building building, bool isWalkable, int unitCount)
    {
        IsOutside = false;
        Building = building;
        IsWalkable = isWalkable;
        UnitCount = unitCount;
    }
}