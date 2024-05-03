// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

using MaterialSkin;
using MaterialSkin.Controls;

namespace PocPlayground
{
    public partial class MainForm : MaterialForm
    {
        public MainForm()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //var examplesList = ExampleLibraryManager.GetExampleModels();
            var examplesList = ExampleLibraryManager.GetExampleModelsFromYaml();

            foreach (var example in examplesList)
            {
                flowLayoutPanel1.Examples.Add(new ExampleUserControl()
                {
                    Title = example.Title,
                    Summary = example.Synopsis
                });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
