using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.WinForms;

namespace MD
{
    public partial class SingleValueManager<T> : UserControl where T : class
    {
        internal SingleValueManager()
        {
            InitializeComponent();
        }

        public SingleValueManager(string elementName, string displayMember, Func<IEnumerable<T>> loadCollection, Func<T, IEnumerable<FaqQuestion>> loadQuestions, Action<T> elementDelete, Action<string> newElement, Action<T, string> saveElement)
            : this()
        {
            this.loadCollection = loadCollection;
            this.loadQuestions = loadQuestions;
            this.elementDelete = elementDelete;
            this.newElement = newElement;
            this.saveElement = saveElement;
            this.elementName = elementName;
            this.displayMember = displayMember;

            CollectionList.DisplayMember = displayMember;
            NewButton.Text = string.Format(NewButton.Text, elementName);
            ManageGroup.Text = string.Format(ManageGroup.Text, elementName);
            SyncData(false, false);
        }

        private string elementName;
        private string displayMember;

        private Func<IEnumerable<T>> loadCollection;
        private Func<T, IEnumerable<FaqQuestion>> loadQuestions;
        private Action<T> elementDelete;
        private Action<string> newElement;
        private Action<T, string> saveElement;

        private List<T> currentCollection = null;
        private List<FaqQuestion> currentQuestions = null;

        public delegate void OnDataSynced();
        public event OnDataSynced DataSynced;
        public void SyncData()
        {
            SyncData(false, true);
        }
        private void SyncData(bool triggerEvent, bool keepIndex)
        {
            CollectionList.BeginUpdate();
            int index = CollectionList.SelectedIndex;
            CollectionList.DataSource = currentCollection = new List<T>(loadCollection());
            if (index != -1 && keepIndex)
                CollectionList.SelectedIndex = index;
            CollectionList.EndUpdate();
            if (triggerEvent)
                if (DataSynced != null)
                    DataSynced();
        }

        private T GetCurrentElement()
        {
            if (CollectionList.SelectedIndex != -1)
                return currentCollection[CollectionList.SelectedIndex];
            return null;
        }
        private void CollectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            T element = GetCurrentElement();
            if (element != null)
            {
                currentQuestions = new List<FaqQuestion>(loadQuestions(element));
                DeleteButton.Enabled = currentQuestions.Count == 0;
                RenameButton.Enabled = (element != null);
            }
            else
                DeleteButton.Enabled = RenameButton.Enabled = false;
            LoadQuestionsList();
        }

        private void LoadQuestionsList()
        {
            QuestionsList.DataSource = currentQuestions;
            if (currentQuestions == null)
                QuestionsLabel.Text = "";
            else
                QuestionsLabel.Text = string.Format("Questions assigned to this {0}: ({1})", elementName, currentQuestions.Count);
        }

        public delegate void OnQuestionSelected(FaqQuestion question);
        public event OnQuestionSelected QuestionSelected;
        private void QuestionsList_DoubleClick(object sender, EventArgs e)
        {
            if (QuestionsList.SelectedIndex > -1)
                if (QuestionSelected != null)
                    QuestionSelected(currentQuestions[QuestionsList.SelectedIndex]);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            T element = GetCurrentElement();
            if (element != null)
                if (Dialog.Warning.YesNo(string.Format("Are you sure you want to delete the selected {0} '{1}'?", elementName, GetDisplayMember(element))))
                {
                    elementDelete(element);
                    SyncData(true, false);
                }
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            var input = new InputBox<TextBox>("Enter the new {0} name.".FormatWith(elementName), "Add new {0}".FormatWith(elementName), "Add");
            if (input.ShowDialog() == DialogResult.OK)
            {
                string value = input.InputControl.Text.Trim();
                if (!value.IsNullOrEmpty())
                {
                    newElement(input.InputControl.Text);
                    SyncData(true, false);
                }
            }
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            T element = GetCurrentElement();
            if (element != null)
            {
                var input = new InputBox<TextBox>("Enter the new {0} name.".FormatWith(elementName), "Edit {0}".FormatWith(elementName), "Save");
                input.InputControl.Text = GetDisplayMember(element);
                if (input.ShowDialog() == DialogResult.OK)
                {
                    string value = input.InputControl.Text.Trim();
                    if (!value.IsNullOrEmpty())
                    {
                        saveElement(element, input.InputControl.Text);
                        SyncData(true, true);
                    }
                }
            }
        }

        private string GetDisplayMember(T element)
        {
            return element.GetType().GetProperty(displayMember).GetValue(element, null).ToString();
        }
    }
}
