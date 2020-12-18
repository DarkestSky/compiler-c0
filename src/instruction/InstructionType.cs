namespace compiler_c0.instruction
{
    public enum InstructionType : byte
    {
        // NoParam
        Nop = 0x00,
        Pop = 0x02,
        Dup = 0x04,

        Load8 = 0x10,
        Load16 = 0x11,
        Load32 = 0x12,
        Load64 = 0x13,
        Store8 = 0x14,
        Store16 = 0x15,
        Store32 = 0x16,
        Store64 = 0x17,

        Alloc = 0x18,
        Free = 0x19,

        AddI = 0x20,
        SubI = 0x21,
        MulI = 0x22,
        DivI = 0x23,
        AddF = 0x24,
        SubF = 0x25,
        MulF = 0x26,
        DivF = 0x27,
        DivU = 0x28,

        Shl = 0x29,
        Shr = 0x2A,
        And = 0x2B,
        Or = 0x2C,
        Xor = 0x2D,
        Not = 0x2E,

        CmpI = 0x30,
        CmpU = 0x31,
        CmpF = 0x32,
        NegI = 0x33,
        NegU = 0x34,
        NegF = 0x35,

        ItoF = 0x36,
        FtoI = 0x37,
        Shrl = 0x38,
        SetLt = 0x39,
        SetGt = 0x3A,

        ScanI = 0x50,
        ScanC = 0x51,
        ScanF = 0x52,

        PrintI = 0x54,
        PrintC = 0x55,
        PrintF = 0x56,
        PrintS = 0x57,
        Println = 0x58,

        Panic = 0xFE,

        // U32Param
        Popn = 0x03,
        Loca = 0x0A,
        Arga = 0x0B,
        Globa = 0x0C,
        StackAlloc = 0x1A,
        Br = 0x41,
        BrFalse = 0x42,
        BrTrue = 0x43,
        Call = 0x48,
        Ret = 0x49,
        Callname = 0x4A,

        //U64Param
        Push = 0x01
    }
}