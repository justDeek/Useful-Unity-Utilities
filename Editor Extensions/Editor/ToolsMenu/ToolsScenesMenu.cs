using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class ToolsScenesMenu : MonoBehaviour
{

	private static string scenePath;
	private static List<int> visitedScenes = new List<int>();
	private static int currentListID;

	private void Awake()
	{
		EditorSceneManager.activeSceneChanged += AddSceneToList;
	}

	static ToolsScenesMenu()
	{
		EditorSceneManager.activeSceneChanged += AddSceneToList;
	}

	private static void LoadScene(int buildID)
	{
		scenePath = SceneUtility.GetScenePathByBuildIndex(buildID);
		if(!string.IsNullOrEmpty(scenePath))
		{
			if(Application.isPlaying)
				SceneManager.LoadScene(scenePath, LoadSceneMode.Single);
			else
				EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

			EditorSceneManager.activeSceneChanged += AddSceneToList;
		}
	}

	private static void AddSceneToList(Scene lastScene, Scene currentScene)
	{
		if(!currentScene.IsValid())
			return;

		if(visitedScenes.Count == 0)
		{
			Scene initScene = SceneManager.GetActiveScene();
			visitedScenes.Add(initScene.buildIndex);
			currentListID = 0;
		} else
		{
			if(visitedScenes[visitedScenes.Count - 1] != currentScene.buildIndex)
			{
				visitedScenes.Add(currentScene.buildIndex);
				currentListID += 1;
			}
		}
	}

	// adds a menu item which gives a brief summary of currently open scenes (from the Unity Documentation)
	[MenuItem("Tools/Scenes/Scene Summary")]
	public static void SummarizeScenes()
	{
		string output = "";
		if(EditorSceneManager.sceneCountInBuildSettings > 0)
		{
			for(int n = 0; n < EditorSceneManager.sceneCountInBuildSettings; ++n)
			{
				Scene scene = EditorSceneManager.GetSceneByBuildIndex(n);
				if(scene.IsValid())
				{
					output += scene.name;
					output += scene.isLoaded ? " (Opened, " : " (Not Opened, ";
					output += scene.isDirty ? "Dirty, " : "Clean, ";
					output += scene.buildIndex >= 0 ? " in build)\n" : " NOT in build)\n";
				}
			}
		} else
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

		LoadScene(currentBuildID);
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

		LoadScene(currentBuildID);
	}

	//Open Preceeding Scene
	[MenuItem("Tools/Scenes/Open Preceding Scene #&,", true)]
	private static bool OpenPrecedingSceneValidation()
	{
		if(visitedScenes.Count > 1 && currentListID > 1)
			return true;
		else
			return false;
	}

	[MenuItem("Tools/Scenes/Open Preceding Scene #&,")]
	public static void OpenPrecedingScene()
	{
		if(currentListID > 0)
		{
			currentListID -= 1;
		}
		scenePath = SceneUtility.GetScenePathByBuildIndex(visitedScenes[currentListID]);

		if(!string.IsNullOrEmpty(scenePath) && EditorSceneManager.GetSceneByPath(scenePath) != EditorSceneManager.GetActiveScene())
		{
			if(Application.isPlaying)
				SceneManager.LoadScene(scenePath, LoadSceneMode.Single);
			else
				EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
		}
	}

	//Open Succeeding Scene
	[MenuItem("Tools/Scenes/Open Succeeding Scene #&.", true)]
	private static bool OpenSucceedingSceneValidation()
	{
		if(visitedScenes.Count > 0 && currentListID < visitedScenes.Count - 1)
			return true;
		else
			return false;
	}

	[MenuItem("Tools/Scenes/Open Succeeding Scene #&.")]
	public static void OpenSucceedingScene()
	{
		if(currentListID < visitedScenes.Count)
		{
			currentListID += 1;
		}
		scenePath = SceneUtility.GetScenePathByBuildIndex(visitedScenes[currentListID]);

		if(!string.IsNullOrEmpty(scenePath) && EditorSceneManager.GetSceneByPath(scenePath) != EditorSceneManager.GetActiveScene())
		{
			if(Application.isPlaying)
				SceneManager.LoadScene(scenePath, LoadSceneMode.Single);
			else
				EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
		}
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
		LoadScene(0);
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
		LoadScene(1);
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
		LoadScene(2);
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
		LoadScene(3);
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
		LoadScene(4);
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
		LoadScene(5);
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
		LoadScene(6);
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
		LoadScene(7);
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
		LoadScene(8);
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

		LoadScene(tmp);
	}

}
