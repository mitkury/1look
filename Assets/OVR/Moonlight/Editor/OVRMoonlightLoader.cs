/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

[InitializeOnLoad]
class OVRMoonlightLoader
{
    static OVRMoonlightLoader()
	{
		if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android || Application.platform != RuntimePlatform.Android)
			return;

		// Default screen orientation must be set to landscape left.
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

		// NOTE: On Adreno Lollipop, it is an error to have antiAliasing set on the
		// main window surface with front buffer rendering enabled. The view will
		// render black.
		// On Adreno KitKat, some tiling control modes will cause the view to render
		// black.
		QualitySettings.antiAliasing = 1;

		// We sync in the TimeWarp, so we don't want unity syncing elsewhere.
		QualitySettings.vSyncCount = 0;

		Debug.Log("OVRMoonlightLoader: Apply settings required for Android Mobile VR");
	}
}
