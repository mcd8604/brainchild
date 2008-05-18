using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Physics2;
using System.ComponentModel.Design;

namespace Project_blob
{
    public class TaskEditor : Form
    {
        private PropertyGrid propertyGrid1;
        private Button AddButton;
        private Button RemoveButton;
        private ListBox taskList;
        private ComboBox TaskTypeCB;

        private IList<Task> m_Tasks;
		public IList<Task> Tasks
		{
			get
			{
				return m_Tasks;
			}
		}

        public TaskEditor() { }

        public TaskEditor(IList<Task> tasks)
        {
            m_Tasks = tasks;
            InitializeComponent();
            UpdateTaskList();

            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom("Physics2.dll");
            foreach (Type t in asm.GetTypes())
            {
                if (t.IsSubclassOf(typeof(Task)))
                {
                    TaskTypeCB.Items.Add(t);
                }
            }
        }

        private void InitializeComponent()
        {
            this.taskList = new System.Windows.Forms.ListBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.TaskTypeCB = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // taskList
            // 
            this.taskList.FormattingEnabled = true;
            this.taskList.Location = new System.Drawing.Point(12, 12);
            this.taskList.Name = "taskList";
            this.taskList.Size = new System.Drawing.Size(120, 199);
            this.taskList.TabIndex = 0;
            this.taskList.SelectedIndexChanged += new System.EventHandler(this.taskList_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(138, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(231, 249);
            this.propertyGrid1.TabIndex = 1;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(12, 238);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(49, 23);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(67, 238);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(65, 23);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // TaskTypeCB
            // 
            this.TaskTypeCB.FormattingEnabled = true;
            this.TaskTypeCB.Location = new System.Drawing.Point(12, 211);
            this.TaskTypeCB.Name = "TaskTypeCB";
            this.TaskTypeCB.Size = new System.Drawing.Size(120, 21);
            this.TaskTypeCB.TabIndex = 4;
            // 
            // TaskEditor
            // 
            this.ClientSize = new System.Drawing.Size(381, 273);
            this.Controls.Add(this.TaskTypeCB);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.taskList);
            this.Name = "TaskEditor";
            this.ResumeLayout(false);

        }

        private void UpdateTaskList()
        {
            taskList.Items.Clear();
            foreach (Task t in m_Tasks)
            {
                taskList.Items.Add(t);
            }
            taskList.Update();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (TaskTypeCB.SelectedIndex != -1)
            {
                m_Tasks.Add(((Type)TaskTypeCB.SelectedItem).GetConstructor(System.Type.EmptyTypes).Invoke(null) as Task);
                UpdateTaskList();
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {

            if (taskList.SelectedIndex != -1)
            {
                m_Tasks.Remove(taskList.SelectedItem as Task);
                UpdateTaskList();
            }
        }

        private void taskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (taskList.SelectedIndex != -1)
            {
                propertyGrid1.SelectedObject = taskList.SelectedItem;
                propertyGrid1.Update();
            }
        }
    }
}
