using System.Collections.Generic;
using System.Diagnostics;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public class PresetZone : BaseZone
    {
        public Instrument Instrument { get; set; }

        public override bool IsGlobal => Generators.List.Count == 0 ||!Generators.LastIsInstrument;

        public PresetZone(Bag bag, GeneratorList generators, List<Modulator> modulators, List<Instrument> instruments) 
            : base(bag, generators, modulators)
        {
            Instrument = getInstrumentFromGeneratorList(Generators, instruments);
        }
    
        public override NoteData GetNote(int note, int velocity)
        {
            Debug.Assert(Instrument != null);
            var presetGeneratorResult = Generators.ProcessGenerators();
            var instrumentNoteResult = Instrument.GetNote(note, velocity);
            instrumentNoteResult.Settings.Add(presetGeneratorResult); // preset is additive to instrument
            return instrumentNoteResult;
        }
    
        protected Instrument getInstrumentFromGeneratorList(GeneratorList generators, List<Instrument> instruments)
        {
            int instrumentIndex = generators.GetInstrumentIndex();
            if (instrumentIndex == -1) return null;
            return instruments[instrumentIndex];
        }
    }
}
