#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


// This whole script is made to make it easier to test features within a level
//       Make sure to remove this script/ gameobject from game
// --



//This class is what shows up in the inspector for tester
//for each editable feature in the game, we do it from here
[CustomEditor(typeof(TestFunctions))]
public class TestFunctionsEditor : Editor
{

    //all variables that get updated
    SerializedProperty timeSpeedProp;
    SerializedProperty moneyProp;
    PetNames petProp;
    SerializedProperty comboProp;

    enum EnemyNames {Starfish, LMBass};
    EnemyNames enemyProp;


    void OnEnable() {
        //setup serializable properties
        timeSpeedProp = serializedObject.FindProperty("timeSpeed");
        moneyProp = serializedObject.FindProperty("money");
        comboProp = serializedObject.FindProperty("comboAmount");
    }



    //all of our inspector changes are going to be in here, if we need a feature: we add it in testfunctions
    public override void OnInspectorGUI()
    {
        //always do this at begining of oninspectorGUI
        serializedObject.Update();



  //time
        GUILayout.Label(string.Format("Update our time speed here: "));
        EditorGUILayout.IntSlider(timeSpeedProp, 1, 100);
        TestFunctions tf = (TestFunctions)target;
        if(GUILayout.Button("Update Time")){
            tf.UpdateTimeSpeed();
        }

        GUILayout.Label(string.Format("\n\n"));//new line

 //money
        GUILayout.Label(string.Format("How much money to give self: "));
        EditorGUILayout.IntSlider(moneyProp, 100, 100000);
        if(GUILayout.Button("Give Money")){
            tf.GiveMoney();
        }

        GUILayout.Label(string.Format("\n\n"));//new line

//pets
        GUILayout.Label(string.Format("Spawn Pet: "));
        petProp = (PetNames)EditorGUILayout.EnumPopup(petProp);
        if(GUILayout.Button("Spawn Pet")){
            tf.SpawnPet(petProp);
        }

        GUILayout.Label(string.Format("\n\n"));//new line

//combo 
        GUILayout.Label(string.Format("Add combo: "));
        EditorGUILayout.IntSlider(comboProp, 1, 10);
        if(GUILayout.Button("Add to Combo")){
            tf.AddToCombo();
        }

        GUILayout.Label(string.Format("\n\n"));//new line

//guppy
        if(GUILayout.Button("Age Guppies")){
            tf.AddAge();
        }

        GUILayout.Label(string.Format("\n\n"));//new line


//tank
        if(GUILayout.Button("Shake Screen")){
            tf.ShakeScreen();
        }

        GUILayout.Label(string.Format("\n\n"));//new line


//enemy
        GUILayout.Label(string.Format("Spawn Enemy: "));
        enemyProp = (EnemyNames)EditorGUILayout.EnumPopup(enemyProp);
        if(GUILayout.Button(string.Format("Spawn {0}", enemyProp.ToString()))){
            tf.SpawnEnemy(enemyProp.ToString());
        }

        GUILayout.Label(string.Format("\n\n"));//new line



        //assuming tester doesn't have insane reflexes, we can update variables last
        //apply changes to variables
        serializedObject.ApplyModifiedProperties(); 

    }

}

#endif
