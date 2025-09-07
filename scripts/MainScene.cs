using Godot;
using System;

public partial class MainScene : Node2D
{
    [Export]
    public MemoryMap MainMemory;
    [Export]
    public CPU CPU;

    public override void _Ready()
    {
        MainMemory = new MemoryMap();
        CPU = new CPU(MainMemory);
        CPU.PrintAllRegisters();
        CPU.Fetch();
        CPU.Execute();
        CPU.PrintAllRegisters();
    }
}
