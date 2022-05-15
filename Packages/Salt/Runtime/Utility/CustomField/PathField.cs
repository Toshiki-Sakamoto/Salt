//
// PathField.cs
// ProductName Ling
//
// Created by Toshiki Sakamoto on 2021.11.21
//

using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utility.CustomField
{
	/// <summary>
	/// フォルダ&ファイルのパス管理
	/// </summary>
	public class PathField : MonoBehaviour
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		[SerializeField] private string _fullPath;

		#endregion


		#region プロパティ

		public string FullPath { get; private set; }

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		#endregion


		#region private 関数

		#endregion

#if UNITY_EDITOR

		/// <summary>
		/// <see cref="PathField"/>のカスタムエディタ
		/// </summary>
		[CustomEditor(typeof(PathField))]
		public class PathFieldCustomEditor : UnityEditor.Editor
		{
			#region 定数, class, enum

			#endregion


			#region public, protected 変数

			#endregion


			#region private 変数

			private PathField _target;
			private SerializedObject _object;
			private SerializedProperty _fullPath;

			#endregion


			#region プロパティ

			#endregion


			#region コンストラクタ, デストラクタ

			#endregion


			#region public, protected 関数

			private void OnEnable()
            {
				_target = target as PathField;

				_object = new SerializedObject(target);
				_fullPath = _object.FindProperty("_fullPath");
			}

            public override void OnInspectorGUI()
			{
				serializedObject.Update();

				EditorGUILayout.PropertyField(_fullPath);

				serializedObject.ApplyModifiedProperties();
			}

            public override VisualElement CreateInspectorGUI()
			{
				var root = new VisualElement();
				var container = new IMGUIContainer(OnInspectorGUI);
				root.Add(container);

				return root;
			}

            #endregion


            #region private 関数

            #endregion
        }
#endif
	}
}
