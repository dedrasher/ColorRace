public struct Skin
{
    public const int –°heapestPrice = 100;
    public readonly SkinData skinData;
    public bool isPurchased;
    public Skin(SkinData skinData, bool isPurchased)
    {
        this.skinData = skinData;
        this.isPurchased = isPurchased;
    }
}
