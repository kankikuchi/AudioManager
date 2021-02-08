namespace KanKikuchi.AudioManager {

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// オーディオファイルへのパスを定数で管理するクラスを自動で作成するスクリプト
/// </summary>
public class AudioPathCreator : AssetPostprocessor {

	//オーディオファイルが入ってるディレクトリへのパス
	public static readonly string BGM_DIRECTORY_PATH = "Resources/" + BGMManager.AUDIO_DIRECTORY_PATH, SE_DIRECTORY_PATH = "Resources/" + SEManager.AUDIO_DIRECTORY_PATH;

	//=================================================================================
	//変更の監視
	//=================================================================================

	#if !UNITY_CLOUD_BUILD

	//オーディオファイルが入ってるディレクトリが変更されたら、自動で各スクリプトを作成
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
		//UnityPackageで最初にインポートした時はEntityがまだnull
		if (AudioManagerSetting.Entity == null || !AudioManagerSetting.Entity.IsAutoUpdateAudioPath) {
			return;
		}
		
		List<string[]> assetsList = new List<string[]>() {
			importedAssets, deletedAssets, movedAssets, movedFromAssetPaths
		};

		//すぐにResources.LoadAllで取得出来ない場合もあるので間を開けて実行
		EditorApplication.delayCall += () => {
			if (ExistsPathInAssets(assetsList, BGM_DIRECTORY_PATH)) {
				CreateBGMPath();
			}
			if (ExistsPathInAssets(assetsList, SE_DIRECTORY_PATH)) {
				CreateSEPath();
			}
		};
	}

	//入力されたassetsのパスの中に、指定したパスが含まれるものが一つでもあるか
	private static bool ExistsPathInAssets(List<string[]> assetPathsList, string targetPath) {
		return assetPathsList
			.Any(assetPaths => assetPaths
				.Any(assetPath => assetPath
					.Contains(targetPath)));
	}

	#endif

	//=================================================================================
	//スクリプト作成
	//=================================================================================

	//BGMとSEファイルへのパスを定数で管理するクラスを作成
	[MenuItem("Tools/KanKikuchi.AudioManager/Create BGM&SE Path")]
	private static void CreateAudionPath() {
		CreateBGMPath();
		CreateSEPath();
	}

	//BGMファイルへのパスを定数で管理するクラスを作成
	[MenuItem("Tools/KanKikuchi.AudioManager/Create BGM Path")]
	private static void CreateBGMPath() {
		Create(BGM_DIRECTORY_PATH);
	}

	//SEファイルへのパスを定数で管理するクラスを作成
	[MenuItem("Tools/KanKikuchi.AudioManager/Create SE Path")]
	private static void CreateSEPath() {
		Create(SE_DIRECTORY_PATH);
	}

	//オーディオファイルへのパスを定数で管理するクラスを作成
	private static void Create(string directoryPath) {
		//オーディオファイルへのパスを抽出
		string directoryName = Path.GetFileName(directoryPath);
		var audioPathDict = new Dictionary<string, string>();

		foreach (var audioClip in Resources.LoadAll<AudioClip>(directoryName)) {
			//アセットへのパスを取得
			var assetPath = AssetDatabase.GetAssetPath(audioClip);
			
			//Resources以下のパス(拡張子なし)に変換
			var targetIndex = assetPath.LastIndexOf("Resources", StringComparison.Ordinal) + "Resources".Length + 1;
			var resourcesPath = assetPath.Substring(targetIndex);
			resourcesPath = resourcesPath.Replace(Path.GetExtension(resourcesPath), "");

			//オーディオ名の重複チェック
			var audioName = audioClip.name;
			if (audioPathDict.ContainsKey(audioName)) {
				Debug.LogError(audioName + " is duplicate!\n1 : " + resourcesPath + "\n2 : " + audioPathDict[audioName]);
			}
			audioPathDict[audioName] = resourcesPath;
		}
		
		//このスクリプトがある所へのパス取得し、定数クラスを書き出す場所を決定
		string selfFileName = "AudioPathCreator.cs";
		string selfPath = Directory.GetFiles("Assets", "*", System.IO.SearchOption.AllDirectories)
			.FirstOrDefault(path => System.IO.Path.GetFileName(path) == selfFileName);

		string exportPath = selfPath.Replace(selfFileName, "").Replace("Editor","Scripts");
		
		//定数クラス作成
		ConstantsClassCreator.Create(directoryName + "Path", directoryName + "ファイルへのパスを定数で管理するクラス", audioPathDict, exportPath, "KanKikuchi.AudioManager");
	}

}
}