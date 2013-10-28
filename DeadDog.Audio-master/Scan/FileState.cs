namespace DeadDog.Audio.Scan
{
    public enum FileState
    {
        Added = 0x01,
        Updated = 0x02,
        Error = 0x04,
        AddError = 0x0C,
        UpdateError = 0x14,
        Removed = 0x20,
        Skipped = 0x40
    }
}