using System;
using System.Linq;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 補助クラスサンプルのボタン
/// </summary>
public class AssistantSampleButton : MonoBehaviour {

  /// <summary>
  /// シーン移動
  /// </summary>
  public void LoadScene() {
    SceneManager.LoadScene(SceneManager.GetSceneAt(0).name == "AssistantSample1" ? "AssistantSample2" : "AssistantSample1");
  }
  
}