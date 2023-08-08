using System.Collections.Generic;
using System.Linq;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public class GeneratorList
    {
        public List<Generator> List { get; set; }

        public bool LastIsInstrument => Last().Type == GeneratorType.Instrument;

        public bool LastIsSample => Last().Type == GeneratorType.SampleID;

        public Generator Last() => List.Last();

        public GeneratorList()
        {
            List = new List<Generator>();
        }

        public int GetSampleIndex()
        {
            var lastGenerator = Last();
            if (lastGenerator.Type != GeneratorType.SampleID) return -1;
            return lastGenerator.Amount.UnsignedValue;
        }

        public int GetInstrumentIndex()
        {
            var lastGenerator = Last();
            if (lastGenerator.Type != GeneratorType.Instrument) return -1;
            return lastGenerator.Amount.UnsignedValue;
        }

        public GeneratorResult ProcessGenerators()
        {
            var result = new GeneratorResult();
            foreach (var generator in List)
            {
                result = processGenerator(generator, result);
            }
            return result;
        }

        protected GeneratorResult processGenerator(Generator generator, GeneratorResult result)
        {
            switch (generator.Type)
            {
                case GeneratorType.Pan:
                {
                    result.Pan = generator.Amount.Value;
                    break;
                }
                case GeneratorType.AttackVolEnv:
                {
                    result.Attack = generator.Amount.UnsignedValue;
                    break;
                }
                case GeneratorType.DecayVolEnv:
                {
                    result.Decay = generator.Amount.UnsignedValue;
                    break;
                }
                case GeneratorType.SustainVolEnv:
                {
                    result.Sustain = generator.Amount.UnsignedValue;
                    break;
                }
                case GeneratorType.ReleaseVolEnv:
                {
                    result.Release = generator.Amount.UnsignedValue;
                    break;
                }
                case GeneratorType.Keynum:
                {
                    result.KeyOverride = generator.Amount.UnsignedValue;
                    break;
                }
                case GeneratorType.Velocity:
                {
                    result.VelocityOverride = generator.Amount.UnsignedValue;
                    break;
                }
                case GeneratorType.InitialAttenuation:
                {
                    result.Attenuation = generator.Amount.UnsignedValue;
                    break;
                }
                case GeneratorType.FineTune:
                {
                    result.PitchModificationCents += generator.Amount.Value;
                    break;
                }
                case GeneratorType.CoarseTune:
                {
                    result.PitchModificationCents += generator.Amount.Value * 100; // 100 cents per semitone
                    break;
                }
                case GeneratorType.OverridingRootKey:
                {
                    result.RootKeyOverride = generator.Amount.Value == -1 ? (int?)null : generator.Amount.UnsignedValue;
                    break;
                }
            }
            return result;
        }
    }
}
