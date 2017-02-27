using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SR5Builder.DataModels;
using System.IO;
using System.Xml.Serialization;
using SR5Builder.Helpers;
using SR5Builder.Prototypes;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SR5Builder
{
    public static class GlobalData
    {
        private const string AddSpacePattern = "([a-z])([A-Z])";
        private const string AddSpaceReplacement = "$1 $2";
        private static Regex AddSpaveRegex = new Regex(AddSpacePattern);

        #region Static Properties

            #region General Settings

        public static Dictionary<string, SettingsPrototype> GenSettingsList { get; set; }

            #endregion // General Settings

            #region Priorities / Metatype / Special Choice

        public static Dictionary<string, MetatypeStats> Metatypes { get; set; }

        public static List<SpecialChoice> Specials { get; private set; }

        public static SerializableDictionary<Priority, PriorityLevel> PriorityLevels { get; private set; }

            #endregion // Priorities / Metatype / Special Choice

            #region Skills

        public static Dictionary<string, List<SkillPrototype>> PreLoadedSkills { get; private set; }

        public static List<SkillGroupPrototype> PreLoadedSkillGroups { get; private set; }

            #endregion // Skills

            #region Magic

        public static Dictionary<string, List<SpellPrototype>> PreLoadedSpells { get; private set; }

        public static List<AdeptPowerPrototype> PreLoadedPowers { get; private set; }

            #endregion // Magic

            #region Gear

        public static Dictionary<string, GearModPrototype> GearMods { get; set; }

        public static Dictionary<string, Dictionary<string, GearModPrototype>> GearModCategories { get; set; }

        public static Dictionary<string, Dictionary<string, GearPrototype>> Gear { get; set; }

        public static Dictionary<string, Dictionary<string, ImplantPrototype>> Implants { get; set; }

        #endregion // Gear

        public static Dictionary<string, List<QualityPrototype>> Qualities;

        public static NumberFormatInfo CostFormat;

        #endregion // Static Properties

        #region Static Methods

        public static void Initialize()
        {
            // Initialize Static Properties
            // Function name are self-explanitory
            LoadSettings();

            LoadMetatpyes();
            
            LoadSpecials();

            LoadPriorities();

            LoadSkills();

            LoadSpells();

            // Load Adept Powers
            LoadPowers();

            // Load Normal Gear (non-cyber/bio-ware)
            LoadGear();

            // Load Implants (cyber/bio-ware)
            LoadImplants();

            // Gear Mods (Includes capacity costing 'ware)
            LoadGearMods();

            LoadQualities();

            // set NumberFormat for nuyen
            CostFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            CostFormat.NumberDecimalDigits = Properties.Settings.Default.NumberDecimalDigits;
            CostFormat.CurrencyDecimalDigits = Properties.Settings.Default.CurrencyDecimalDigits;
            CostFormat.CurrencySymbol = Properties.Settings.Default.CurrencySymbol;
            CostFormat.CurrencyPositivePattern = Properties.Settings.Default.CurrencyPositivePattern;
            CostFormat.CurrencyNegativePattern = Properties.Settings.Default.CurrencyNegativePattern;
#if DEBUG
            // Use for generating intitial Xml Serialization templates
            WriteFile();
#endif
        }

        private static void LoadSettings()
        {
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\GenSettings");
            FileInfo[] files = info.GetFiles("*.xml");

            GenSettingsList = new Dictionary<string, SettingsPrototype>(files.Length);

            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name).Replace("Settings","");
                SettingsPrototype sl = SettingsPrototype.LoadFromFile(file.FullName);
                GenSettingsList.Add(name, sl);
            }
        }

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
                catch (InvalidOperationException)
                {
                    Log.LogMessage("Error loading SpeicalCoice(s) from " + file.FullName);
                }
            }
        }

        private static void LoadPriorities()
        {
            XmlSerializer ser = new XmlSerializer(typeof(SerializableDictionary<Priority, PriorityLevel>));
            StreamReader reader = new StreamReader(".\\Resources\\PrioritiesLevels.xml");
            PriorityLevels = (SerializableDictionary<Priority, PriorityLevel>)ser.Deserialize(reader);
        }

        private static void LoadPowers()
        {
            PreLoadedPowers = new List<AdeptPowerPrototype>();
            DirectoryInfo dInfo = new DirectoryInfo("Resources\\AdeptPowers");
            FileInfo[] files = dInfo.GetFiles("*.xml");

            foreach (FileInfo file in files)
            {
                try
                {
                    PreLoadedPowers = PreLoadedPowers.Concat(
                        AdeptPowerPrototype.LoadFromFile(file.FullName)).ToList();
                }
                catch (InvalidOperationException e) // catch parse errors
                {
                    System.Diagnostics.Debug.WriteLine("Error Loading file: {0}.", (object)file.Name);   // log error
                    System.Diagnostics.Debug.WriteLine("• {0}.", (object)e.Message);
                    System.Diagnostics.Debug.WriteLine("• {0}", e.InnerException.Message);
                    continue; // skip loading of file and go to next
                } 
            }
        }

            #region Skills

        private static void LoadSkills()
        {
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Skills");
            FileInfo[] files = info.GetFiles("*.xml");
            XmlSerializer ser = new XmlSerializer(typeof(List<SkillPrototype>));
            StreamReader reader;

            PreLoadedSkills = new Dictionary<string, List<SkillPrototype>>();

            // creata a key from each file and load the list of skills from that file
            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);  // get category name from filename
                name = name.Replace("Skills", "");                          // remove 'Skills' suffix
                reader = new StreamReader(file.FullName);

                PreLoadedSkills.Add(name, (List<SkillPrototype>)ser.Deserialize(reader));
            }

            // create 'All' category
            List<SkillPrototype> allSkills = new List<SkillPrototype>();
            foreach (List<SkillPrototype> item in GlobalData.PreLoadedSkills.Values)
            {
                allSkills = allSkills.Concat(item).ToList();
            }
            allSkills.Sort();

            // Add 'All' category to dictionary
            PreLoadedSkills.Add("All", allSkills);

            // Autoconstruct Skill Groups from the 'All' category
            MakeSkillGroups();
        }

        private static void MakeSkillGroups()
        {
            PreLoadedSkillGroups = new List<SkillGroupPrototype>();

            foreach (SkillPrototype skill in PreLoadedSkills["All"])
            {
                if (skill.GroupName != "None" && skill.GroupName != "")
                {
                    if (!(from g in PreLoadedSkillGroups
                          select g.Name).Contains(skill.GroupName))
                    {
                        SkillGroupPrototype pg = new SkillGroupPrototype()
                        {
                            Name = skill.GroupName
                        };
                        pg.SkillNames.Add(skill.Name);
                        PreLoadedSkillGroups.Add(pg);
                    }
                    else
                    {
                        SkillGroupPrototype pg = (from g in PreLoadedSkillGroups
                                                  where g.Name == skill.GroupName
                                                  select g).Single();
                        pg.SkillNames.Add(skill.Name);
                    }
                }
            }

            PreLoadedSkillGroups.Sort();
        }

            #endregion // Skills

        private static void LoadSpells()
        {
            // find all .xml files in req. directory
            // Each file is a category w/ key = filename (w/o extention)
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Spells");
            FileInfo[] files = info.GetFiles("*.xml");

            PreLoadedSpells = new Dictionary<string, List<SpellPrototype>>();

            foreach (FileInfo file in files)
            {
                try
                {
                    string name = Path.GetFileNameWithoutExtension(file.Name);
                    name = name.Replace("Spells", "");
                    PreLoadedSpells.Add(name, SpellPrototype.LoadFromFile(file.FullName));
                }
                catch (InvalidOperationException e) // catch parse errors
                {
                    System.Diagnostics.Debug.WriteLine("Error Loading file: {0}.", (object)file.Name);   // log error
                    System.Diagnostics.Debug.WriteLine("• {0}.", (object)e.Message);
                    System.Diagnostics.Debug.WriteLine("• {0}", e.InnerException.Message);
                    continue; // skip loading of file and go to next
                }
            }

            // Create "All" category
            List<SpellPrototype> allSpells = new List<SpellPrototype>();
            foreach (List<SpellPrototype> item in PreLoadedSpells.Values)
            {
                allSpells = allSpells.Concat(item).ToList();
            }
            allSpells.Sort();
            PreLoadedSpells.Add("All", allSpells);
        }

        private static void LoadGear()
        {
            List<GearPrototype> list;

            // find all .xml files in req. directory
            // Each file is a category w/ key = filename (w/o extention)
            XmlSerializer ser = new XmlSerializer(typeof(List<GearPrototype>));
            

                // Misc Gear
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Gear\\Misc");

            FileInfo[] files = info.GetFiles("*.xml");


                // Weapons
            info = new DirectoryInfo(".\\Resources\\Gear\\Weapons");

            files = files.Concat(info.GetFiles("*.xml")).ToArray();


            // Initialize Dictionary
            Gear = new Dictionary<string, Dictionary<string, GearPrototype>>(files.Length + 1)
            {
                {  "All", new Dictionary<string, GearPrototype>() }
            };

            foreach (FileInfo file in files)
            {
                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    try
                    {
                        list = (List<GearPrototype>)ser.Deserialize(reader);
                        System.Diagnostics.Debug.WriteLine("Succesfully loaded file: {0}", (object)file.Name);
                    }
                    catch (InvalidOperationException e) // catch parse errors
                    {
                        System.Diagnostics.Debug.WriteLine("Error Loading file: {0}.", (object)file.Name);   // log error
                        System.Diagnostics.Debug.WriteLine("• {0}.", (object)e.Message);
                        System.Diagnostics.Debug.WriteLine("• {0}", e.InnerException.Message);
                        continue; // skip loading of file and go to next
                    }
                }

                string name = Path.GetFileNameWithoutExtension(file.Name);
                Gear.Add(name, new Dictionary<string, GearPrototype>(list.Count));

                foreach (GearPrototype loader in list)
                {
                    loader.Category = name;
                    Gear[name].Add(loader.Name, loader);
                    Gear["All"].Add(loader.Name, loader);
                }
            }
        }
        
        private static void LoadImplants()
        {
            List<ImplantPrototype> list;
            XmlSerializer ser = new XmlSerializer(typeof(List<ImplantPrototype>));

            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Gear\\Implants");

            FileInfo[] files = info.GetFiles("*.xml");

            Implants = new Dictionary<string, Dictionary<string, ImplantPrototype>>(files.Length + 1)
            {
                { "All", new Dictionary<string, ImplantPrototype>() }
            };
            foreach (FileInfo file in files)
            {
                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    try
                    {
                        list = (List<ImplantPrototype>)ser.Deserialize(reader);
                        System.Diagnostics.Debug.WriteLine("Succesfully loaded file: {0}", (object)file.Name);
                    }
                    catch (InvalidOperationException e) // catch parse errors
                    {
                        System.Diagnostics.Debug.WriteLine("Error Loading file: {0}.", (object)file.Name);   // log error
                        System.Diagnostics.Debug.WriteLine("• {0}.", (object)e.Message);
                        System.Diagnostics.Debug.WriteLine("• {0}", e.InnerException.Message);
                        continue; // skip loading of file and go to next
                    }
                }

                string name = Path.GetFileNameWithoutExtension(file.Name);
                Implants.Add(name, new Dictionary<string, ImplantPrototype>(list.Count));

                foreach (ImplantPrototype loader in list)
                {
                    loader.Category = name;
                    Implants[name].Add(loader.Name, loader);
                    Implants["All"].Add(loader.Name, loader);
                }
            }
        }

        private static void LoadGearMods()
        {
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Gear\\GearMods");

            FileInfo[] files = info.GetFiles("*.xml");

            XmlSerializer ser = new XmlSerializer(typeof(List<GearModPrototype>));
            
            GearMods = new Dictionary<string, GearModPrototype>();
            GearModCategories = new Dictionary<string, Dictionary<string, GearModPrototype>>();

            foreach (FileInfo file in files)
            {
                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    List<GearModPrototype> list = (List<GearModPrototype>)ser.Deserialize(reader);
                    string cat = Path.GetFileNameWithoutExtension(file.Name);

                    foreach (GearModPrototype loader in list)
                    {
                        loader.Category = cat;
                        loader.SubCategory = loader.SubCategory ?? "None";
                        GearMods.Add(loader.Name, loader);
                        if (GearModCategories.ContainsKey(cat))
                        {
                            GearModCategories[cat].Add(loader.Name, loader);
                        }
                        else
                        {
                            GearModCategories.Add(cat, new Dictionary<string, GearModPrototype>());
                            GearModCategories[cat].Add(loader.Name, loader);
                        }

                        if (GearModCategories.ContainsKey(loader.SubCategory))
                        {
                            GearModCategories[loader.SubCategory].Add(loader.Name, loader);
                        }
                        else
                        {
                            GearModCategories.Add(loader.SubCategory, new Dictionary<string, GearModPrototype>());
                            GearModCategories[loader.SubCategory].Add(loader.Name, loader);
                        }
                    }
                }
            }
        }

        private static void LoadQualities()
        {
            Qualities = new Dictionary<string, List<QualityPrototype>>();

            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Qualities");

            FileInfo[] files = info.GetFiles("*.xml");

            foreach (FileInfo file in files)
            {
                List<QualityPrototype> quals = QualityPrototype.LoadFromFile(file.FullName);
                quals.Sort();
                
                string cat = Path.GetFileNameWithoutExtension(file.Name);
                cat = AddSpaveRegex.Replace(cat, AddSpaceReplacement);
                Qualities.Add(cat, quals);
            }

            IEnumerable<QualityPrototype> allQuals = new List<QualityPrototype>();
            foreach (var list in Qualities.Values)
            {
                allQuals = allQuals.Concat(list);
            }

            Qualities.Add("All", allQuals.ToList());
            Log.LogMessage("Quaities Loaded");
        }

            #region Debug and Stuff

        public static void WriteFile()
        {

        }

            #endregion // Debug and Stuff

        #endregion // Static Methods
    }
}
