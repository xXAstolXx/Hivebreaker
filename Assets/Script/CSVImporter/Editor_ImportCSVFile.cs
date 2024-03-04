#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CSVImporter
{
    public class Editor_ImportCSVFile : EditorWindow
    {
        private string SAVEFILENAME;

        private string lastPath = "Assets";
        private string targetLocation = "Assets";
        private string className = "";


        private List<GameObject> markedObjects = new List<GameObject>();
        private Vector2 scrollPosition;
                
        private void Awake()
        {
            SAVEFILENAME = Application.persistentDataPath + "/csvimporter.Save";
            Deserialize();
        }

        [MenuItem("Window/Import/CSVFile")]
        public static void ShowWindow()
        {
            GetWindow<Editor_ImportCSVFile>();
        }

        private void OnGUI()
        {
            #region select target location
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("target Location:", "data path where the scriptable objects should be created"), EditorStyles.boldLabel);
            targetLocation = GUILayout.TextArea(targetLocation, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            #endregion select target location

            #region select class Name
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Class Name:", "Select a class which derives from BaseImportObject. "), EditorStyles.boldLabel);
            className = GUILayout.TextArea(className, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            #endregion select class Name

            #region Import CSV File
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("import File", "select a CSV file from your computer and creates scriptable objects"), EditorStyles.boldLabel);
            if (GUILayout.Button("Import", GUILayout.Width(200)))
            {
                lastPath = EditorUtility.OpenFilePanel("Select File", lastPath, "csv");
                OpenFile(lastPath);
            }

            GUILayout.EndHorizontal();
            #endregion Import CSV File

            #region GetAllObjectsOfType
            GUILayout.BeginHorizontal();
           if (GUILayout.Button(new GUIContent("Get all NPCs", "creates a list of all objects with the NPC script on it"), GUILayout.Width(200)))
           {
               //markedObjects = new List<GameObject>();
               //foreach (var item in SceneView.FindObjectsOfType<NPC>())
               //{
               //    markedObjects.Add(item.gameObject);
               //}
           }
            GUILayout.EndHorizontal();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            foreach (var item in markedObjects)
            {
                EditorGUILayout.ObjectField(item.gameObject, typeof(GameObject), true);
            }
            GUILayout.EndScrollView();
            #endregion GetAllObjectsOfType


        }

        private void OpenFile(string path)
        {
            if (!File.Exists(path)) return;

            ImportFilesAsScriptableObjects(path);
        }

        private void ImportFilesAsScriptableObjects(string path)
        {
            int rowNumber = 0;
            foreach (var line in File.ReadAllLines(path))
            {
                rowNumber++;
                var data = CreateInstance(className) as BaseImportObject;
                if (data == null) return;
            
                var tokens = line.Split(",", StringSplitOptions.RemoveEmptyEntries);
            
                try
                {
                    data.SetupFromTokens(tokens);
                    AssetDatabase.CreateAsset(data, targetLocation + tokens[0] + ".asset");
                    AssetDatabase.SaveAssets();
            
                }
                catch (Exception e)
                {
                    Debug.LogError($"In Line: {rowNumber}. {e}");
                }
            }

            AssetDatabase.Refresh();
            Serialize();
        }

        private void Serialize()
        {
            List<string> tokens = new List<string>();
            tokens.Add(lastPath);
            tokens.Add(targetLocation);
            tokens.Add(className);


            FileStream fileStream = new FileStream(SAVEFILENAME, FileMode.OpenOrCreate);
            fileStream.Close();
            StreamWriter streamWriter = new StreamWriter(SAVEFILENAME);
            foreach (var item in tokens)
            {
                streamWriter.WriteLine(item);
            }
            streamWriter.Close();


        }

        private void Deserialize()
        {
            if (!File.Exists(SAVEFILENAME)) return;
            var result = File.ReadAllLines(SAVEFILENAME);
            lastPath = result[0];
            targetLocation = result[1];
            className = result[2];
        }


    }
}

#endif //UNITY_EDITOR
