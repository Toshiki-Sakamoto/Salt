//
// DotTexturePacker.cs
// ProductName Ling
//
// Created by Toshiki Sakamoto on 2021.11.21
//

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utility.CustomVisualElement;
using Utility.Extensions;
using System.Linq;

namespace Utility.Editor.DotTexturePacker
{
	/// <summary>
	/// 入力フォルダ内にテクスチャがある場合組み合わせる
	/// </summary>
	public class DotTexturePacker : EditorWindow
	{
		#region 定数, class, enum

		private const string UXMLPath = "Assets/App/Scripts/Utility/Editor/DotTexturePacker/DotTexturePacker.uxml";

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		[SerializeField] private string _sourceFullPath;
		[SerializeField] private List<string> _logListItems = new List<string>();

		private ListView _logListView;

		#endregion


		#region プロパティ

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		[MenuItem("Utility/UI/DotTexturePacker")]
		public static void ShowExample()
		{
			var wnd = GetWindow<DotTexturePacker>();
			wnd.titleContent = new GUIContent("DotTexturePacker");
		}

		public void OnEnable()
		{
			// EditorWindowはScriptableObject継承しているのでそのままシリアライズできる
			var so = new SerializedObject(this);
			so.Update();

			var root = rootVisualElement;

			var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXMLPath);
			var uxml = visualTree.CloneTree();

			root.Add(uxml);
			root.Bind(so);

			// 入力パス
			root.Q<TextField>(PathFieldElement.SourceFullPathName);
			// 実行ボタン
			var enterButton = root.Q<Button>("EnterButton");
			enterButton.clickable.clicked += () =>
			{
				Execute();
			};

			// ログを表示するためのリスト
			_logListView = root.Q<ListView>("LogListView");

			// アイテム作成時どのVisualElementを複製するか
			_logListView.makeItem = () =>
			{
				var label = new Label();
				label.style.unityTextAlign = TextAnchor.MiddleLeft;

				return label;
			};

			// アイテムが表示される時に表示される内容を設定
			_logListView.bindItem = (e, i) =>
			{
				(e as Label).text = _logListItems[i];
			};

			// 紐付けられるデータ
			_logListView.itemsSource = _logListItems;

			// 選択可能 (Single:一つのアイテムのみ選択できる。Multiple:複数選択できる。None:選択不可)
			_logListView.selectionType = SelectionType.Single;
		}

		#endregion


		#region private 関数

		private void AddLog(string log)
		{
			_logListItems.Add(log);


			// itemsSourceに紐付けられたアイテムが更新されたらリビルドする必要がある
			_logListView.Rebuild();
		}

		private void Execute()
		{
			if (string.IsNullOrEmpty(_sourceFullPath))
			{
				AddLog("パスを入力してください");
				return;
			}

			AddLog($"入力パス{_sourceFullPath}");

			var textures = Resources.LoadAll<Texture2D>(_sourceFullPath);
			if (textures.IsNullOrEmpty())
			{
				AddLog("テクスチャが存在しません");
				return;
			}

			var filenameRegex = new Regex(@"(\d+)_(\d+)");
			Debug.Log(filenameRegex);

			var groups = textures
				.GroupBy(tex =>
				{
					var match = filenameRegex.Match(tex.name);
					if (!match.Success) return null;

					return match.Groups[0];
				})
				.Where(group => group != null)
				.OrderBy(group => group.Key.Name);

			var dstTexture = new Texture2D(64 * 8, 64 * 8);

			int yIndex = 0;

			foreach (var group in groups)
			{
				int xIndex = 0;

				foreach (var tex in group)
				{
					dstTexture.SetPixels(64 * xIndex++, 64 * yIndex++, 64, 64, tex.GetPixels());
				}

				++yIndex;
			}
		}

		#endregion
	}
}
