namespace EZSF2.Soundfont
{
    public struct GeneratorResult
    {
        public int? Pan;
        public int? Attack;
        public int? Decay;
        public int? Sustain;
        public int? Release;
        public int? KeyOverride;
        public int? VelocityOverride;
        public int? Attenuation;
        public int? RootKeyOverride;
        public int? PitchModificationCents;

        public GeneratorResult Apply(GeneratorResult overlay)
        {
            if (overlay.Pan.HasValue) Pan = overlay.Pan;
            if (overlay.Attack.HasValue) Attack = overlay.Attack;
            if (overlay.Decay.HasValue) Decay = overlay.Decay;
            if (overlay.Sustain.HasValue) Sustain = overlay.Sustain;
            if (overlay.Release.HasValue) Release = overlay.Release;
            if (overlay.KeyOverride.HasValue) KeyOverride = overlay.KeyOverride;
            if (overlay.VelocityOverride.HasValue) VelocityOverride = overlay.VelocityOverride;
            if (overlay.Attenuation.HasValue) Attenuation = overlay.Attenuation;
            if (overlay.RootKeyOverride.HasValue) RootKeyOverride = overlay.RootKeyOverride;
            if (overlay.PitchModificationCents.HasValue) PitchModificationCents = overlay.PitchModificationCents;
            return this;
        }

        public GeneratorResult Add(GeneratorResult other)
        {
            if (other.Pan.HasValue) Pan += other.Pan;
            if (other.Attack.HasValue) Attack += other.Attack;
            if (other.Decay.HasValue) Decay += other.Decay;
            if (other.Sustain.HasValue) Sustain += other.Sustain;
            if (other.Release.HasValue) Release += other.Release;
            if (other.KeyOverride.HasValue) KeyOverride += other.KeyOverride;
            if (other.VelocityOverride.HasValue) VelocityOverride += other.VelocityOverride;
            if (other.Attenuation.HasValue) Attenuation += other.Attenuation;
            if (other.RootKeyOverride.HasValue) RootKeyOverride += other.RootKeyOverride;
            if (other.PitchModificationCents.HasValue) PitchModificationCents += other.PitchModificationCents;
            return this;
        }

        public GeneratorResult SetNullsToDefaults()
        {
            Pan = Pan ?? 0;
            Attack = Attack ?? -12000;
            Decay = Decay ?? -12000;
            Sustain = Sustain ?? 0;
            Release = Release ?? -12000;
            Attenuation = Attenuation ?? 0;
            PitchModificationCents = PitchModificationCents ?? 0;
            return this;
        }
    }
}
