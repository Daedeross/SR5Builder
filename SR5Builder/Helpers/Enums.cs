namespace SR5Builder
{
    /// <summary>
    /// The possible types of character generation.
    /// </summary>
    public enum CharGenMethod
    {
        /// <summary>For GM use, no points, limitations, etc.</summary>
        NPC = 0,
        /// <summary>Default method from <em>SR5 - Core Rulebook</em> §3.</summary>
        Priority = 1,
        /// <summary>See <em>Run Faster</em> p.62.</summary>
        SumToTen = 2,
        /// <summary>See <em>Run Faster</em> p.64. Changed name from PointBuy to differentiate from <see cref="CharGenMethod.BuildPoints"/>.</summary>
        KarmaGen = 3,
        /// <summary>See <em>Run Faster</em> p.65</summary>
        LifeModules = 4,
        /// <summary>Added for possible SR4/house rules compatability.</summary>
        BuildPoints = 5,
    }

    public enum Priority
    {
        U = 0, E = 1, D = 2, C = 3, B = 4, A = 5
    }

    static class PriorityMethods
    {
        /// <summary>
        /// Converts a priority into a bit-mask.
        /// Equivalent to 2^(int)p
        /// </summary>
        /// <param name="p"></param>
        /// <returns>U:0x1, E:0x2, D:0x4, C:0x8, B:0x10, A:0x20</returns>
        public static uint Mask(this Priority p)
        {
            // This should be much faster than 2^(int)p
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

    /// <summary>
    /// <em>How</em> the Augment enhances the target trait.
    /// Essentialy what property it effects.
    /// </summary>
    public enum AugmentKind
    {
        /// <summary>Augment does nothing!?</summary>
        None = 0,
        /// <summary>Augment alters the target trait's <b>Rating.</b></summary>
        Rating,
        /// <summary>Augment alters the target trait's <b>Damage Value.</b></summary>
        DamageValue,
        /// <summary>Augment alters the target trait's <b>Damage Type</b> (i.e. Stun ↔ Physical).</summary>
        DamageType,
        /// <summary>Augment alters the target trait's <b>Accuracy</b> (or Limit).</summary>
        Accuracy,
        /// <summary>Augment alters the target trait's <b>Availability</b> rating</summary>
        Availability,
        /// <summary>>Augment alters the target trait's <b>Restriction</b> (eg R or F).</summary>
        Restriction,
        /// <summary>>Augment alters the target trait's <b>Recoil Compensation</b>.</summary>
        RC,
        /// <summary>Augment alters the target trait's <b>Armor Penetration</b> rating.</summary>
        AP,
    }

    /// <summary>
    /// The general types of skills.
    /// </summary>
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
        /// <summary>For special modes.</summary>
        Other = 0,
        /// <summary>Single Shot</summary>
        SS,
        /// <summary>Semi-auto</summary>
        SA,
        /// <summary>Burst-fire</summary>
        BF,
        /// <summary>Full-auto</summary>
        FA
    }

    /// <summary>
    /// How ammo is fed into the gun. See SR5 - Core rulebook p.417 "Ammo" §.
    /// </summary>
    public enum ReloadMethod
    {   
        /// <summary>special</summary>
        s,
        /// <summary>break</summary>
        b,
        /// <summary>clip</summary>
        c,
        /// <summary>drum</summary>
        d,
        /// <summary>muzzle-load</summary>
        ml,
        /// <summary>internal magazine</summary>
        m,
        /// <summary>cylinder</summary>
        cy,
        /// <summary>belt-fed</summary>
        belt,
    }

}