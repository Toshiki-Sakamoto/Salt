//
// PathFieldElement.cs
// ProductName Ling
//
// Created by Toshiki Sakamoto on 2021.11.21
//

using UnityEngine.UIElements;
using Utility.CustomField;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Utility.CustomVisualElement
{
	/// <summary>
	/// ドラッグアンドドロップを受け付けそのパスを表示、格納する
	/// </summary>
	public class PathFieldElement : VisualElement
	{
		#region 定数, class, enum

		public static readonly string SourceFullPathName = "SourceFullPathInput";

		public new class UxmlFactory : UxmlFactory<PathFieldElement, UxmlTraits> { }

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			private UxmlBoolAttributeDescription _autoConvertFolderPath = new UxmlBoolAttributeDescription { name = "AutoConvertFolderPath" };
			private UxmlBoolAttributeDescription _autoConvertResourcesPath = new UxmlBoolAttributeDescription { name = "AutoConvertResourcesPath" };

			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);

				var element = ((PathFieldElement)ve);
				element._autoConvertFolderPath = _autoConvertFolderPath.GetValueFromBag(bag, cc);
				element._autoConvertResourcesPath = _autoConvertResourcesPath.GetValueFromBag(bag, cc);
			}
		}

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		private TextField _sourceFullPath;
		private bool _autoConvertFolderPath = true; // 自動でフォルダのパスに変換する場合true
		private bool _autoConvertResourcesPath = false; // 自動でResourcesフォルダ配下のパスにする場合true

		#endregion


		#region プロパティ

		/// <summary>
		/// UIBuilderに表示するために同盟のプロパティ宣言が必要
		/// </summary>
		public bool AutoConvertFolderPath => _autoConvertFolderPath;
		public bool AutoConvertResourcesPath => _autoConvertResourcesPath;

		#endregion


		#region コンストラクタ, デストラクタ

		public PathFieldElement()
		{
			_sourceFullPath = new TextField("パス");
			_sourceFullPath.name = SourceFullPathName;
			_sourceFullPath.bindingPath = "_sourceFullPath";
			_sourceFullPath.style.marginTop = 10;
			_sourceFullPath.style.marginBottom = 10;

			Add(_sourceFullPath);

			Add(CreateDragAndDropArea());
		}

		#endregion


		#region public, protected 関数

		#endregion


		#region private 関数

		private VisualElement CreateDragAndDropArea()
		{
			return new IMGUIContainer(() =>
			{
				var evt = Event.current;

				var dragAndDropArea = GUILayoutUtility.GetRect(0.0f, 40.0f, GUILayout.ExpandWidth(true));

				var boxStyle = new GUIStyle(EditorStyles.textField)
				{
					alignment = TextAnchor.MiddleCenter
				};

				GUI.Box(dragAndDropArea, "ドラッグ・アンド・ドロップ", boxStyle);

				int id = GUIUtility.GetControlID(FocusType.Passive);

				if (!dragAndDropArea.Contains(evt.mousePosition)) return;

				switch (evt.type)
				{
					// 更新または実行
					case EventType.DragUpdated:
						{
							DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
							DragAndDrop.activeControlID = id;
							break;
						}

					case EventType.DragPerform:

						DragAndDrop.AcceptDrag();

						// 最初のオブジェクトのみ判定する
						var obj = DragAndDrop.objectReferences.FirstOrDefault();
						if (obj != null)
						{
							var assetPath = AssetDatabase.GetAssetPath(obj);

							// フォルダ名に変換する場合
							if (AutoConvertFolderPath)
							{
								if (!AssetDatabase.IsValidFolder(assetPath))
								{
									assetPath = System.IO.Path.GetDirectoryName(assetPath);
								}
							}

							// Resourcesフォルダ配下のパスにする
							if (AutoConvertResourcesPath)
							{
								var resourcesStr = "Resources/";
								var index = assetPath.LastIndexOf(resourcesStr);
								if (index >= 0)
								{
									assetPath = assetPath.Substring(index + resourcesStr.Length);
								}
							}

							_sourceFullPath.value = assetPath;
						}

						DragAndDrop.activeControlID = 0;

						Event.current.Use();
						break;
				}
			});
		}

		#endregion
	}
}
