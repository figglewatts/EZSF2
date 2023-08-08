using System.IO;

namespace EZSF2.Soundfont.Chunk
{
    public class GenAmountType
    {
        protected ushort _backing;

        public GenAmountType(BinaryReader br)
        {
            _backing = br.ReadUInt16();
        }

        public ushort UnsignedValue
        {
            get => _backing;
            set => _backing = value;
        }

        public short Value
        {
            get => (short)_backing;
            set => _backing = (ushort)value;
        }

        public ushort LoByte
        {
            get => (ushort)(_backing & 0xFF);
            set => _backing = (ushort)((_backing & ~0xFF) | value);
        }

        public ushort HiByte
        {
            get => (ushort)(_backing >> 8);
            set => _backing = (ushort)((_backing & ~0xFF00) | value << 8);
        }
    }
}
