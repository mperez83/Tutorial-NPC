using UnityEngine;

namespace UnityEditor.Tilemaps
{
    [CreateAssetMenu(fileName = "Prefab brush", menuName = "Brushes/Prefab brush")]
    [CustomGridBrush(false, false, false, "Prefab Brush")]
    public class PrefabBrush : GridBrush
    {
        /// <summary>
        /// The prefab to paint
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// Anchor Point of the Instantiated Prefab in the cell when painting
        /// </summary>
        public Vector3 m_Anchor = new Vector3(0.5f, 0.5f, 0.5f);

        GameObject prev_brushTarget;

        /// <summary>
        /// Paints a single prefab into a given position within the selected layers, if a gameObject isn't already there
        /// </summary>
        /// <param name="gridLayout">Grid used for layout</param>
        /// <param name="brushTarget">Target of the paint operation. By default the currently selected GameObject</param>
        /// <param name="position">The coordinates of the cell to paint data to</param>
        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            //Set the current GameObject we're painting to?
            if (brushTarget) prev_brushTarget = brushTarget;
            brushTarget = prev_brushTarget;

            //Do not allow editing palettes
            if (brushTarget.layer == 31) return;

            //Only draw if the current cell doesn't have an object in it
            Transform checkObject = GetObjectInCell(grid, brushTarget.transform, position);
            if (checkObject != null) return;

            //Actually draw the object
            GameObject objectToPaint = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (objectToPaint != null)
            {
                Undo.MoveGameObjectToScene(objectToPaint, brushTarget.scene, "Paint Prefabs");
                Undo.RegisterCreatedObjectUndo(objectToPaint, "Paint Prefabs");

                objectToPaint.transform.SetParent(brushTarget.transform);
                objectToPaint.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + m_Anchor));
            }
        }

        /// <summary>
        /// Erases Prefabs in a given position within the selected layers.
        /// The PrefabBrush overrides this to provide Prefab erasing functionality.
        /// </summary>
        /// <param name="gridLayout">Grid used for layout</param>
        /// <param name="brushTarget">Target of the erase operation. By default the currently selected GameObject</param>
        /// <param name="position">The coordinates of the cell to erase data from</param>
        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            //Set the current GameObject we're erasing from?
            if (brushTarget) prev_brushTarget = brushTarget;
            brushTarget = prev_brushTarget;

            //Do not allow editing palettes
            if (brushTarget.layer == 31) return;

            //Actually erase the object
            Transform objectToErase = GetObjectInCell(grid, brushTarget.transform, position);
            if (objectToErase != null)
                Undo.DestroyObjectImmediate(objectToErase.gameObject);
        }

        static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
        {
            int childCount = parent.childCount;
            Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
            Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
            Bounds bounds = new Bounds((max + min) * .5f, max - min);

            for (int i = 0; i < childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (bounds.Contains(child.position))
                    return child;
            }
            return null;
        }
    }



    //Actual stuff to display to the user within the editor
    [CustomEditor(typeof(PrefabBrush))]
    public class PrefabBrushEditor : GridBrushEditor
    {
        private PrefabBrush prefabBrush { get { return target as PrefabBrush; } }

        private SerializedProperty prefab;
        private SerializedProperty m_Anchor;
        private SerializedObject m_SerializedObject;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_SerializedObject = new SerializedObject(target);
            prefab = m_SerializedObject.FindProperty("prefab");
            m_Anchor = m_SerializedObject.FindProperty("m_Anchor");
        }

        public override void OnPaintInspectorGUI()
        {
            m_SerializedObject.UpdateIfRequiredOrScript();
            EditorGUILayout.PropertyField(prefab, true);
            EditorGUILayout.PropertyField(m_Anchor);
            m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}