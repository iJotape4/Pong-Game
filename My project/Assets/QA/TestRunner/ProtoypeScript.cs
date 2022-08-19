using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;

public class ProtoypeScript 
{
    #region Fields
   // private GameObject ship;
    //private PlayerMovement playerController;   
    #endregion


    [SetUp]
    public void SetUp()
    {
        EditorSceneManager.LoadScene("sce_lander_mission_1");
    }   


    [UnityTest]
    public IEnumerator ProtoypeScriptsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;


        yield return new WaitForSeconds(10f);
    }

    [TearDown]
    public void TearDown()
    {
        EditorSceneManager.UnloadSceneAsync("sce_lander_mission_1");
    }
    
}
