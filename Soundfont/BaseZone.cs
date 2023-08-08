using System.Collections.Generic;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public abstract class BaseZone
    {
        public Bag Bag { get; set; }
        public GeneratorList Generators { get; set; }
        public List<Modulator> Modulators { get; set; }

        public ZoneRange Range { get; set; }

        protected BaseZone(Bag bag, GeneratorList generators, List<Modulator> modulators)
        {
            Bag = bag;
            Generators = generators;
            Modulators = modulators;

            Range = getRangeFromGeneratorList(Generators);
        }

        public abstract bool IsGlobal { get; }

        public abstract NoteData GetNote(int note, int velocity);

        protected ZoneRange getRangeFromGeneratorList(GeneratorList generators)
        {
            ZoneRange range = ZoneRange.CreateDefault();

            // if we have generators
            if (generators.List.Count > 0 && !IsGlobal)
            {
                // if the first is a key range (it can only be the first)
                var firstGenerator = generators.List[0];
                if (firstGenerator.Type == GeneratorType.KeyRange)
                {
                    range.MinNote = firstGenerator.Amount.LoByte;
                    range.MaxNote = firstGenerator.Amount.HiByte;
                
                    // the 2nd could also be a velocity range
                    if (generators.List.Count > 1 && generators.List[1].Type == GeneratorType.VelRange)
                    {
                        range.MinVelocity = generators.List[1].Amount.LoByte;
                        range.MaxVelocity = generators.List[1].Amount.HiByte;
                    }
                }
            }
            return range;
        }
    }
}
