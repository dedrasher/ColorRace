public struct Skin
{
    public readonly SkinData skinData;
    public bool isPurchased;
    public Skin(SkinData skinData, bool isPurchased)
    {
        this.skinData = skinData;
        this.isPurchased = isPurchased;
    }
}
