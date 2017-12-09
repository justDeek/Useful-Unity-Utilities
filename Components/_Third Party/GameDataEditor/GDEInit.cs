using UnityEngine;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

public class GDEInit : MonoBehaviour
{
    public string GDEDataFileName;

	public bool Encrypted;

    void Reset()
    {
        GDEDataFileName = null;
    }
        
    void Awake()
    {
        try
        {
            if (!GDEDataManager.Init(GDEDataFileName, Encrypted))
			UnityEngine.Debug.LogError(GDMConstants.ErrorNotInitialized + " " + GDEDataFileName);
        }
        catch(UnityException ex)
        {
		UnityEngine.Debug.LogError(ex.ToString());
        }
    }
}

#endif
