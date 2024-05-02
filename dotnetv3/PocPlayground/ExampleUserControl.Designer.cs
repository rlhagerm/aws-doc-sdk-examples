// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

using MaterialSkin.Controls;

namespace PocPlayground
{
    partial class ExampleUserControl
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
            btnViewCode = new MaterialButton();
            btnRun = new MaterialButton();
            lblTitle = new MaterialLabel();
            lnkReadme = new LinkLabel();
            lblSummary = new MaterialLabel();
            panel1 = new Panel();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // btnViewCode
            // 
            btnViewCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnViewCode.AutoSize = false;
            btnViewCode.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnViewCode.Density = MaterialButton.MaterialButtonDensity.Default;
            btnViewCode.Depth = 0;
            btnViewCode.HighEmphasis = true;
            btnViewCode.Icon = null;
            btnViewCode.Location = new Point(0, 7);
            btnViewCode.Margin = new Padding(20);
            btnViewCode.MouseState = MaterialSkin.MouseState.HOVER;
            btnViewCode.Name = "btnViewCode";
            btnViewCode.NoAccentTextColor = Color.Empty;
            btnViewCode.Size = new Size(120, 36);
            btnViewCode.TabIndex = 0;
            btnViewCode.Text = "View Code";
            btnViewCode.Type = MaterialButton.MaterialButtonType.Contained;
            btnViewCode.UseAccentColor = false;
            btnViewCode.UseVisualStyleBackColor = true;
            btnViewCode.Click += btnViewCode_Click;
            // 
            // btnRun
            // 
            btnRun.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRun.AutoSize = false;
            btnRun.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnRun.Density = MaterialButton.MaterialButtonDensity.Default;
            btnRun.Depth = 0;
            btnRun.HighEmphasis = true;
            btnRun.Icon = null;
            btnRun.Location = new Point(0, 64);
            btnRun.Margin = new Padding(20);
            btnRun.MouseState = MaterialSkin.MouseState.HOVER;
            btnRun.Name = "btnRun";
            btnRun.NoAccentTextColor = Color.Empty;
            btnRun.Size = new Size(120, 36);
            btnRun.TabIndex = 1;
            btnRun.Text = "Run Example";
            btnRun.Type = MaterialButton.MaterialButtonType.Contained;
            btnRun.UseAccentColor = false;
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Depth = 0;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Roboto", 24F, FontStyle.Bold, GraphicsUnit.Pixel);
            lblTitle.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
            lblTitle.HighEmphasis = true;
            lblTitle.Location = new Point(10, 10);
            lblTitle.MouseState = MaterialSkin.MouseState.HOVER;
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(48, 29);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Title";
            // 
            // lnkReadme
            // 
            lnkReadme.AutoSize = true;
            lnkReadme.Dock = DockStyle.Bottom;
            lnkReadme.Location = new Point(10, 85);
            lnkReadme.Name = "lnkReadme";
            lnkReadme.Size = new Size(60, 15);
            lnkReadme.TabIndex = 3;
            lnkReadme.TabStop = true;
            lnkReadme.Text = "linkLabel1";
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Depth = 0;
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            lblSummary.Location = new Point(10, 39);
            lblSummary.MouseState = MaterialSkin.MouseState.HOVER;
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(44, 19);
            lblSummary.TabIndex = 4;
            lblSummary.Text = "label1";
            // 
            // panel1
            // 
            panel1.Controls.Add(btnViewCode);
            panel1.Controls.Add(btnRun);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(661, 10);
            panel1.Name = "panel1";
            panel1.Size = new Size(125, 110);
            panel1.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblSummary);
            panel2.Controls.Add(lblTitle);
            panel2.Controls.Add(lnkReadme);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(10, 10);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(10);
            panel2.Size = new Size(651, 110);
            panel2.TabIndex = 6;
            // 
            // ExampleUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ExampleUserControl";
            Padding = new Padding(10);
            Size = new Size(796, 130);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MaterialButton btnViewCode;
        private MaterialButton btnRun;
        private MaterialLabel lblTitle;
        private LinkLabel lnkReadme;
        private MaterialLabel lblSummary;
        private Panel panel1;
        private Panel panel2;
    }
}
