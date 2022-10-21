// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

namespace AuroraItemTracker;

/// <summary>
/// Work item object.
/// </summary>
public class WorkItem
{
    /// <summary>
    /// Id of the work item.
    /// </summary>
    public string IdItem { get; set; } = null!;

    /// <summary>
    /// Date the item was created;
    /// </summary>
    // public DateTime? Date { get; set; }

    /// <summary>
    /// Description of the work item.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// The guide for the work item.
    /// </summary>
    public string Guide { get; set; } = null!;

    /// <summary>
    /// User name for the work item;
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The current status of the work item.
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// The archive state of the work item.
    /// </summary>
    public ArchiveState Archived { get; set; }
}