using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using PlanetLander.GameManagement.LanderSystem;

public class ProtoypeScript 
{
    #region Fields
    private GameObject ship;
    private PlayerMovement playerController;   
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

        ship = GameObject.Find("pf_ParentLanderMantis(Clone)");
        playerController = ship.GetComponent<PlayerMovement>();
        playerController.UpdatePropeller(10f);     

        yield return new WaitForSeconds(10f);
        Assert.That(ship != null);
    }

    [TearDown]
    public void TearDown()
    {
        EditorSceneManager.UnloadSceneAsync("sce_lander_mission_1");
    }
}
