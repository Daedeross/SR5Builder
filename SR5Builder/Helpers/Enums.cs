namespace SR5Builder
{
    public enum Priority
    {
        U = 0, E = 1, D = 2, C = 3, B = 4, A = 5
    }

    static class PriorityMethods
    {
        public static uint Mask(this Priority p)
        {
            switch (p)
            {
                case Priority.U:
                    return 0x1;
                case Priority.E:
                    return 0x2;
                case Priority.D:
                    return 0x4;
                case Priority.C:
                    return 0x8;
                case Priority.B:
                    return 0x10;
                case Priority.A:
                    return 0x20;
                default:
                    return 0x0;
            }
        }
    }

    public enum SpecialKind
    {
        None = 0,
        Magic,
        Resonance,
    }

    public enum AugmentKind
    {
        None = 0,
        Rating,
        DamageValue,
        DamageType,
        Accuracy,
        Availability,
        Restriction,
        RC,
        AP,
    }

    public enum SkillType
    {
        NA = 0,
        Active,
        Magical,
        Resonance,
        Knowledge,
        Language
    }

    public enum VisionType
    {
        Normal = 0,
        LowLight,
        Thermographic,
    }

    public enum FireMode
    {
        Other = 0,
        SS,
        SA,
        BF,
        FA
    }

    public enum ReloadMethod
    {
        b,
        c,
        d,
        ml,
        m,
        cy,
        belt,
    }

}