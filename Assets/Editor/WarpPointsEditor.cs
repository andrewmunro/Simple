#if UNITY_EDITOR
using Assets.Scripts.Simple.Vendor.Locations;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(WorldLocations))]
    public class WarpPointsEditor : UnityEditor.Editor
    {

        private bool initDone = false;
        private GUIStyle MajorButton;
        private GUIStyle TinyButton;
        private GUIStyle TinyButtonWide;
        private GUIStyle TinyButtonRed;

        private void DrawWarpPointEntry(WorldLocation loc)
        {

            var targ = target as WorldLocations;
            GUILayout.Space(6);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("\u25B2", TinyButton))
            { //move up
                Undo.RecordObject(targ, "Move Warp Up");
                targ.MoveUp(loc);
                EditorGUI.FocusTextInControl(null); //in case the user has a focused text field
                EditorUtility.SetDirty(targ);
            }
            if (GUILayout.Button("\u25BC", TinyButton))
            { //move down
                Undo.RecordObject(targ, "Move Warp Down");
                targ.MoveDown(loc);
                EditorGUI.FocusTextInControl(null);
                EditorUtility.SetDirty(targ);
            }

            if (targ.WarpMatch(loc))
            { //we'll do a different button color if we have a match between current transforms and this warp
                if (GUILayout.Button("\u2248", TinyButtonWide))
                {
                    Undo.RecordObject(targ.transform, "Warp GameObject");
                    targ.WarpTo(loc);
                    EditorUtility.SetDirty(targ.transform);
                }
            }
            else {
                if (GUILayout.Button("warp", TinyButtonWide))
                {
                    Undo.RecordObject(targ.transform, "Warp GameObject");
                    targ.WarpTo(loc);
                    EditorUtility.SetDirty(targ.transform);
                }
            }

            string _name = EditorGUILayout.TextField(loc.Name);
            if (_name != loc.Name)
            {
                Undo.RecordObject(targ, "Change Warp Name");
                loc.Name = _name;
                EditorUtility.SetDirty(targ);
            }


            if (GUILayout.Button("save", TinyButtonWide))
            { //overwrite
                if (EditorUtility.DisplayDialog("Overwrite Warp Point?", "Do you want to overwrite this warp point with the current transforms?", "Yes", "Cancel"))
                {
                    Undo.RecordObject(targ, "Overwrite Warp");
                    targ.Overwrite(loc);
                    EditorUtility.SetDirty(targ);
                }
            }

            if (GUILayout.Button("X", TinyButtonRed))
            {
                if (EditorUtility.DisplayDialog("Remove Warp Point?", "Do you want to remove this warp point?", "Yes", "Cancel"))
                {
                    Undo.RecordObject(targ, "Remove Warp");
                    targ.RemoveWarpPoint(loc);
                    EditorUtility.SetDirty(targ);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        public override void OnInspectorGUI()
        {
            var targ = target as WorldLocations;

            var name = EditorGUILayout.TextField(targ.LocationGroupName);
            if (name != targ.LocationGroupName)
            {
                targ.LocationGroupName = name;
                EditorUtility.SetDirty(targ);
            }

            if ((!EditorApplication.isCompiling) && (!initDone))
            { //if its not compiling and this is the first OnInspectorGUI update
                InitStyles();
                initDone = true;
            }

            if (EditorApplication.isCompiling)
            { //compiling will make the init stuff happen again
                initDone = false;
            }

            for (int i = 0; i < targ.Locations.Count; i++)
            {
                DrawWarpPointEntry(targ.Locations[i]);
            }

            GUILayout.Space(6);

            if (GUILayout.Button("Create New Warp", MajorButton))
            {
                Undo.RecordObject(targ, "Create New Warp");
                targ.CreateWarpPoint();
                EditorUtility.SetDirty(targ);
            }

        }

        void InitStyles()
        {

            MajorButton = new GUIStyle(GUI.skin.button);
            MajorButton.stretchWidth = false;
            MajorButton.wordWrap = true;
            MajorButton.alignment = TextAnchor.MiddleCenter;
            MajorButton.fixedHeight = 20;
            MajorButton.fontSize = 12;
            MajorButton.fontStyle = FontStyle.Bold;

            TinyButton = new GUIStyle(GUI.skin.button);
            TinyButton.stretchWidth = false;
            TinyButton.wordWrap = false;
            TinyButton.alignment = TextAnchor.MiddleCenter;
            TinyButton.fixedHeight = 15;
            TinyButton.fontSize = 10;
            TinyButton.fontStyle = FontStyle.Bold;
            TinyButton.fixedWidth = 20;

            TinyButtonWide = new GUIStyle(TinyButton);
            TinyButtonWide.fixedWidth = 40;

            TinyButtonRed = new GUIStyle(TinyButton);
            TinyButtonRed.normal.textColor = Color.red;
        }


    }
}
#endif
