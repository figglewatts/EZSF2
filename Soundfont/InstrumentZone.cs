using System.Collections.Generic;
using System.Diagnostics;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public class InstrumentZone : BaseZone
    {
        public Sample Sample { get; set; }

        public override bool IsGlobal => Generators.List.Count == 0 ||!Generators.LastIsSample;

        public InstrumentZone(Bag bag, GeneratorList generators, List<Modulator> modulators, List<Sample> samples) 
            : base(bag, generators, modulators)
        {
            Sample = getSampleFromGeneratorList(Generators, samples);
        }
    
        public override NoteData GetNote(int note, int velocity)
        {
            Debug.Assert(Sample != null);
            NoteData noteData = new NoteData
            {
                Sample = Sample,
                Note = note,
                Velocity = velocity,
                Settings = Generators.ProcessGenerators()
            };
            return noteData;
        }

        protected Sample getSampleFromGeneratorList(GeneratorList generators, List<Sample> samples)
        {
            int sampleIdx = generators.GetSampleIndex();
            if (sampleIdx == -1) return null;
            return samples[sampleIdx];
        }
    }
}
