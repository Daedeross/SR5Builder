Shadowrun 5th edition character builder
=======================================

This is the repository for SR5Builder, a tool to create and maintan characters for Shadowrun 5th edition. The program is written primarily in C# for using Windows Presentation Foundation.

Goals
-----

* Create Characters
  * Allow rules-accurate tracking of character options: points, karma, nuyen, etc.
  * Allow multiple methods of character generation
    * Priority System (default)
    * Sum to Ten (slight change to priority system)
    * Point Buy (aka KarmaGen)
    * That other one... with the packages?
    * Built Point (perhaps, for SR4 compatability & houserules)
* Save & Load Characters
* Import data
  Import data such as Priority Levels, Metatypes, Spells, Gear, etc. from XML files to allow for expandability and customization.
* Track Characters
  Keep track of "In Play" characters.
  * Run rewards (Karma, nuyen, Contacts, and more)
  * Street Cred, Notoriety, Public Awareness.
  * Advancement
    * Increased Attributes and Skills
    * Initiation and Submersion
    * Foci bonding and Spell learning
    * +/- Qualities
    * Martial Arts
* Print character (pipe dream)
