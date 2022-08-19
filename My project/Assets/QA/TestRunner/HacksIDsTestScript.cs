using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using UnityEngine.InputSystem;


public class HacksIDsTestScript : InputTestFixture
{
    #region Fields
    
//private UI_MainMenu uiMainMenu;
    private GameObject _uiManager;
    private GameObject _circularLoad;
    private GameObject ship;

   // private PlayerMovement playerController;
    Keyboard gamepad;

    #endregion


    [SetUp]
    public override void Setup()
    {
        base.Setup();
        //gamepad = InputSystem.AddDevice<Keyboard>();
        EditorSceneManager.LoadScene("sce_lander4_mission3");

    }   


    [UnityTest]
    public IEnumerator FullFlowTest()
    {

        yield return new WaitForSeconds(3f);

    }

    public IEnumerator CircularLoad()
    {
        yield return null;
    }

    [TearDown]
    public override void TearDown()
    {
        base.TearDown();
        //EditorSceneManager.UnloadSceneAsync("scm_mainMenu");
    }

    public void CreateAgame()
    {
        PressAccept(5);
    }

    public void SelectLuna()
    {
        PressRightArrow(1);        
    }

    public void SelectCargo()
    {
        PressAccept(6);
    }


    #region Input
    public void PressAccept(int times)
    {
        int count = 0;
        while( count != times)
        {
            Press(gamepad.spaceKey);
            Release(gamepad.spaceKey);
            count++;
        }
    }

    public void PressRightArrow(int times)
    {
        int count = 0;
        while (count != times)
        {
            Press(gamepad.rightArrowKey);
            Release(gamepad.rightArrowKey);
            count++;
        }
    }
    #endregion
}

