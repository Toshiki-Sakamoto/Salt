﻿// 
// DebugButtonItem.cs  
// ProductName Ling
//  
// Created by toshiki sakamoto on 2020.09.05
// 

using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Utility.DebugConfig
{
#if DEBUG
	/// <summary>
	/// DebugItemボタン
	/// </summary>
	public class DebugButtonItem : MonoBehaviour
	{
		public class Data : DebugItemDataBase<DebugButtonItem>
		{
			public System.Action OnClick { get; }

			public Data(string title, System.Action onClick)
				: base(title)
			{
				OnClick = onClick;
			}

			protected override void DataUpdateInternal(DebugButtonItem obj)
			{
				obj.SetData(this);
			}

			public override Const.MenuType GetMenuType() =>
				Const.MenuType.Button;
		}


		[SerializeField] private Button _button = default;
		[SerializeField] private Text _buttonText = default;

		private Data _data;

		public void SetData(Data data)
		{
			_buttonText.text = data.Title;

			_data = data;
		}

		public void Awake()
		{
			_button
				.OnClickAsObservable()
				.Subscribe(_ =>
					{
						_data.OnClick?.Invoke();
					});
		}
	}
#endif
}