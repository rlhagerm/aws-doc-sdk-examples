// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PocPlayground
{
    public partial class ExampleUserControl : UserControl
    {
        public ExampleUserControl()
        {
            InitializeComponent();
        }

        private void btnViewCode_Click(object sender, EventArgs e)
        {
            ExampleLibraryManager.ViewCode("test");
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            ExampleLibraryManager.RunExample("test");
        }

        private void lnkReadme_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.lnkReadme.LinkVisited = true;

            // Navigate to a URL

            System.Diagnostics.Process.Start("explorer.exe",@"https://github.com/awsdocs/aws-doc-sdk-examples/tree/main/workflows/s3_object_lock");
        }

        public string Title
        {
            get => lblTitle.Text;
            set
            {
                if (!Equals(lblTitle.Text, value))
                {
                    lblTitle.Text = value;
                }
            }
        }

        public string Summary
        {
            get => lblSummary.Text;
            set
            {
                if (!Equals(lblSummary.Text, value))
                {
                    lblSummary.Text = value;
                }
            }
        }
    }
}
