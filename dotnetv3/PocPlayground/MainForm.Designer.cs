// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

using MaterialSkin.Controls;

namespace PocPlayground
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var treeNode1 = new TreeNode("Node0");
            var treeNode2 = new TreeNode("Node12");
            var treeNode3 = new TreeNode("ACM", new TreeNode[] { treeNode1, treeNode2 });
            var treeNode4 = new TreeNode("Node13");
            var treeNode5 = new TreeNode("Node14");
            var treeNode6 = new TreeNode("API Gateway", new TreeNode[] { treeNode4, treeNode5 });
            var treeNode7 = new TreeNode("Node15");
            var treeNode8 = new TreeNode("Node16");
            var treeNode9 = new TreeNode("Aurora", new TreeNode[] { treeNode7, treeNode8 });
            var treeNode10 = new TreeNode("Node17");
            var treeNode11 = new TreeNode("Node18");
            var treeNode12 = new TreeNode("Auto Scaling", new TreeNode[] { treeNode10, treeNode11 });
            var treeNode13 = new TreeNode("Node19");
            var treeNode14 = new TreeNode("Node20");
            var treeNode15 = new TreeNode("AWS Batch", new TreeNode[] { treeNode13, treeNode14 });
            var treeNode16 = new TreeNode("Node21");
            var treeNode17 = new TreeNode("Node22");
            var treeNode18 = new TreeNode("CloudFront", new TreeNode[] { treeNode16, treeNode17 });
            var treeNode19 = new TreeNode("Node23");
            var treeNode20 = new TreeNode("Node24");
            var treeNode21 = new TreeNode("CloudTrail", new TreeNode[] { treeNode19, treeNode20 });
            var treeNode22 = new TreeNode("Node25");
            var treeNode23 = new TreeNode("Node26");
            var treeNode24 = new TreeNode("CloudWatch", new TreeNode[] { treeNode22, treeNode23 });
            var treeNode25 = new TreeNode("Actions");
            var treeNode26 = new TreeNode("Scenarios");
            var treeNode27 = new TreeNode("Amazon S3", new TreeNode[] { treeNode25, treeNode26 });
            var treeNode28 = new TreeNode("Node27");
            var treeNode29 = new TreeNode("Node28");
            var treeNode30 = new TreeNode("Amazon Redshift", new TreeNode[] { treeNode28, treeNode29 });
            textBox1 = new MaterialTextBox();
            button1 = new MaterialButton();
            button2 = new MaterialButton();
            flowLayoutPanel1 = new CustomFlowLayoutPanel();
            flowLayoutPanel2 = new CustomFlowLayoutPanel();
            panel1 = new Panel();
            treeView1 = new TreeView();
            flowLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.AnimateReadOnly = false;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Depth = 0;
            textBox1.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            textBox1.LeadingIcon = null;
            textBox1.Location = new Point(10, 10);
            textBox1.Margin = new Padding(10);
            textBox1.MaxLength = 50;
            textBox1.MouseState = MaterialSkin.MouseState.OUT;
            textBox1.Multiline = false;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(796, 50);
            textBox1.TabIndex = 0;
            textBox1.Text = "";
            textBox1.TrailingIcon = null;
            // 
            // button1
            // 
            button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button1.Density = MaterialButton.MaterialButtonDensity.Default;
            button1.Depth = 0;
            button1.HighEmphasis = true;
            button1.Icon = null;
            button1.Location = new Point(934, 18);
            button1.Margin = new Padding(10);
            button1.MouseState = MaterialSkin.MouseState.HOVER;
            button1.Name = "button1";
            button1.NoAccentTextColor = Color.Empty;
            button1.Padding = new Padding(5);
            button1.Size = new Size(78, 36);
            button1.TabIndex = 1;
            button1.Text = "Search";
            button1.Type = MaterialButton.MaterialButtonType.Contained;
            button1.UseAccentColor = false;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button2.Density = MaterialButton.MaterialButtonDensity.Default;
            button2.Depth = 0;
            button2.HighEmphasis = true;
            button2.Icon = null;
            button2.Location = new Point(826, 18);
            button2.Margin = new Padding(10);
            button2.MouseState = MaterialSkin.MouseState.HOVER;
            button2.Name = "button2";
            button2.NoAccentTextColor = Color.Empty;
            button2.Size = new Size(90, 36);
            button2.TabIndex = 3;
            button2.Text = "Settings";
            button2.Type = MaterialButton.MaterialButtonType.Contained;
            button2.UseAccentColor = false;
            button2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Location = new Point(147, 138);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(868, 309);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoScroll = true;
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(3, 3);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(200, 0);
            flowLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(3, 64);
            panel1.Margin = new Padding(10);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10);
            panel1.Size = new Size(1026, 74);
            panel1.TabIndex = 5;
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            treeView1.Location = new Point(3, 141);
            treeView1.Name = "treeView1";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Node0";
            treeNode2.Name = "Node12";
            treeNode2.Text = "Node12";
            treeNode3.Name = "Services";
            treeNode3.Text = "ACM";
            treeNode4.Name = "Node13";
            treeNode4.Text = "Node13";
            treeNode5.Name = "Node14";
            treeNode5.Text = "Node14";
            treeNode6.Name = "Node1";
            treeNode6.Text = "API Gateway";
            treeNode7.Name = "Node15";
            treeNode7.Text = "Node15";
            treeNode8.Name = "Node16";
            treeNode8.Text = "Node16";
            treeNode9.Name = "Node2";
            treeNode9.Text = "Aurora";
            treeNode10.Name = "Node17";
            treeNode10.Text = "Node17";
            treeNode11.Name = "Node18";
            treeNode11.Text = "Node18";
            treeNode12.Name = "Node3";
            treeNode12.Text = "Auto Scaling";
            treeNode13.Name = "Node19";
            treeNode13.Text = "Node19";
            treeNode14.Name = "Node20";
            treeNode14.Text = "Node20";
            treeNode15.Name = "Node4";
            treeNode15.Text = "AWS Batch";
            treeNode16.Name = "Node21";
            treeNode16.Text = "Node21";
            treeNode17.Name = "Node22";
            treeNode17.Text = "Node22";
            treeNode18.Name = "Node5";
            treeNode18.Text = "CloudFront";
            treeNode19.Name = "Node23";
            treeNode19.Text = "Node23";
            treeNode20.Name = "Node24";
            treeNode20.Text = "Node24";
            treeNode21.Name = "Node6";
            treeNode21.Text = "CloudTrail";
            treeNode22.Name = "Node25";
            treeNode22.Text = "Node25";
            treeNode23.Name = "Node26";
            treeNode23.Text = "Node26";
            treeNode24.Name = "Node7";
            treeNode24.Text = "CloudWatch";
            treeNode25.Name = "Node10";
            treeNode25.Text = "Actions";
            treeNode26.Name = "Node11";
            treeNode26.Text = "Scenarios";
            treeNode27.Name = "Node8";
            treeNode27.Text = "Amazon S3";
            treeNode28.Name = "Node27";
            treeNode28.Text = "Node27";
            treeNode29.Name = "Node28";
            treeNode29.Text = "Node28";
            treeNode30.Name = "Node9";
            treeNode30.Text = "Amazon Redshift";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode3, treeNode6, treeNode9, treeNode12, treeNode15, treeNode18, treeNode21, treeNode24, treeNode27, treeNode30 });
            treeView1.Size = new Size(138, 306);
            treeView1.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1032, 450);
            Controls.Add(treeView1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "AWS SDK Code Example Playground";
            flowLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MaterialTextBox textBox1;
        private MaterialButton button1;
        private MaterialButton button2;
        private CustomFlowLayoutPanel flowLayoutPanel1;
        private Panel panel1;
        private CustomFlowLayoutPanel flowLayoutPanel2;
        private TreeView treeView1;
    }
}
