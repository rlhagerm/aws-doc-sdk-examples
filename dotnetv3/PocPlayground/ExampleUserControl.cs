// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
