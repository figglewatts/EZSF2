using System.IO;

namespace EZSF2.Soundfont.Chunk
{
    public class Modulator : IChunkElement
    {
        public const int Size = 5 * 2;

        public ModulatorType ModulatorType { get; set; }
        public GeneratorType GeneratorType { get; set; }
        public short Amount { get; set; }
        public ModulatorType AmountModulatorType { get; set; }
        public TransformType TransformType { get; set; }

        public IChunkElement FromBinaryReader(BinaryReader br)
        {
            ModulatorType = (ModulatorType)br.ReadUInt16();
            GeneratorType = (GeneratorType)br.ReadUInt16();
            Amount = br.ReadInt16();
            AmountModulatorType = (ModulatorType)br.ReadUInt16();
            TransformType = (TransformType)br.ReadUInt16();
            return this;
        }

        public bool IsZero() => ModulatorType == 0 && GeneratorType == 0 && Amount == 0 && AmountModulatorType == 0 &&
                                TransformType == 0;
    }
}
