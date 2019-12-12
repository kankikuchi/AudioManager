namespace KanKikuchi.AudioManager {

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BGMを切り替えるためのクラス
/// </summary>
public static class BGMSwitcher {

  /// <summary>
  /// 再生中のものをフェードアウトさせて、次のを再生開始する
  /// </summary>
  public static void FadeOut(string audioPath, float fadeOutDuration = 1f, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = true, Action callback = null) {
    if (!BGMManager.Instance.IsPlaying()) {
      BGMManager.Instance.Play(audioPath, volumeRate, delay, pitch, isLoop);
      return;
    }
    
    BGMManager.Instance.FadeOut(fadeOutDuration, () => {
      BGMManager.Instance.Play(audioPath, volumeRate, delay, pitch, isLoop);
      callback?.Invoke();
    });
  }
  
  /// <summary>
  /// 再生中のものを即停止させて、次のをフェードインで開始する
  /// </summary>
  public static void FadeIn(string audioPath, float fadeInDuration = 1f, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = true, Action callback = null) {
    BGMManager.Instance.Stop();
    BGMManager.Instance.Play(audioPath, volumeRate, delay, pitch, isLoop);
    BGMManager.Instance.FadeIn(audioPath, fadeInDuration, callback);
  }
  
  /// <summary>
  /// 再生中のものをフェードアウトさせて、次のをフェードインで開始する
  /// </summary>
  public static void FadeOutAndFadeIn(string audioPath, float fadeOutDuration = 1f, float fadeInDuration = 1f, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = true, Action callback = null) {
    if (!BGMManager.Instance.IsPlaying()) {
      FadeIn(audioPath, fadeInDuration, volumeRate, delay, pitch, isLoop, callback);
      return;
    }
    
    BGMManager.Instance.FadeOut(fadeOutDuration, () => {
      FadeIn(audioPath, fadeInDuration, volumeRate, delay, pitch, isLoop, callback);
    });
  }
  
  /// <summary>
  /// 再生中のものをフェードアウトさせて、同時に次のをフェードインで開始する
  /// </summary>
  public static void CrossFade(string audioPath, float fadeDuration = 1f, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = true, Action callback = null) {
    if (BGMManager.Instance.GetCurrentAudioNames().Count >= BGMManager.Instance.AudioPlayerNum) {
      Debug.LogWarning("クロスフェードするにはAudio Player Numが足りません");
    }
    
    foreach (var currentAudioName in BGMManager.Instance.GetCurrentAudioNames()) {
      BGMManager.Instance.FadeOut(currentAudioName, fadeDuration);
    }
    
    BGMManager.Instance.Play(audioPath, volumeRate, delay, pitch, isLoop, allowsDuplicate:true);
    BGMManager.Instance.FadeIn(audioPath, fadeDuration, callback);
  }
  
}

}