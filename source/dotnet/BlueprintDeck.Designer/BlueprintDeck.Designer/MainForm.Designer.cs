namespace BlueprintDeck.Designer
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rightPanel = new System.Windows.Forms.Panel();
            this.midPanel = new System.Windows.Forms.Panel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.displayCodeButton = new System.Windows.Forms.Button();
            this.blueprintDesigner1 = new BlueprintDeck.Designer.BlueprintDesigner();
            this.codeTabController = new System.Windows.Forms.TabControl();
            this.blueprintCodeTabPage = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.applyCodeButton = new System.Windows.Forms.Button();
            this.txtBlueprint = new System.Windows.Forms.TextBox();
            this.txtNodes = new System.Windows.Forms.TextBox();
            this.rightPanel.SuspendLayout();
            this.midPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.codeTabController.SuspendLayout();
            this.blueprintCodeTabPage.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.applyCodeButton);
            this.rightPanel.Controls.Add(this.codeTabController);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Location = new System.Drawing.Point(662, 0);
            this.rightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(650, 720);
            this.rightPanel.TabIndex = 1;
            // 
            // midPanel
            // 
            this.midPanel.Controls.Add(this.blueprintDesigner1);
            this.midPanel.Controls.Add(this.headerPanel);
            this.midPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.midPanel.Location = new System.Drawing.Point(0, 0);
            this.midPanel.Name = "midPanel";
            this.midPanel.Size = new System.Drawing.Size(662, 720);
            this.midPanel.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.Controls.Add(this.displayCodeButton);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(662, 45);
            this.headerPanel.TabIndex = 1;
            // 
            // displayCodeButton
            // 
            this.displayCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.displayCodeButton.Location = new System.Drawing.Point(548, 12);
            this.displayCodeButton.Name = "displayCodeButton";
            this.displayCodeButton.Size = new System.Drawing.Size(102, 23);
            this.displayCodeButton.TabIndex = 0;
            this.displayCodeButton.Text = "Display Code";
            this.displayCodeButton.UseVisualStyleBackColor = true;
            this.displayCodeButton.Click += new System.EventHandler(this.displayCodeButton_Click);
            // 
            // blueprintDesigner1
            // 
            this.blueprintDesigner1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(38)))), ((int)(((byte)(53)))));
            this.blueprintDesigner1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.blueprintDesigner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueprintDesigner1.Location = new System.Drawing.Point(0, 45);
            this.blueprintDesigner1.Name = "blueprintDesigner1";
            this.blueprintDesigner1.Size = new System.Drawing.Size(662, 675);
            this.blueprintDesigner1.TabIndex = 0;
            // 
            // codeTabController
            // 
            this.codeTabController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codeTabController.Controls.Add(this.blueprintCodeTabPage);
            this.codeTabController.Controls.Add(this.tabPage2);
            this.codeTabController.Location = new System.Drawing.Point(6, 20);
            this.codeTabController.Name = "codeTabController";
            this.codeTabController.SelectedIndex = 0;
            this.codeTabController.Size = new System.Drawing.Size(641, 697);
            this.codeTabController.TabIndex = 0;
            // 
            // blueprintCodeTabPage
            // 
            this.blueprintCodeTabPage.Controls.Add(this.txtBlueprint);
            this.blueprintCodeTabPage.Location = new System.Drawing.Point(4, 22);
            this.blueprintCodeTabPage.Name = "blueprintCodeTabPage";
            this.blueprintCodeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.blueprintCodeTabPage.Size = new System.Drawing.Size(633, 671);
            this.blueprintCodeTabPage.TabIndex = 0;
            this.blueprintCodeTabPage.Text = "Blueprint";
            this.blueprintCodeTabPage.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtNodes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(633, 671);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nodes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // applyCodeButton
            // 
            this.applyCodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.applyCodeButton.Location = new System.Drawing.Point(550, 12);
            this.applyCodeButton.Name = "applyCodeButton";
            this.applyCodeButton.Size = new System.Drawing.Size(88, 23);
            this.applyCodeButton.TabIndex = 0;
            this.applyCodeButton.Text = "Übernehmen";
            this.applyCodeButton.UseVisualStyleBackColor = true;
            this.applyCodeButton.Click += new System.EventHandler(this.applyCodeButton_Click);
            // 
            // txtBlueprint
            // 
            this.txtBlueprint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBlueprint.Font = new System.Drawing.Font("Consolas", 11.25F);
            this.txtBlueprint.Location = new System.Drawing.Point(3, 3);
            this.txtBlueprint.Multiline = true;
            this.txtBlueprint.Name = "txtBlueprint";
            this.txtBlueprint.Size = new System.Drawing.Size(627, 665);
            this.txtBlueprint.TabIndex = 0;
            // 
            // txtNodes
            // 
            this.txtNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNodes.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNodes.Location = new System.Drawing.Point(3, 3);
            this.txtNodes.Multiline = true;
            this.txtNodes.Name = "txtNodes";
            this.txtNodes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNodes.Size = new System.Drawing.Size(627, 665);
            this.txtNodes.TabIndex = 1;
            this.txtNodes.Text = resources.GetString("txtNodes.Text");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 720);
            this.Controls.Add(this.midPanel);
            this.Controls.Add(this.rightPanel);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "BlueprintDeck";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.rightPanel.ResumeLayout(false);
            this.midPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.codeTabController.ResumeLayout(false);
            this.blueprintCodeTabPage.ResumeLayout(false);
            this.blueprintCodeTabPage.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BlueprintDesigner blueprintDesigner1;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel midPanel;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Button displayCodeButton;
        private System.Windows.Forms.TabControl codeTabController;
        private System.Windows.Forms.TabPage blueprintCodeTabPage;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button applyCodeButton;
        private System.Windows.Forms.TextBox txtBlueprint;
        private System.Windows.Forms.TextBox txtNodes;
    }
}

