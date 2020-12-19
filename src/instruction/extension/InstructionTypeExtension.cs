using System;
using System.Collections.Generic;

namespace compiler_c0.instruction.extension
{
    public static class InstructionTypeExtension
    {
        public static int GetParamSize(this InstructionType type)
        {
            if (NoParamInstructionTypes.Contains(type))
            {
                return 0;
            }

            if (U32ParamInstructionTypes.Contains(type) || I32ParamInstructionTypes.Contains(type))
            {
                return 1;
            }

            if (U64ParamInstructionTypes.Contains(type))
            {
                return 2;
            }

            throw new Exception("unexpected instruction type");
        }

        public static bool IsSigned(this InstructionType type)
        {
            return I32ParamInstructionTypes.Contains(type);
        }

        private static readonly HashSet<InstructionType> NoParamInstructionTypes = new()
        {
            InstructionType.Nop,
            InstructionType.Pop,
            InstructionType.Dup,
            InstructionType.Load8,
            InstructionType.Load16,
            InstructionType.Load32,
            InstructionType.Load64,
            InstructionType.Store8,
            InstructionType.Store16,
            InstructionType.Store32,
            InstructionType.Store64,
            InstructionType.Alloc,
            InstructionType.Free,
            InstructionType.AddI,
            InstructionType.SubI,
            InstructionType.MulI,
            InstructionType.DivI,
            InstructionType.AddF,
            InstructionType.SubF,
            InstructionType.MulF,
            InstructionType.DivF,
            InstructionType.DivU,
            InstructionType.Shl,
            InstructionType.Shr,
            InstructionType.And,
            InstructionType.Or,
            InstructionType.Xor,
            InstructionType.Not,
            InstructionType.CmpI,
            InstructionType.CmpU,
            InstructionType.CmpF,
            InstructionType.NegI,
            InstructionType.NegF,
            InstructionType.ItoF,
            InstructionType.FtoI,
            InstructionType.Shrl,
            InstructionType.SetLt,
            InstructionType.SetGt,
            InstructionType.Ret,
            InstructionType.ScanI,
            InstructionType.ScanC,
            InstructionType.ScanF,
            InstructionType.PrintI,
            InstructionType.PrintC,
            InstructionType.PrintF,
            InstructionType.PrintS,
            InstructionType.Println,
            InstructionType.Panic,
        };

        private static readonly HashSet<InstructionType> U32ParamInstructionTypes = new()
        {
            InstructionType.Popn,
            InstructionType.Loca,
            InstructionType.Arga,
            InstructionType.Globa,
            InstructionType.StackAlloc,
            InstructionType.Call,
            InstructionType.Callname,
        };

        private static readonly HashSet<InstructionType> I32ParamInstructionTypes = new()
        {
            InstructionType.Br,
            InstructionType.BrFalse,
            InstructionType.BrTrue,
        };

        private static readonly HashSet<InstructionType> U64ParamInstructionTypes = new()
        {
            InstructionType.Push,
        };
    }
}