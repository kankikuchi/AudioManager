using System;
using System.IO;
using System.Linq;
using UnityEngine;
using KanKikuchi.AudioManager;
using UnityEngine.UI;

/// <summary>
/// BGM切り替えサンプルのボタン
/// </summary>
public class BGMSwitchSampleButton : MonoBehaviour {

  //=================================================================================
  //再生、停止
  //=================================================================================
  
  /// <summary>
  /// BGM1を再生
  /// </summary>
  public void PlayBGM1() {
    PlayBGM(BGMPath.FANTASY14);
  }
  
  /// <summary>
  /// BGM1を再生
  /// </summary>
  public void PlayBGM2() {
    PlayBGM(BGMPath.BATTLE27);
  }
  
  /// <summary>
  /// 重複するBGMを再生
  /// </summary>
  public void PlayDuplicateBGM() {
    PlayBGM(BGMPath.HEARTBEAT01, 0.5f, true);
  }

  //BGMを再生
  private void PlayBGM(string bgmPath, float volumeRate = 1, bool allowsDuplicate = false) {
    Debug.Log(bgmPath + "再生開始");
    
    BGMManager.Instance.Play(bgmPath, volumeRate:volumeRate, allowsDuplicate:allowsDuplicate);
  }
  
  /// <summary>
  /// BGMを再生していたら停止する
  /// </summary>
  public void StopBGM() {
    Debug.Log("BGM停止");
    BGMManager.Instance.Stop();
  }
  
  //=================================================================================
  //切り替え
  //=================================================================================

  //次に再生するBGMのパスを取得
  private string GetNextBGMPath() {
    return BGMManager.Instance.GetCurrentAudioNames().Any(audioName => audioName == Path.GetFileNameWithoutExtension(BGMPath.FANTASY14)) ? BGMPath.BATTLE27 : BGMPath.FANTASY14;
  }
  
  /// <summary>
  /// 再生中のものをフェードアウトさせて、次のを再生開始する
  /// </summary>
  public void SwitchByFadeOut() {
    BGMSwitcher.FadeOut(GetNextBGMPath());
  }
  
  /// <summary>
  /// 再生中のものを即停止させて、次のをフェードインで開始する
  /// </summary>
  public void SwitchByFadeIn() {
    BGMSwitcher.FadeIn(GetNextBGMPath());
  }
  
  /// <summary>
  /// 再生中のものをフェードアウトさせて、次のをフェードインで開始する
  /// </summary>
  public void SwitchByFadeOutAndFadeIn() {
    BGMSwitcher.FadeOutAndFadeIn(GetNextBGMPath());
  }
  
  /// <summary>
  /// 再生中のものをフェードアウトさせて、同時に次のをフェードインで開始する
  /// </summary>
  public void SwitchByCrossFade() {
    BGMSwitcher.CrossFade(GetNextBGMPath());
  }
  
}