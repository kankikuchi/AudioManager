namespace KanKikuchi.AudioManager {

using System;
using UnityEngine;

/// <summary>
/// SEを再生するための補助クラス
/// </summary>
public class SEAssistant : AudioAssistant {

  //ループ再生するか
  [SerializeField]
  private bool _isLoop = false;
  public bool IsLoop {
    get { return _isLoop; }
    set { _isLoop = value; }
  }

  //=================================================================================
  //再生
  //=================================================================================

  /// <summary>
  /// SE再生
  /// </summary>
  public override void Play() {
    Play(null);
  }

  /// <summary>
  /// コールバックを指定してSE再生
  /// </summary>
  public void Play(Action callback) {
    if (_audioClip == null) {
      Debug.LogWarning(gameObject.name + "のSEAssistantにAudioClipが設定されていません");
      callback?.Invoke();
      return;
    }

    SEManager.Instance.Play(_audioClip, _volumeRate, _delay, _pitch, _isLoop, callback);
    if (_fadeInDuration > 0) {
      SEManager.Instance.FadeIn(_audioClip.name, _fadeInDuration);
    }

  }

}
}