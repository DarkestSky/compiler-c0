using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compiler_c0.instruction;
using ValueType = compiler_c0.symbol_manager.value_type.ValueType;

namespace compiler_c0.symbol_manager.symbol
{
    public class Function : Symbol
    {
        private static readonly SymbolManager SymbolManager = SymbolManager.Instance;
        
        public uint Name { get; set; }

        public uint ReturnSlot { get; protected set; }

        public ValueType ReturnType { get; set; }

        public uint ParamSlots { get; protected set; }

        public uint LocSlots { get; set; }

        public readonly List<ValueType> Params = new();

        private readonly List<Instruction> _instructions = new();

        private int _whileDepth;


        public bool CheckParamList(List<ValueType> paramList)
        {
            // first slot may be reserved for return slot;
            var returnSlot = ReturnType == ValueType.Void ? 0 : 1;
            
            if (paramList.Count != Params.Count - returnSlot)
                return false;

            for (var i = 0; i < paramList.Count; i++)
            {
                if (paramList[i] != Params[i + returnSlot])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// update loc_slot
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="Exception"></exception>
        public void AddVariable(ValueType type)
        {
            switch (type)
            {
                case ValueType.Int:
                case ValueType.Float:
                    LocSlots += 1;
                    break;
                default:
                    throw new Exception("invalid variable type");
            }
        }

        public void AddParam(ValueType type)
        {
            Params.Add(type);
            
            switch (type)
            {
                case ValueType.Int:
                case ValueType.Float:
                    ParamSlots += 1;
                    break;
                default:
                    throw new Exception("invalid variable type");
            }
        }
        
        public void SetReturnType(ValueType valueType)
        {
            ReturnType = valueType;
            switch (valueType)
            {
                case ValueType.Void:
                    ReturnSlot = 0;
                    break;
                case ValueType.Int:
                case ValueType.Float:
                    ReturnSlot = 1;
                    SymbolManager.SetReturnParam(valueType);
                    Params.Insert(0, ReturnType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueType), valueType, null);
            }
        }

        public void AddInstruction(Instruction instruction)
        {
            _instructions.Add(instruction);
        }

        public int GetInstructionOffset(Instruction instruction1, Instruction instruction2)
        {
            return _instructions.FindIndex(i => i == instruction2)
                   - _instructions.FindIndex(i => i == instruction1)
                   - 1;
        }

        public int GetInstructionOffset(Instruction instruction)
        {
            return _instructions.Count - _instructions.FindIndex(i => i == instruction) - 1;
        }

        public void EnterWhile()
        {
            _whileDepth += 1;
        }

        public void LeaveWhile()
        {
            _whileDepth -= 1;
            _continuePoint.RemoveAt(_continuePoint.Count - 1);
        }

        private List<Instruction> _continuePoint = new();
        public void SetContinuePoint(Instruction instruction)
        {
            _continuePoint.Add(instruction);
        }
        
        public void SetContinue()
        {
            if (_whileDepth == 0)
                throw new Exception("continue is not allowed here");
            var i = SymbolManager.AddInstruction(new Instruction(InstructionType.Br, 0));
            i.SetParam(SymbolManager.GetInstructionOffset(i, _continuePoint.Last()));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\tFunction:");
            sb.AppendLine($"\t\tName: {Name}");
            sb.AppendLine($"\t\tReturnSlot: {ReturnSlot}");
            sb.AppendLine($"\t\tParamSlot: {ParamSlots}");
            sb.AppendLine($"\t\tLocSlot: {LocSlots}");

            sb.AppendLine("\t\tInstructions:");
            foreach (var instruction in _instructions)
            {
                sb.AppendLine($"\t\t\t{instruction}");
            }

            return sb.ToString();
        }

        public IEnumerable<byte> ToBytes()
        {
            var nameByte = BitConverter.GetBytes(Name).Reverse();
            var returnByte = BitConverter.GetBytes(ReturnSlot).Reverse();
            var paramByte = BitConverter.GetBytes(ParamSlots).Reverse();
            var locByte = BitConverter.GetBytes(LocSlots).Reverse();

            var bodyByte = BitConverter.GetBytes(_instructions.Count).Reverse();
            foreach (var instruction in _instructions)
            {
                bodyByte = bodyByte.Concat(instruction.ToBytes());
            }

            return Enumerable.Empty<byte>()
                .Concat(nameByte)
                .Concat(returnByte)
                .Concat(paramByte)
                .Concat(locByte)
                .Concat(bodyByte);
        }
    }
}