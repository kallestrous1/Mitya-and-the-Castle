// Place this script inside an `Editor/` folder
// This window now supports BOTH entity persistence (with IDs)
// and global/singleton persistence (no IDs required)

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PersistenceDebugWindow : EditorWindow
{
    private Vector2 scrollPos;
    private DataPersistenceManager manager;

    [MenuItem("Window/Persistence Debugger")]
    public static void ShowWindow()
    {
        GetWindow<PersistenceDebugWindow>("Persistence Debugger");
    }

    private void OnFocus()
    {
        FindManager();
    }

    private void FindManager()
    {
        // This finds runtime objects even during Play Mode
        manager = Resources
            .FindObjectsOfTypeAll<DataPersistenceManager>()
            .FirstOrDefault(m => m.gameObject.scene.isLoaded);
    }

    private void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Data Persistence Debugger", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (manager == null)
        {
            EditorGUILayout.HelpBox("No DataPersistenceManager found in the scene.", MessageType.Warning);
            if (GUILayout.Button("Retry"))
               FindManager();
            return;
        }

        EditorGUILayout.LabelField("Loading State", manager.IsLoading ? "LOADING" : "IDLE");
        EditorGUILayout.LabelField("Registered Objects", manager.RegisteredCount.ToString());
        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        DrawEntityPersistenceObjects();
        EditorGUILayout.Space();
        DrawGlobalPersistenceObjects();
        EditorGUILayout.Space();
        DrawSavedItemStates();

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Refresh"))
            Repaint();
    }

    // ---------------- ENTITY (ID-BASED) OBJECTS ----------------
    private void DrawEntityPersistenceObjects()
    {
        EditorGUILayout.LabelField("Entity Persistence Objects (Require Unique IDs)", EditorStyles.boldLabel);

        var entities = manager.GetEntityPersistenceObjects();
        var groupedByID = entities.GroupBy(e => e.DebugID);

        foreach (var group in groupedByID)
        {
            bool duplicate = group.Count() > 1;
            GUI.color = duplicate ? Color.red : Color.white;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"ID: {group.Key}");
            EditorGUILayout.LabelField($"Instances: {group.Count()}");

            foreach (var entity in group)
            {
                EditorGUILayout.ObjectField("Object", entity as Object, typeof(Object), true);
            }

            if (duplicate)
                EditorGUILayout.HelpBox("Duplicate ENTITY ID detected!", MessageType.Error);

            EditorGUILayout.EndVertical();
            GUI.color = Color.white;
        }

        if (!entities.Any())
        {
            EditorGUILayout.HelpBox("No entity persistence objects registered.", MessageType.Info);
        }
    }

    // ---------------- GLOBAL (NO-ID) OBJECTS ----------------
    private void DrawGlobalPersistenceObjects()
    {
        EditorGUILayout.LabelField("Global / Singleton Persistence Objects", EditorStyles.boldLabel);

        var globals = manager.GetGlobalPersistenceObjects();

        foreach (var global in globals)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.ObjectField(global as Object, typeof(Object), true);
            EditorGUILayout.LabelField("[GLOBAL]", GUILayout.Width(70));
            EditorGUILayout.EndHorizontal();
        }

        if (!globals.Any())
        {
            EditorGUILayout.HelpBox("No global persistence objects registered.", MessageType.Info);
        }
    }

    // ---------------- SAVED DATA ----------------
    private void DrawSavedItemStates()
    {
        EditorGUILayout.LabelField("Saved Item States (GameData)", EditorStyles.boldLabel);

        if (manager.gameData == null)
        {
            EditorGUILayout.LabelField("No GameData loaded");
            return;
        }

        foreach (var kvp in manager.gameData.itemStates)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.LabelField(kvp.Key, GUILayout.Width(260));
            EditorGUILayout.LabelField(kvp.Value ? "ACTIVE" : "INACTIVE");
            EditorGUILayout.EndHorizontal();
        }
    }
}
