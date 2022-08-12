using UnityEngine;

public class VersionWindowCreator : MonoBehaviour
{
   public GameObject ConfigureVersionWindowObject()
    {
        GameObject windowVersion = new GameObject("GUIcommit");

        windowVersion.AddComponent<GUICommitTest>();
        return windowVersion;
    }
}

public class Debuginstantiator
{
    //This method runs when every scene loads, in order to add to the scene the object with Window Version Script
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntime()
    {
        CreateDebugVersionWindow();       
    }

    static void CreateDebugVersionWindow()
    {
        VersionWindowCreator versionWindow = new VersionWindowCreator();
        var window = versionWindow.ConfigureVersionWindowObject();
    }

    
}
