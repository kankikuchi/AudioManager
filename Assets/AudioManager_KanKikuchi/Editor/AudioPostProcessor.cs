namespace KanKikuchi.AudioManager {

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// オーディオファイルの設定を自動でするクラス
/// </summary>
public class AudioPostProcessor : AssetPostprocessor {
  
  //=================================================================================
  //変更の監視
  //=================================================================================
  
  #if !UNITY_CLOUD_BUILD

  //オーディオファイルが入ってるディレクトリが変更されたら、自動で各スクリプトを作成
  private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
    //UnityPackageで最初にインポートした時はまだnullなのでその時用
    if (AudioManagerSetting.Entity == null) {
      return;
    }
    
    string bgmDirectoryPath = GetBGMDirectoryPath(), seDirectoryPath = GetSEDirectoryPath();
    
    //対象のディレクトのファイルがあるかチェック
    var targetBGMPathList = new List<string>();
    var targetSEPathList  = new List<string>();

    foreach (var path in importedAssets) {
      if (path.Contains(bgmDirectoryPath)) {
        targetBGMPathList.Add(path);
      }
      else if (path.Contains(seDirectoryPath)) {
        targetSEPathList.Add(path);
      }
    }
    
    foreach (var path in movedAssets) {
      if (path.Contains(bgmDirectoryPath) && !targetBGMPathList.Contains(path)) {
        targetBGMPathList.Add(path);
      }
      else if (path.Contains(seDirectoryPath) && !targetSEPathList.Contains(path)) {
        targetSEPathList.Add(path);
      }
    }

    if (AudioManagerSetting.Entity.IsAutoUpdateBGMSetting) {
      targetBGMPathList.ForEach(ChangeBGMSetting);
    }
    if (AudioManagerSetting.Entity.IsAutoUpdateSESetting) {
      targetSEPathList.ForEach(ChangeSESetting);
    }
  }

  #endif
  
  //=================================================================================
  //パスの取得
  //=================================================================================
  
  //対象のBGMが入ってるディレクトリへのパスを取得
  private static string GetBGMDirectoryPath() {
    return GetTopDirectoryPath() + AudioPathCreator.BGM_DIRECTORY_PATH;
  }
  
  //対象のSEが入ってるディレクトリへのパスを取得
  private static string GetSEDirectoryPath() {
    return GetTopDirectoryPath() + AudioPathCreator.SE_DIRECTORY_PATH;
  }

  //対象のが入ってるディレクトリへのパスを取得
  private static string GetTopDirectoryPath() {
    //このスクリプトがある所へのパス取得し、そこからパスを計算
    string selfFileName = "AudioPostProcessor.cs";
    string selfPath = Directory.GetFiles("Assets", "*", System.IO.SearchOption.AllDirectories).FirstOrDefault(path => System.IO.Path.GetFileName(path) == selfFileName);
    
    var editorIndex = selfPath.LastIndexOf("Editor");
    return selfPath.Substring(0, editorIndex).Replace("\\", "/");
  }
  
  //=================================================================================
  //設定変更
  //=================================================================================

  //全オーディオファイルの設定を更新する
  [MenuItem("Tools/KanKikuchi.AudioManager/Update BGM&SE Setting")]
  private static void UpdateSetting() {
    UpdateBGMSetting();
    UpdateSESetting();
  }
  
  /// <summary>
  /// 全BGMファイルの設定を更新する
  /// </summary>
  [MenuItem("Tools/KanKikuchi.AudioManager/Update BGM Setting")]
  public static void UpdateBGMSetting() {
    UpdateSetting(GetBGMDirectoryPath(), ChangeBGMSetting);
  }
  
  /// <summary>
  /// 全SEファイルの設定を更新する
  /// </summary>
  [MenuItem("Tools/KanKikuchi.AudioManager/Update SE Setting")]
  public static void UpdateSESetting() {
    UpdateSetting(GetSEDirectoryPath(), ChangeSESetting);
  }
  
  //全オーディオファイルの設定を更新する
  private static void UpdateSetting(string directoryPath, Action<string> changeSettingAction) {
    foreach (var filePath in Directory.GetFiles (directoryPath, "*", SearchOption.AllDirectories)) {
      changeSettingAction(filePath);
    }
  }
  
  //BGMファイルの設定を変更する
  private static void ChangeBGMSetting(string audioPath) {
    var setting = AudioManagerSetting.Entity;
    ChangeSetting(audioPath, setting.ForceToMonoForBGM, setting.NormalizeForBGM, setting.AmbisonicForBGM, setting.LoadInBackgroundForBGM, 
      setting.LoadTypeForBGM, setting.QualityForBGM, setting.CompressionFormatForBGM, setting.SampleRateSettingForBGM);
  }
  
  //SEファイルの設定を変更する
  private static void ChangeSESetting(string audioPath) {
    var setting = AudioManagerSetting.Entity;
    ChangeSetting(audioPath, setting.ForceToMonoForSE, setting.NormalizeForSE, setting.AmbisonicForSE, setting.LoadInBackgroundForSE, 
      setting.LoadTypeForSE, setting.QualityForSE, setting.CompressionFormatForSE, setting.SampleRateSettingForSE);
  }

  //オーディオファイルの設定を変更する
  private static void ChangeSetting(string audioPath, bool forceToMono, bool normalize, bool ambisonic, bool loadInBackground, AudioClipLoadType loadType, float quality, AudioCompressionFormat compressionFormat, AudioSampleRateSetting sampleRateSetting) {
    if (AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath) == null) {
      return;
    }
    var importer = AssetImporter.GetAtPath(audioPath) as AudioImporter;
    
    importer.forceToMono = forceToMono;
    
    var serializedObject = new SerializedObject(importer);
    var normalizeProperty = serializedObject.FindProperty("m_Normalize");
    normalizeProperty.boolValue = normalize;
    serializedObject.ApplyModifiedProperties();
        
    importer.ambisonic        = ambisonic;
    importer.loadInBackground = loadInBackground;
    
    var settings = importer.defaultSampleSettings;
    settings.loadType = loadType;
    settings.quality  = quality;
    settings.compressionFormat = compressionFormat;
    settings.sampleRateSetting = sampleRateSetting;

    importer.defaultSampleSettings = settings;
    
    Debug.Log(audioPath + "の設定を変更しました");
  }
  
}

}
