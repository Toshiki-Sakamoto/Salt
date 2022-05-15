﻿//
// UtilityEditorSettings.cs
// ProductName Ling
//
// Created by toshiki sakamoto on 2021.04.16
//

using UnityEngine;
using UnityEditor;

namespace Utility.Editor
{
	/// <summary>
	/// UtilityのEditorに関する設定を管理する
	/// </summary>
	
	[CreateAssetMenu(menuName = "Ling/Utility/EditorSettings")]
	public class UtilityEditorSettings : ScriptableObject
	{
		#region 定数, class, enum

		#endregion


		#region public, protected 変数

		#endregion


		#region private 変数

		[Header("Canvas追加時にCanvasCategoryを追加する")]
		[SerializeField] private bool _enableCanvasGroupAttach = default;

		private static UtilityEditorSettings instance;

		#endregion


		#region プロパティ

		public static UtilityEditorSettings Instance
		{
			get
			{
				if (instance != null) return instance;

				instance = AssetHelper.LoadAsset<UtilityEditorSettings>();
				return instance;
			}
		}

		public static bool EnableCanvasGroupAttach => Instance._enableCanvasGroupAttach;

		#endregion


		#region コンストラクタ, デストラクタ

		#endregion


		#region public, protected 関数

		#endregion


		#region private 関数

		#endregion
	}
}
