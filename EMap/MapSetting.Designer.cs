namespace EMap
{
    partial class MapSetting
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelFunction = new System.Windows.Forms.Panel();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAddMap = new System.Windows.Forms.Button();
            this.panelTree = new System.Windows.Forms.Panel();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.panelSetting = new System.Windows.Forms.Panel();
            this.panelSpace = new System.Windows.Forms.Panel();
            this.panelFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFunction
            // 
            this.panelFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.panelFunction.Controls.Add(this.buttonDelete);
            this.panelFunction.Controls.Add(this.buttonAddMap);
            this.panelFunction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFunction.Location = new System.Drawing.Point(0, 42);
            this.panelFunction.Name = "panelFunction";
            this.panelFunction.Size = new System.Drawing.Size(220, 40);
            this.panelFunction.TabIndex = 2;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(113, 14);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(104, 26);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDeleteClick);
            // 
            // buttonAddMap
            // 
            this.buttonAddMap.Location = new System.Drawing.Point(3, 14);
            this.buttonAddMap.Name = "buttonAddMap";
            this.buttonAddMap.Size = new System.Drawing.Size(104, 26);
            this.buttonAddMap.TabIndex = 0;
            this.buttonAddMap.Text = "Add";
            this.buttonAddMap.UseVisualStyleBackColor = true;
            this.buttonAddMap.Click += new System.EventHandler(this.BtnAddMapClick);
            // 
            // panelTree
            // 
            this.panelTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.panelTree.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTree.Location = new System.Drawing.Point(0, 96);
            this.panelTree.Name = "panelTree";
            this.panelTree.Size = new System.Drawing.Size(220, 250);
            this.panelTree.TabIndex = 3;
            // 
            // panelTitle
            // 
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(220, 42);
            this.panelTitle.TabIndex = 0;
            // 
            // panelSetting
            // 
            this.panelSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSetting.Location = new System.Drawing.Point(0, 346);
            this.panelSetting.Name = "panelSetting";
            this.panelSetting.Size = new System.Drawing.Size(220, 290);
            this.panelSetting.TabIndex = 5;
            // 
            // panelSpace
            // 
            this.panelSpace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.panelSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSpace.Location = new System.Drawing.Point(0, 82);
            this.panelSpace.Name = "panelSpace";
            this.panelSpace.Size = new System.Drawing.Size(220, 14);
            this.panelSpace.TabIndex = 4;
            // 
            // MapSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.panelSetting);
            this.Controls.Add(this.panelTree);
            this.Controls.Add(this.panelSpace);
            this.Controls.Add(this.panelFunction);
            this.Controls.Add(this.panelTitle);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MapSetting";
            this.Size = new System.Drawing.Size(220, 636);
            this.panelFunction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFunction;
        private System.Windows.Forms.Panel panelTree;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Button buttonAddMap;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Panel panelSetting;
        private System.Windows.Forms.Panel panelSpace;
    }
}
