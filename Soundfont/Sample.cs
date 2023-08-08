using System;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public class Sample
    {
        public SampleHeader Header { get; set; }
        public Memory<byte> Data { get; set; }

        public Sample(SampleHeader header, Memory<byte> data)
        {
            Header = header;
            Data = data;
        }

        public short[] GetPCMData()
        {
            byte[] rawData = Data.ToArray();
            short[] pcmData = new short[rawData.Length / 2];
            for (int i = 0; i < rawData.Length; i += 2)
            {
                pcmData[i / 2] = BitConverter.ToInt16(rawData, i);
            }
            return pcmData;
        }
    }
}
