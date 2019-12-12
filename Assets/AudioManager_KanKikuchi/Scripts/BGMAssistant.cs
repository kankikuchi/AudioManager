namespace KanKikuchi.AudioManager {

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BGMを再生するための補助クラス
/// </summary>
public class BGMAssistant : AudioAssistant {

  //フェードアウトの長さ(0ならフェードアウトしない)
  [SerializeField]
  private float _fadeOutDuration = 0;

  public float FadeOutDuration {
    get { return _fadeOutDuration; }
    set { _fadeOutDuration = value; }
  }

  //ループ再生するか
  [SerializeField]
  private bool _isLoop = true;
  public bool IsLoop {
    get { return _isLoop; }
    set { _isLoop = value; }
  }

  //再生を自動で停止するか
  [SerializeField]
  private bool _isAutoStop = true;

  public bool IsAutoStop {
    get { return _isAutoStop; }
    set { _isAutoStop = value; }
  }

  //=================================================================================
  //初期化、破棄
  //=================================================================================

  protected override void Start() {
    base.Start();

    //自動で停止する時はシーンが破棄されるタイミングで
    if (_isAutoStop) {
      SceneManager.sceneUnloaded += OnUnloadedScene;
    }
  }

  //シーンが破棄された
  private void OnUnloadedScene(Scene scene) {
    SceneManager.sceneUnloaded -= OnUnloadedScene;
    if (_fadeOutDuration > 0) {
      BGMManager.Instance.FadeOut(_fadeOutDuration);
    }
    else {
      BGMManager.Instance.Stop();
    }
  }

  //=================================================================================
  //再生
  //=================================================================================

  /// <summary>
  /// BGM再生
  /// </summary>
  public override void Play() {
    if (_audioClip == null) {
      Debug.LogWarning(gameObject.name + "のBGMAssistantにAudioClipが設定されていません");
      return;
    }

    BGMManager.Instance.Play(_audioClip, _volumeRate, _delay, _pitch, _isLoop, allowsDuplicate: _fadeInDuration > 0);
    if (_fadeInDuration > 0) {
      BGMManager.Instance.FadeIn(_audioClip.name, _fadeInDuration);
    }
  }

}
}