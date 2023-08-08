namespace EZSF2.Soundfont
{
    public struct ZoneRange
    {
        public int MinNote;
        public int MaxNote;
        public int MinVelocity;
        public int MaxVelocity;

        public static ZoneRange CreateDefault()
        {
            return new ZoneRange
            {
                MinNote = 0,
                MaxNote = 127,
                MinVelocity = 0,
                MaxVelocity = 127
            };
        }

        public bool NoteInRange(int note, int velocity)
        {
            return note >= MinNote && note <= MaxNote && velocity >= MinVelocity && velocity <= MaxVelocity;
        }
    }
}
