using Godot;

public partial class MemoryMap : GodotObject
{
    public byte[] Memory;

    public MemoryMap()
    {
        Memory = new byte[65536];
    }
}
