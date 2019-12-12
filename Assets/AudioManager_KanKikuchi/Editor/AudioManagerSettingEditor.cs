namespace KanKikuchi.AudioManager {

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// PuzzleAdjustValueの表示を変えるクラス
/// </summary>
[CustomEditor(typeof(AudioManagerSetting))]
public class AudioManagerSettingEditor : Editor {

  //=================================================================================
  //更新
  //=================================================================================

  //Inspectorを更新する
  public override void OnInspectorGUI() {
    serializedObject.Update();
    ShowGUIAtSkin(ShowInspectorGUI);
    serializedObject.ApplyModifiedProperties();
  }

  //Inspector上のGUIを表示
  private void ShowInspectorGUI() {
    ShowGUIAtSkin(() => {
      ShowPropertyField("_isAutoUpdateAudioPath", "Is Auto Update Audio Path", "BGMPathとSEPathを自動更新する");
    });
    
    ShowGUIAtSkin(() => {
      ShowPropertyField("_bgmAudioPlayerNum", "BGM Audio Player Num", "BGMの同時再生可能数");
      ShowPropertyField("_seAudioPlayerNum",  "SE Audio Player Num",  "SEの同時再生可能数");
    });
    
    ShowGUIAtSkin(() => {
      ShowPropertyField("_bgmBaseVolume", "BGM Base Volume", "BGMの基準ボリューム");
      ShowPropertyField("_seBaseVolume",  "SE Base Volume",  "SEの基準ボリューム");
      
      EditorGUILayout.Space();
      
      ShowPropertyField("_shouldAdjustSEVolumeRate", "Should Adjust SE Volume Rate", "SEのボリューム倍率調整をする");
    });
    
    ShowGUIAtSkin(() => {
      ShowPropertyField("_isAutoGenerateBGMManager", "Is Auto Generate BGM Manager", "BGMManagerを自動生成する");
      ShowPropertyField("_isAutoGenerateSEManager", "Is Auto Generate SE Manager", "SEManagerを自動生成する");
      
      EditorGUILayout.Space();
      
      ShowPropertyField("_isDestroyBGMManager", "Is Destroy BGM Manager", "BGMManagerをシーン遷移時に破棄する");
      ShowPropertyField("_isDestroySEManager",  "Is Destroy SE Manager",  "SEManagerをシーン遷移時に破棄する");
    });
    
    ShowGUIAtSkin(() => {
      EditorGUILayout.LabelField("キャッシュの種類");
      EditorGUI.indentLevel++;
      EditorGUILayout.LabelField("None : 一切キャッシュしない");
      EditorGUILayout.LabelField("All : 起動時に全てキャッシュ");
      EditorGUILayout.LabelField("Used : ゲーム中に使ったものをキャッシュ");
      EditorGUI.indentLevel--;
      EditorGUILayout.Space();

      ShowGUIAtSkin(ShowCacheGUI, "BGM");
      ShowGUIAtSkin(ShowCacheGUI, "SE");
    });
    
    ShowGUIAtSkin(() => {
      ShowGUIAtSkin(ShowAutoSettingGUI, "BGM");
      ShowGUIAtSkin(ShowAutoSettingGUI, "SE");
    });
  }

  //オーディオの自動設定周りの設定をするGUIを表示
  private void ShowAutoSettingGUI(string targetTypeName) {
    var _isAutoUpdateProperty = ShowPropertyField("_isAutoUpdate" + targetTypeName + "Setting", "Is Auto Update" + targetTypeName + " Setting", targetTypeName + "ファイルの設定を自動でする");
    if (!_isAutoUpdateProperty.boolValue) {
      return;
    }
    
    if (GUILayout.Button("全" + targetTypeName + "ファイル更新")) {
      if (targetTypeName == "BGM") {
        AudioPostProcessor.UpdateBGMSetting();
      }
      else {
        AudioPostProcessor.UpdateSESetting();
      }
    }
    
    ShowPropertyField("_forceToMonoFor" + targetTypeName, "Force To Mono For" + targetTypeName, targetTypeName + "ファイルをステレオから強制的にモノラルにする");
    ShowPropertyField("_normalizeFor" + targetTypeName, "Normalize For " + targetTypeName, targetTypeName + "ファイルの音量を平均化する");
    ShowPropertyField("_ambisonicFor" + targetTypeName, "Ambisonic For " + targetTypeName, targetTypeName + "ファイルをアンビソニック(?)にする");
    ShowPropertyField("_loadInBackgroundFor" + targetTypeName, "Load In Background For " + targetTypeName, targetTypeName + "ファイルをバックグラウンドでロードする");
    ShowPropertyField("_loadTypeFor" + targetTypeName, "Load Type For " + targetTypeName, targetTypeName + "ファイルのロードの種類");
    ShowPropertyField("_qualityFor" + targetTypeName, "Quality For " + targetTypeName, targetTypeName + "ファイルの品質");
    ShowPropertyField("_compressionFormatFor" + targetTypeName, "Compression Format For " + targetTypeName, targetTypeName + "ファイルの圧縮フォーマット");
    ShowPropertyField("_sampleRateSettingFor" + targetTypeName, "Sample Rate Setting For " + targetTypeName, targetTypeName + "ファイルのサンプリングレート");
  }

  //キャッシュの設定をするGUIを表示
  private void ShowCacheGUI(string targetTypeName) {
    var typeProperty = ShowPropertyField("_" + targetTypeName.ToLower() + "CacheType", targetTypeName + " Cache Type", targetTypeName + "のキャッシュの種類");
    var cacheType = (AudioCacheType)Enum.ToObject (typeof(AudioCacheType), typeProperty.enumValueIndex);

    if (cacheType == AudioCacheType.Used) {
      ShowPropertyField("_isRelease" + targetTypeName + "Cache", "Is Release " + targetTypeName + " Cache", "シーン遷移時に" + targetTypeName + "のキャッシュを破棄する");
    }
  }
  
  //プロパティを変更するGUIを表示
  private SerializedProperty ShowPropertyField(string propertyName, string propertyDisplayName, string summary) {
    var property = serializedObject.FindProperty(propertyName);
    EditorGUILayout.PropertyField(property , new GUIContent(propertyDisplayName));
    EditorGUI.indentLevel++;
    EditorGUILayout.LabelField(summary);
    EditorGUI.indentLevel--;
    return property;
  }
  
  //GUIをスキンで挟んで表示
  private static void ShowGUIAtSkin(Action showGUIAction){
    EditorGUILayout.BeginVertical(GUI.skin.box);
    showGUIAction();
    EditorGUILayout.EndVertical();
    EditorGUILayout.Space();
  }
  
  /// <summary>
  /// GUIをスキンで挟んで表示(引数1つ)
  /// </summary>
  public static void ShowGUIAtSkin<T1>(Action<T1> showGUIAction, T1 t1) {
    ShowGUIAtSkin(() => { showGUIAction(t1);});
  }
  
}

}