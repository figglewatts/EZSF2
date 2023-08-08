using System;
using System.IO;
using System.Text;

namespace EZSF2.Soundfont.Chunk
{
    public class SampleHeader : IChunkElement
    {
        public const int Size = 20 + 5 * 4 + 2 * 2 + 2 * 1;

        public string Name { get; set; }
        public uint Start { get; set; }
        public uint End { get; set; }
        public uint StartLoop { get; set; }
        public uint EndLoop { get; set; }
        public uint SampleRate { get; set; }
        public byte OriginalPitch { get; set; }
        public byte PitchCorrection { get; set; }
        public ushort SampleLink { get; set; }
        public SampleType SampleType { get; set; }

        public IChunkElement FromBinaryReader(BinaryReader br)
        {
            Name = Encoding.ASCII.GetString(br.ReadBytes(count: 20)).Trim('\0').Trim();
            Start = br.ReadUInt32();
            End = br.ReadUInt32();
            StartLoop = br.ReadUInt32();
            EndLoop = br.ReadUInt32();
            SampleRate = br.ReadUInt32();
            OriginalPitch = br.ReadByte();
            PitchCorrection = br.ReadByte();
            SampleLink = br.ReadUInt16();
            SampleType = (SampleType)br.ReadUInt16();
            return this;
        }

        public bool IsZero() => Name.Equals("EOS", StringComparison.InvariantCulture) && Start == 0 && End == 0 &&
                                StartLoop == 0 && EndLoop == 0 && SampleRate == 0 && OriginalPitch == 0 &&
                                PitchCorrection == 0 && SampleLink == 0 && SampleType == 0;
    }
}
