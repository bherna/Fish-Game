
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


    void OnEnable() {
        //setup serializable properties
        timeSpeedProp = serializedObject.FindProperty("timeSpeed");
        moneyProp = serializedObject.FindProperty("money");
    }



    //all of our inspector changes are going to be in here, if we need a feature: we add it in testfunctions
    public override void OnInspectorGUI()
    {
        //always do this at begining of oninspectorGUI
        serializedObject.Update();

  
        GUILayout.Label(string.Format("Update our time speed here: "));
        EditorGUILayout.IntSlider(timeSpeedProp, 1, 100);
        TestFunctions tf = (TestFunctions)target;
        if(GUILayout.Button("Update Time")){
            tf.UpdateTimeSpeed();
        }
 

        GUILayout.Label(string.Format("How much money to give self: "));
        EditorGUILayout.IntSlider(moneyProp, 100, 100000);
        if(GUILayout.Button("Give Money")){
            tf.GiveMoney();
        }



        //assuming tester doesn't have insane reflexes, we can update variables last
        //apply changes to variables
        serializedObject.ApplyModifiedProperties(); 

        

    }

}
