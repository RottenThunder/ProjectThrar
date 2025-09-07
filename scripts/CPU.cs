using Godot;

public partial class CPU : GodotObject
{
    public ushort[] Registers;
    public ushort MAR;
    public ushort CIR;
    public MemoryMap MemoryMap;

    public CPU(MemoryMap memoryMap)
    {
        Registers = new ushort[16];
        MAR = 0;
        CIR = 0;
        MemoryMap = memoryMap;
    }

    private void ExecuteSTP(ushort exitCode)
    {
        GD.Print("CPU has Stopped with Exit Code: ", exitCode);
    }

    private void ExecuteADD1(ushort rD, ushort rN, ushort rM)
    {
        Registers[rD] = (ushort)(Registers[rN] + Registers[rM]);
    }

    private void ExecuteADD2(ushort rD, ushort imm8)
    {
        Registers[rD] = (ushort)(Registers[rD] + imm8);
    }

    private void ExecuteSUB1(ushort rD, ushort rN, ushort rM)
    {
        Registers[rD] = (ushort)(Registers[rN] - Registers[rM]);
    }

    private void ExecuteSUB2(ushort rD, ushort imm8)
    {
        Registers[rD] = (ushort)(Registers[rD] - imm8);
    }

    private void ExecuteMOV1(ushort rD, ushort imm8)
    {
        Registers[rD] = imm8;
    }

    private void ExecuteCMP1(ushort rN, ushort imm8)
    {
        GD.Print("Comparing Value in Register ", rN, " (", Registers[rN], ") with immediate value ", imm8);
    }

    private void ExecuteLDR(ushort rD, ushort rA, ushort imm4)
    {
        ushort address = (ushort)(Registers[rA] + (imm4 * 2));
        byte byte0 = MemoryMap.Memory[address];
        byte byte1 = MemoryMap.Memory[address + 1];
        Registers[rD] = 0;
        Registers[rD] |= byte1;
        Registers[rD] <<= 8;
        Registers[rD] |= byte0;
    }

    private void ExecuteSTR(ushort rN, ushort rA, ushort imm4)
    {
        ushort address = (ushort)(Registers[rA] + (imm4 * 2));
        byte byte0 = (byte)(Registers[rN] & 0x00FF);
        byte byte1 = (byte)((Registers[rN] & 0xFF00) >> 8);
        MemoryMap.Memory[address] = byte0;
        MemoryMap.Memory[address + 1] = byte1;
    }

    private void ExecuteNAN(ushort rD, ushort rN, ushort rM)
    {
        Registers[rD] = (ushort)(Registers[rN] & Registers[rM]);
        Registers[rD] = (ushort)~Registers[rD];
    }

    private void ExecuteNOR(ushort rD, ushort rN, ushort rM)
    {
        Registers[rD] = (ushort)(Registers[rN] | Registers[rM]);
        Registers[rD] = (ushort)~Registers[rD];
    }

    private void ExecuteLSL(ushort rD, ushort rN, ushort imm4)
    {
        Registers[rD] = (ushort)(Registers[rN] << (imm4 + 1));
    }

    private void ExecuteLSR(ushort rD, ushort rN, ushort imm4)
    {
        Registers[rD] = (ushort)(Registers[rN] >> (imm4 + 1));
    }

    private void ExecuteBRANCH0(ushort cond, ushort rA, ushort imm3)
    {

    }

    private void ExecuteBRANCH1(ushort cond, ushort imm7)
    {

    }

    private void ExecuteAND(ushort rD, ushort rN)
    {
        Registers[rD] = (ushort)(Registers[rD] & Registers[rN]);
    }

    private void ExecuteORR(ushort rD, ushort rN)
    {
        Registers[rD] = (ushort)(Registers[rD] | Registers[rN]);
    }

    private void ExecuteXOR(ushort rD, ushort rN)
    {
        Registers[rD] = (ushort)(Registers[rD] ^ Registers[rN]);
    }

    private void ExecuteNOT(ushort rD, ushort rN)
    {
        Registers[rD] = (ushort)~Registers[rN];
    }

    private void ExecuteLDB(ushort rD, ushort rA)
    {
        ushort address = Registers[rA];
        byte byte0 = MemoryMap.Memory[address];
        Registers[rD] = 0;
        Registers[rD] |= byte0;
    }

    private void ExecuteSTB(ushort rN, ushort rA)
    {
        ushort address = Registers[rA];
        byte byte0 = (byte)(Registers[rN] & 0x00FF);
        MemoryMap.Memory[address] = byte0;
    }

    private void ExecuteMOV2(ushort rD, ushort rN)
    {
        Registers[rD] = Registers[rN];
    }

    private void ExecuteCMP2(ushort rN, ushort rM)
    {
        GD.Print("Comparing Value in Register ", rN, " (", Registers[rN], ") with Value in Register ", rM, " (", Registers[rM], ")");
    }

    private void ExecuteINT(ushort cond, ushort interruptIndex)
    {

    }

    public void PrintAllRegisters()
    {
        GD.Print("R0:       ", Registers[0]);
        GD.Print("R1:       ", Registers[1]);
        GD.Print("R2:       ", Registers[2]);
        GD.Print("R3:       ", Registers[3]);
        GD.Print("R4:       ", Registers[4]);
        GD.Print("R5:       ", Registers[5]);
        GD.Print("R6:       ", Registers[6]);
        GD.Print("R7:       ", Registers[7]);
        GD.Print("R8:       ", Registers[8]);
        GD.Print("R9:       ", Registers[9]);
        GD.Print("R10:      ", Registers[10]);
        GD.Print("R11:      ", Registers[11]);
        GD.Print("R12:      ", Registers[12]);
        GD.Print("R13 (LR): ", Registers[13]);
        GD.Print("R14 (FR): ", Registers[14]);
        GD.Print("R15 (PC): ", Registers[15]);
        GD.Print("MAR:      ", MAR);
        GD.Print("CIR:      ", CIR);
        GD.Print("--------------------------------");
        GD.Print("");
    }

    public void Fetch()
    {
        MAR = Registers[15];
        byte byte0 = MemoryMap.Memory[MAR];
        byte byte1 = MemoryMap.Memory[MAR + 1];
        CIR = 0;
        CIR |= byte1;
        CIR <<= 8;
        CIR |= byte0;
        Registers[15] += 2;
    }

    public void Execute()
    {
        ushort opcode = (ushort)(CIR & 0xF000);
        opcode >>= 12;
        ushort secondaryOpcode = (ushort)(CIR & 0x0F00);
        secondaryOpcode >>= 8;

        switch (opcode)
        {
            case 0:
                ExecuteSTP((ushort)(CIR & 0x0FFF));
                break;
            case 1:
                ExecuteADD1((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 2:
                ExecuteADD2((ushort)((CIR & 0x0F00) >> 8), (ushort)(CIR & 0x00FF));
                break;
            case 3:
                ExecuteSUB1((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 4:
                ExecuteSUB2((ushort)((CIR & 0x0F00) >> 8), (ushort)(CIR & 0x00FF));
                break;
            case 5:
                ExecuteMOV1((ushort)((CIR & 0x0F00) >> 8), (ushort)(CIR & 0x00FF));
                break;
            case 6:
                ExecuteCMP1((ushort)((CIR & 0x0F00) >> 8), (ushort)(CIR & 0x00FF));
                break;
            case 7:
                ExecuteLDR((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 8:
                ExecuteSTR((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 9:
                ExecuteNAN((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 10:
                ExecuteNOR((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 11:
                ExecuteLSL((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 12:
                ExecuteLSR((ushort)((CIR & 0x0F00) >> 8), (ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                break;
            case 13:
                if (secondaryOpcode <= 7)
                    ExecuteBRANCH0((ushort)((CIR & 0x0780) >> 7), (ushort)((CIR & 0x0078) >> 3), (ushort)(CIR & 0x0007));
                else
                    ExecuteBRANCH1((ushort)((CIR & 0x0780) >> 7), (ushort)(CIR & 0x007F));
                break;
            case 14:
                switch (secondaryOpcode)
                {
                    case 0:
                        ExecuteAND((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 1:
                        ExecuteORR((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 2:
                        ExecuteXOR((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 3:
                        ExecuteNOT((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 4:
                        ExecuteLDB((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 5:
                        ExecuteSTB((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 6:
                        ExecuteMOV2((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 7:
                        ExecuteCMP2((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 8:
                        ExecuteAND((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 9:
                        ExecuteORR((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 10:
                        ExecuteXOR((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 11:
                        ExecuteNOT((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 12:
                        ExecuteLDB((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 13:
                        ExecuteSTB((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 14:
                        ExecuteMOV2((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    case 15:
                        ExecuteCMP2((ushort)((CIR & 0x00F0) >> 4), (ushort)(CIR & 0x000F));
                        break;
                    default:
                        break;
                }
                break;
            case 15:
                ExecuteINT((ushort)((CIR & 0x0F00) >> 8), (ushort)(CIR & 0x00FF));
                break;
            default:
                break;
        }
    }
}