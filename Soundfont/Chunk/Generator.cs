using System.IO;

namespace EZSF2.Soundfont.Chunk
{
    public class Generator : IChunkElement
    {
        public const int Size = 2 * 2;

        public GeneratorType Type { get; set; }
        public GenAmountType Amount { get; set; }

        public IChunkElement FromBinaryReader(BinaryReader br)
        {
            Type = (GeneratorType)br.ReadUInt16();
            Amount = new GenAmountType(br);
            return this;
        }
    }
}
