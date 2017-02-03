﻿using System.Collections.Generic;

/// <summary>
/// Exposes a property used to provide a hierarchy of scene objects to the object
/// </summary>
public interface IUsesHierarchyData
{
	/// <summary>
	/// Set accessor for hierarchy list data
	/// Used to update existing implementors after lazy load completes
	/// </summary>
	List<HierarchyData> hierarchyData { set; }
}