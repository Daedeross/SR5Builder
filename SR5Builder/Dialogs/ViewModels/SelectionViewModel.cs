using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;
using SR5Builder.Loaders;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SR5Builder.ViewModels
{
    public class SelectionViewModel: DialogViewModel
    {
        #region Private Fields

        private SR5Character character;

        private string message;

        #endregion // Private fields

        #region Properties

        public override string Message
        {
            get { return message; }
        }

        public bool Fillable
        {
            get { return (SelectionList == null || SelectionList.Count == 0); }
        }

        public ObservableCollection<string> SelectionList { get; set; }

        public string Selection { get; set; }

        #endregion // Properties

        #region Constructors

        public SelectionViewModel()
        {
            this.message = "";
            Selection = "";
        }

        public SelectionViewModel(string message)
            :base ()
        {
            this.message = message;
            Selection = "";
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Fillable");
        }

        #endregion // Private Methods

        #region Public Methods

        public void SetMessage(string msg)
        {
            message = msg;
        }

        public void SetList(List<string> list)
        {
            if (SelectionList != null)
            {
                SelectionList.CollectionChanged -= this.OnCollectionChanged;
            }
            SelectionList = new ObservableCollection<string>(list);
            SelectionList.CollectionChanged += this.OnCollectionChanged;
        }

        public static SelectionViewModel CreateSelection(SR5Character character, TraitExtLoader loader)
        {
            SelectionViewModel vm;
            string message = "";
            List<string> selList = null;

            // Generate SelectionList
            if (loader.ExtArray != null && loader.ExtArray.Count > 0)
            {
                selList = loader.ExtArray;
                switch (loader.ExtKind)
                {
                    case "Skill":
                       message = "Select the skill to receive the bonus:";
                        break;
                    case "Attribute":
                        message = "Select the attribute to receive the bonus:";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (loader.ExtKind)
                {
                    case "Free":
                    case "free":
                        selList = new List<string>();
                        if (loader.ExtPrompt != null)
                            message = loader.ExtPrompt;
                        else
                            message = "Enter the name extension for this trait:";
                        break;
                    case "Skill":
                        selList = character.SkillList.Values.Select(v => v.Name).ToList();
                        foreach (SkillGroup sg in character.SkillGroupsList.Values)
                            selList = selList.Concat(sg.Skills.Keys).ToList();
                        message = "Enter the name of the skill to receive the Bonus:";
                        break;
                    default:
                        break;
                }
            }
            vm = new SelectionViewModel(message);
            vm.SetList(selList);
                        
            return vm;
        }

        #endregion // Public Methods
    }
}