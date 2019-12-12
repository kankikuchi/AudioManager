using UnityEngine;
using KanKikuchi.AudioManager;

/// <summary>
/// 基本サンプルのスライダー
/// </summary>
public class BasicSampleSlider : MonoBehaviour {

  /// <summary>
  /// スライダーに合わせてBGMのボリュームを変更する(再生中のものも)
  /// </summary>
  public void ChangeBGMVolume(float volume) {
    BGMManager.Instance.ChangeBaseVolume(volume);
    Debug.Log("BGMのボリューム変更 : " + volume);
  }
  
  /// <summary>
  /// スライダーに合わせてBGMのボリュームを変更する(再生中のものも)
  /// </summary>
  public void ChangeSEVolume(float volume) {
    SEManager.Instance.ChangeBaseVolume(volume);
    Debug.Log("SEのボリューム変更 : " + volume);
  }

}