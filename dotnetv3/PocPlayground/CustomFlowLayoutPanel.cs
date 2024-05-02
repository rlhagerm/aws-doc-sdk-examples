// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved. 
// SPDX-License-Identifier: Apache-2.0

using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace PocPlayground;

class CustomFlowLayoutPanel : FlowLayoutPanel
{
    public CustomFlowLayoutPanel()
    {
        AutoScroll = true;
        Examples.ListChanged += (sender, e) =>
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Controls.Add(Examples[e.NewIndex]);
                    //Examples[e.NewIndex].AutoSize = true;
                    Examples[e.NewIndex].Width = this.Width - 10;
                    Examples[e.NewIndex].Anchor = AnchorStyles.Right | AnchorStyles.Top;
                    break;
                default:
                    break;
            }
        };
    }
    public BindingList<ExampleUserControl> Examples = new BindingList<ExampleUserControl>();

    internal void Search(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            foreach (var example in Examples)
            {
                example.Visible = true;
            }
        }
        else
        {
            foreach (var example in Examples)
            {
                example.Visible =
                    example.Summary.Contains(
                        text,
                        StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}