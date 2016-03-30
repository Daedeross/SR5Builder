namespace SR5Builder
{
    public enum Priority
    {
        U = 0, A = 1, B = 2, C = 3, D = 4, E = 5
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