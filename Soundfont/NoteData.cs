namespace EZSF2.Soundfont
{
    public struct NoteData
    {
        public int Note { get; set; }
        public int Velocity { get; set; }
        public Sample Sample { get; set; }
        public GeneratorResult Settings { get; set; }
    }
}
