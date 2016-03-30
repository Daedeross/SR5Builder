using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SR5Builder.DataModels;
using System.IO;
using System.Xml.Serialization;
using SR5Builder.Helpers;
using SR5Builder.Loaders;
using System.Globalization;

namespace SR5Builder
{
    public static class GlobalData
    {
        #region Static Properties

            #region Priorities / Metatype / Special Choice

        public static Dictionary<string, MetatypeStats> Metatypes { get; set; }

        public static List<SpecialChoice> Specials { get; private set; }

        public static SerializableDictionary<Priority, PriorityLevel> PriorityLevels { get; private set; }

            #endregion // Priorities / Metatype / Special Choice

            #region Skills

        public static Dictionary<string, List<SkillLoader>> PreLoadedSkills { get; private set; }

        public static List<SkillGroupLoader> PreLoadedSkillGroups { get; private set; }

            #endregion // Skills

            #region Magic

        public static Dictionary<string, List<Spell>> PreLoadedSpells { get; private set; }

        public static List<AdeptPowerLoader> PreLoadedPowers { get; private set; }

            #endregion // Magic

            #region Gear

        public static Dictionary<string, GearModLoader> GearMods { get; set; }

        public static Dictionary<string, Dictionary<string, GearLoader>> Gear { get; set; }

        public static Dictionary<string, Dictionary<string, ImplantLoader>> Implants { get; set; }

            #endregion // Gear

        public static NumberFormatInfo CostFormat;

        #endregion // Static Properties

        #region Static Methods

            #region Initialize

        public static void Initialize()
        {
            // Initialize Static Properties
            // Function name are self-explanitory
            LoadMetatpyes();
            
            LoadSpecials();

            LoadPriorities();

            LoadSkills();

            LoadSpells();

            // Load Adept Powers
            PreLoadedPowers = AdeptPowerLoader.LoadFromFile("Resources\\AdeptPowers.xml");

            // Load Normal Gear (non-cyber/bio-ware)
            LoadGear();

            // Load Implants (cyber/bio-ware)
            LoadImplants();

            // Gear Mods
            LoadGearMods();

            // set NumberFormat for nuyen
            CostFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            CostFormat.NumberDecimalDigits = 0;

            // Use for generating intitial Xml Serialization templates
            //WriteFile();
        }

            #endregion Initialize

            #region LoadMetatypes

        private static void LoadMetatpyes()
        {
            // find all .xml files in req. directory
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Metatypes");
            FileInfo[] files = info.GetFiles("*.xml");

            Metatypes = new Dictionary<string, MetatypeStats>(files.Length);

            foreach (FileInfo file in files)
            {
                MetatypeStats m = MetatypeStats.LoadFromFile(file.FullName);
                Metatypes.Add(m.Name, m);
            }
        }

            #endregion // Load Metatypes

            #region LoadSpecials
        
        private static void LoadSpecials()
        {
            // find all .xml files in req. directory
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\SpecialChoices");
            FileInfo[] files = info.GetFiles("*.xml");

            Specials = new List<SpecialChoice>();

            foreach (FileInfo file in files)
            {
                try
                {
                    List<SpecialChoice> new_list = SpecialChoice.LoadFromFile(file.FullName);
                    Specials = Specials.Concat(new_list).ToList();
                }
                catch
                {
                    Log.LogMessage("Error loading SpeicalCoice(s) from " + file.FullName);
                }
            }
        }

            #endregion //LoadSpecials

            #region LoadPriorities

        private static void LoadPriorities()
        {
            XmlSerializer ser = new XmlSerializer(typeof(SerializableDictionary<Priority, PriorityLevel>));
            StreamReader reader = new StreamReader(".\\Resources\\PrioritiesLevels.xml");
            PriorityLevels = (SerializableDictionary<Priority, PriorityLevel>)ser.Deserialize(reader);
        }

            #endregion // LoadPriorities

            #region Skills

                #region LoadSkills

        private static void LoadSkills()
        {
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Skills");
            FileInfo[] files = info.GetFiles("*.xml");
            XmlSerializer ser = new XmlSerializer(typeof(List<SkillLoader>));
            StreamReader reader;

            PreLoadedSkills = new Dictionary<string, List<SkillLoader>>();

            // creata a key from each file and load the list of skills from that file
            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);  // get category name from filename
                name = name.Replace("Skills", "");                          // remove 'Skills' suffix
                reader = new StreamReader(file.FullName);

                PreLoadedSkills.Add(name, (List<SkillLoader>)ser.Deserialize(reader));
            }

            // create 'All' category
            List<SkillLoader> allSkills = new List<SkillLoader>();
            foreach (List<SkillLoader> item in GlobalData.PreLoadedSkills.Values)
            {
                allSkills = allSkills.Concat(item).ToList();
            }
            allSkills.Sort();

            // Add 'All' category to dictionary
            PreLoadedSkills.Add("All", allSkills);

            // Autoconstruct Skill Groups from the 'All' category
            MakeSkillGroups();

        }

                #endregion // LoadSkills

                #region MakeSkillGroups

        private static void MakeSkillGroups()
        {
            PreLoadedSkillGroups = new List<SkillGroupLoader>();

            foreach (SkillLoader skill in PreLoadedSkills["All"])
            {
                if (skill.GroupName != "None" && skill.GroupName != "")
                {
                    if (!(from g in PreLoadedSkillGroups
                          select g.Name).Contains(skill.GroupName))
                    {
                        SkillGroupLoader pg = new SkillGroupLoader();
                        pg.Name = skill.GroupName;
                        pg.SkillNames.Add(skill.Name);
                        PreLoadedSkillGroups.Add(pg);
                    }
                    else
                    {
                        SkillGroupLoader pg = (from g in PreLoadedSkillGroups
                                                  where g.Name == skill.GroupName
                                                  select g).Single();
                        pg.SkillNames.Add(skill.Name);
                    }
                }
            }

            PreLoadedSkillGroups.Sort();
        }

                #endregion // MakeSkillGroups

            #endregion // Skills

            #region LoadSpells
        
        private static void LoadSpells()
        {
            // find all .xml files in req. directory
            // Each file is a category w/ key = filename (w/o extention)
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Spells");
            FileInfo[] files = info.GetFiles("*.xml");

            PreLoadedSpells = new Dictionary<string, List<Spell>>();

            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                name = name.Replace("Spells", "");
                PreLoadedSpells.Add(name, Spell.LoadFromFile(file.FullName));
            }

            // Create "All" category
            List<Spell> allSpells = new List<Spell>();
            foreach (List<Spell> item in PreLoadedSpells.Values)
            {
                allSpells = allSpells.Concat(item).ToList();
            }
            allSpells.Sort();
            PreLoadedSpells.Add("All", allSpells);
        }

            #endregion // LoadSpells

            #region LoadGear

        private static void LoadGear()
        {
            List<GearLoader> list;

            // find all .xml files in req. directory
            // Each file is a category w/ key = filename (w/o extention)
            XmlSerializer ser = new XmlSerializer(typeof(List<GearLoader>));
            StreamReader reader;

                // Misc Gear
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Gear\\Misc");

            FileInfo[] files = info.GetFiles("*.xml");


                // Weapons
            info = new DirectoryInfo(".\\Resources\\Gear\\Weapons");

            files = files.Concat(info.GetFiles("*.xml")).ToArray();


                // Initialize Dictionary
            Gear = new Dictionary<string, Dictionary<string, GearLoader>>(files.Length+1);

            Gear.Add("All", new Dictionary<string, GearLoader>());

            foreach (FileInfo file in files)
            {
                reader = new StreamReader(file.FullName);
                list = (List<GearLoader>)ser.Deserialize(reader);
                reader.Close();

                string name = Path.GetFileNameWithoutExtension(file.Name);
                Gear.Add(name, new Dictionary<string, GearLoader>(list.Count));

                foreach (GearLoader loader in list)
                {
                    loader.Category = name;
                    Gear[name].Add(loader.Name, loader);
                    Gear["All"].Add(loader.Name, loader);
                }
            }
        }
        
            #endregion // LoadGear

            #region LoadImplants

        private static void LoadImplants()
        {
            List<ImplantLoader> list;
            XmlSerializer ser = new XmlSerializer(typeof(List<ImplantLoader>));
            StreamReader reader;

            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Gear\\Implants");

            FileInfo[] files = info.GetFiles("*.xml");

            Implants = new Dictionary<string, Dictionary<string, ImplantLoader>>(files.Length + 1);

            Implants.Add("All", new Dictionary<string, ImplantLoader>());

            foreach (FileInfo file in files)
            {
                reader = new StreamReader(file.FullName);
                list = (List<ImplantLoader>)ser.Deserialize(reader);
                reader.Close();

                string name = Path.GetFileNameWithoutExtension(file.Name);
                Implants.Add(name, new Dictionary<string, ImplantLoader>(list.Count));

                foreach (ImplantLoader loader in list)
                {
                    Implants[name].Add(loader.Name, loader);
                    Implants["All"].Add(loader.Name, loader);
                }
            }
        }

            #endregion LoadImplants

            #region LoadGearMods

        private static void LoadGearMods()
        {
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Gear\\GearMods");

            FileInfo[] files = info.GetFiles("*.xml");

            XmlSerializer ser = new XmlSerializer(typeof(List<GearModLoader>));

            StreamReader reader;

            GearMods = new Dictionary<string, GearModLoader>();

            foreach (FileInfo file in files)
            {
                reader = new StreamReader(file.FullName);

                try
                {
                    List<GearModLoader> list = (List<GearModLoader>)ser.Deserialize(reader);
                    string cat = Path.GetFileNameWithoutExtension(file.Name);

                    foreach (GearModLoader loader in list)
                    {
                        loader.Category = cat;
                        GearMods.Add(loader.Name, loader);
                    }

                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

            #endregion // LoadGearMods

        #region Debug and Stuff

        public static void WriteFile()
        {
            

            XmlSerializer ser = new XmlSerializer(typeof(List<WeaponLoader>));
            StreamWriter writer = new StreamWriter("RangedWeapons.xml");

            List<WeaponLoader> list = new List<WeaponLoader>();

            RangedWeaponLoader rwl = new RangedWeaponLoader();

            rwl.Name = "Ares Light Fire 70";
            rwl.Book = "SR5";
            rwl.Page = 426;
            rwl.Category = "Ranged Weapons - Light Pistol";
            rwl.ExtArray = new List<string>(0);
            rwl.ExtKind = "";
            rwl.ExtLabel = "";
            rwl.Rating = 0;
            rwl.HasRating = false;
            rwl.Availability = new Availability("3R");
            rwl.Cost = 200;
            rwl.Capacity = 0;
            rwl.BaseMods = new string[0];
            rwl.Mods = new string[0];
            rwl.DV = 6;
            rwl.DamageType = DamageType.P;
            rwl.AP = 0;
            rwl.Acc = 7;
            rwl.AmmoCount = 16;
            rwl.FireModes = new FireMode[] { FireMode.SA };
            rwl.ReloadMethod = ReloadMethod.c;
            rwl.RC = 0;

            list.Add(rwl);

            ser.Serialize(writer, list);


            

            writer.Close();
        }

            #endregion // Debug and Stuff

        #endregion // Static Methods
    }
}
