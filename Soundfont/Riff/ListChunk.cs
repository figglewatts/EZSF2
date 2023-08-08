using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace EZSF2.Soundfont.Riff
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ListChunk
    {
        public const uint HeaderLength = 4 + 4 + 4; // length of ID + Size + Format

        public string ID { get; set; }
        public uint Size { get; set; }
        public string Format { get; set; }

        public Dictionary<string, DataChunk> SubChunks { get; set; }

        public ListChunk Parse(BinaryReader br)
        {
            ID = Encoding.ASCII.GetString(br.ReadBytes(count: 4));
            Size = br.ReadUInt32();
            Format = Encoding.ASCII.GetString(br.ReadBytes(count: 4));
            readSubChunks(br);
            return this;
        }

        private void readSubChunks(BinaryReader br)
        {
            SubChunks = new Dictionary<string, DataChunk>();
            uint curByte = 4; // 4 as we technically started from 'Format'
            while (curByte < Size)
            {
                DataChunk chunk = new DataChunk().Parse(br);
                curByte += chunk.Size + DataChunk.HeaderLength;
                SubChunks[chunk.ID] = chunk;
            }
        }
    }
}
