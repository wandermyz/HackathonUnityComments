﻿using UnityEngine.InputNew;

namespace UnityEngine.Experimental.EditorVR.Tools
{
	/// <summary>
	/// Provided to a tool for device input (e.g. position / rotation)
	/// </summary>
	public interface ITrackedObjectActionMap
	{
		TrackedObject trackedObjectInput { set; }
	}
}