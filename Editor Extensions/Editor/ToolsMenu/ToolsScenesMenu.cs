
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ToolsScenesMenu : MonoBehaviour {

	static string scenePath;


	// adds a menu item which gives a brief summary of currently open scenes (from the Unity Documentation)
	[MenuItem("Tools/Scenes/Scene Summary")]
	public static void SummarizeScenes()
	{
			string output = "";
			if (EditorSceneManager.sceneCountInBuildSettings > 0)
			{
					for (int n = 0; n < EditorSceneManager.sceneCountInBuildSettings; ++n)
					{
							Scene scene = EditorSceneManager.GetSceneByBuildIndex(n);
							if (scene.IsValid()) {
								output += scene.name;
								output += scene.isLoaded ? " (Opened, " : " (Not Opened, ";
								output += scene.isDirty ? "Dirty, " : "Clean, ";
								output += scene.buildIndex >= 0 ? " in build)\n" : " NOT in build)\n";
							}
					}
			}
			else
			{
					output = "No open scenes.";
			}
			EditorUtility.DisplayDialog("Scene Summary", output, "Ok");
	}

	//Open Previous Scene
	[MenuItem("Tools/Scenes/Open Previous Scene #&-", true)]
	private static bool OpenPreviousSceneValidation()
	{
		int currentBuildID = EditorSceneManager.GetActiveScene().buildIndex - 1;
		return SceneUtility.GetScenePathByBuildIndex(currentBuildID) != "";
	}

	[MenuItem("Tools/Scenes/Open Previous Scene #&-")]
	public static void OpenPreviousScene()
	{
		int currentBuildID = EditorSceneManager.GetActiveScene().buildIndex - 1;
		scenePath = SceneUtility.GetScenePathByBuildIndex(currentBuildID);
		EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Next Scene
	[MenuItem("Tools/Scenes/Open Next Scene #&+", true)]
	private static bool OpenNextSceneValidation()
	{
		int currentBuildID = EditorSceneManager.GetActiveScene().buildIndex + 1;
		return SceneUtility.GetScenePathByBuildIndex(currentBuildID) != "";
	}

	[MenuItem("Tools/Scenes/Open Next Scene #&+")]
	public static void OpenNextScene()
	{
		int currentBuildID = EditorSceneManager.GetActiveScene().buildIndex + 1;
		scenePath = SceneUtility.GetScenePathByBuildIndex(currentBuildID);
		EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open First Scene
	[MenuItem("Tools/Scenes/Open First Scene #&1", true)]
	private static bool OpenFirstSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(0) != "";
	}

	[MenuItem("Tools/Scenes/Open First Scene #&1")]
	public static void OpenFirstScene()
	{
	  scenePath = SceneUtility.GetScenePathByBuildIndex(0);
	  EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Second Scene
	[MenuItem("Tools/Scenes/Open Second Scene #&2", true)]
	private static bool OpenSecondSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(1) != "";
	}

	[MenuItem("Tools/Scenes/Open Second Scene #&2")]
	public static void OpenSecondScene()
	{
	  scenePath = SceneUtility.GetScenePathByBuildIndex(1);
	  EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Third Scene
	[MenuItem("Tools/Scenes/Open Third Scene #&3", true)]
	private static bool OpenThirdSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(2) != "";
	}

	[MenuItem("Tools/Scenes/Open Third Scene #&3")]
	public static void OpenThirdScene()
	{
	  scenePath = SceneUtility.GetScenePathByBuildIndex(2);
	  EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Fourth Scene
	[MenuItem("Tools/Scenes/Open Fourth Scene #&4", true)]
	private static bool OpenFourthSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(3) != "";
	}

	[MenuItem("Tools/Scenes/Open Fourth Scene #&4")]
	public static void OpenFourthScene()
	{
	  scenePath = SceneUtility.GetScenePathByBuildIndex(3);
	  EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Fifth Scene
	[MenuItem("Tools/Scenes/Open Fifth Scene #&5", true)]
	private static bool OpenFifthSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(4) != "";
	}

	[MenuItem("Tools/Scenes/Open Fifth Scene #&5")]
	public static void OpenFifthScene()
	{
	  scenePath = SceneUtility.GetScenePathByBuildIndex(4);
	  EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Sixth Scene
	[MenuItem("Tools/Scenes/Open Sixth Scene #&6", true)]
	private static bool OpenSixthSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(5) != "";
	}

	[MenuItem("Tools/Scenes/Open Sixth Scene #&6")]
	public static void OpenSixthScene()
	{
	  scenePath = SceneUtility.GetScenePathByBuildIndex(5);
	  EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Seventh Scene
	[MenuItem("Tools/Scenes/Open Seventh Scene #&7", true)]
	private static bool OpenSeventhSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(6) != "";
	}

	[MenuItem("Tools/Scenes/Open Seventh Scene #&7")]
	public static void OpenSeventhScene()
	{
		scenePath = SceneUtility.GetScenePathByBuildIndex(6);
		EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Eigth Scene
	[MenuItem("Tools/Scenes/Open Eigth Scene #&8", true)]
	private static bool OpenEigthSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(7) != "";
	}

	[MenuItem("Tools/Scenes/Open Eigth Scene #&8")]
	public static void OpenEigthScene()
	{
		scenePath = SceneUtility.GetScenePathByBuildIndex(7);
		EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Ninth Scene
	[MenuItem("Tools/Scenes/Open Ninth Scene #&9", true)]
	private static bool OpenNinthSceneValidation()
	{
		return SceneUtility.GetScenePathByBuildIndex(8) != "";
	}

	[MenuItem("Tools/Scenes/Open Ninth Scene #&9")]
	public static void OpenNinthScene()
	{
		scenePath = SceneUtility.GetScenePathByBuildIndex(8);
		EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

	//Open Last Scene
	[MenuItem("Tools/Scenes/Open Last Scene #&0", true)]
	private static bool OpenLastSceneValidation()
	{
		int tmp = EditorSceneManager.sceneCountInBuildSettings - 1;
		return SceneUtility.GetScenePathByBuildIndex(tmp) != "";
	}

	[MenuItem("Tools/Scenes/Open Last Scene #&0")]
	public static void OpenLastScene()
	{
		int tmp = EditorSceneManager.sceneCountInBuildSettings - 1;
		scenePath = SceneUtility.GetScenePathByBuildIndex(tmp);
		EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
	}

}
