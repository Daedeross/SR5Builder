﻿using System;
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

            #region General Settings

        public static Dictionary<string, SettingsLoader> GenSettingsList { get; set; }

            #endregion // General Settings

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

        public static Dictionary<string, List<SpellLoader>> PreLoadedSpells { get; private set; }

        public static List<AdeptPowerLoader> PreLoadedPowers { get; private set; }

            #endregion // Magic

            #region Gear

        public static Dictionary<string, GearModLoader> GearMods { get; set; }

        public static Dictionary<string, Dictionary<string, GearModLoader>> GearModCategories { get; set; }

        public static Dictionary<string, Dictionary<string, GearLoader>> Gear { get; set; }

        public static Dictionary<string, Dictionary<string, ImplantLoader>> Implants { get; set; }

            #endregion // Gear

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
            PreLoadedPowers = AdeptPowerLoader.LoadFromFile("Resources\\AdeptPowers.xml");

            // Load Normal Gear (non-cyber/bio-ware)
            LoadGear();

            // Load Implants (cyber/bio-ware)
            LoadImplants();

            // Gear Mods (Includes capacity costing 'ware)
            LoadGearMods();

            // set NumberFormat for nuyen
            CostFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            CostFormat.NumberDecimalDigits = Properties.Settings.Default.NumberDecimalDigits;
            CostFormat.CurrencyDecimalDigits = Properties.Settings.Default.CurrencyDecimalDigits;
            CostFormat.CurrencySymbol = Properties.Settings.Default.CurrencySymbol;
            CostFormat.CurrencyPositivePattern = Properties.Settings.Default.CurrencyPositivePattern;
            CostFormat.CurrencyNegativePattern = Properties.Settings.Default.CurrencyNegativePattern;
#if DEBUG
            // Use for generating intitial Xml Serialization templates
            //WriteFile();
#endif
        }

        private static void LoadSettings()
        {
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\GenSettings");
            FileInfo[] files = info.GetFiles("*.xml");

            GenSettingsList = new Dictionary<string, SettingsLoader>(files.Length);

            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name).Replace("Settings","");
                SettingsLoader sl = SettingsLoader.LoadFromFile(file.FullName);
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
                catch
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

            #region Skills

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

            #endregion // Skills

        private static void LoadSpells()
        {
            // find all .xml files in req. directory
            // Each file is a category w/ key = filename (w/o extention)
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Spells");
            FileInfo[] files = info.GetFiles("*.xml");

            PreLoadedSpells = new Dictionary<string, List<SpellLoader>>();

            foreach (FileInfo file in files)
            {
                string name = Path.GetFileNameWithoutExtension(file.Name);
                name = name.Replace("Spells", "");
                PreLoadedSpells.Add(name, SpellLoader.LoadFromFile(file.FullName));
            }

            // Create "All" category
            List<SpellLoader> allSpells = new List<SpellLoader>();
            foreach (List<SpellLoader> item in PreLoadedSpells.Values)
            {
                allSpells = allSpells.Concat(item).ToList();
            }
            allSpells.Sort();
            PreLoadedSpells.Add("All", allSpells);
        }

        private static void LoadGear()
        {
            List<GearLoader> list;

            // find all .xml files in req. directory
            // Each file is a category w/ key = filename (w/o extention)
            XmlSerializer ser = new XmlSerializer(typeof(List<GearLoader>));
            

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
                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    try
                    {
                        list = (List<GearLoader>)ser.Deserialize(reader);
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
                Gear.Add(name, new Dictionary<string, GearLoader>(list.Count));

                foreach (GearLoader loader in list)
                {
                    loader.Category = name;
                    Gear[name].Add(loader.Name, loader);
                    Gear["All"].Add(loader.Name, loader);
                }
            }
        }
        
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

        private static void LoadGearMods()
        {
            DirectoryInfo info = new DirectoryInfo(".\\Resources\\Gear\\GearMods");

            FileInfo[] files = info.GetFiles("*.xml");

            XmlSerializer ser = new XmlSerializer(typeof(List<GearModLoader>));
            
            GearMods = new Dictionary<string, GearModLoader>();
            GearModCategories = new Dictionary<string, Dictionary<string, GearModLoader>>();

            foreach (FileInfo file in files)
            {
                using (StreamReader reader = new StreamReader(file.FullName))
                {
                    List<GearModLoader> list = (List<GearModLoader>)ser.Deserialize(reader);
                    string cat = Path.GetFileNameWithoutExtension(file.Name);

                    foreach (GearModLoader loader in list)
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
                            GearModCategories.Add(cat, new Dictionary<string, GearModLoader>());
                            GearModCategories[cat].Add(loader.Name, loader);
                        }

                        if (GearModCategories.ContainsKey(loader.SubCategory))
                        {
                            GearModCategories[loader.SubCategory].Add(loader.Name, loader);
                        }
                        else
                        {
                            GearModCategories.Add(loader.SubCategory, new Dictionary<string, GearModLoader>());
                            GearModCategories[loader.SubCategory].Add(loader.Name, loader);
                        }
                    }
                }
            }
        }

            #region Debug and Stuff

        public static void WriteFile()
        {
            
        }

            #endregion // Debug and Stuff

        #endregion // Static Methods
    }
}
