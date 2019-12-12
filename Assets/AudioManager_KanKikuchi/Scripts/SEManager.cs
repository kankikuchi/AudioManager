namespace KanKikuchi.AudioManager {

using System;
using UnityEngine;

/// <summary>
/// SE関連の管理をするクラス
/// </summary>
public class SEManager : AudioManager<SEManager> {

  //AudioPlayerの数(同時再生可能数)
  protected override int _audioPlayerNum => AudioManagerSetting.Entity.SEAudioPlayerNum;

  //オーディオファイルが入ってるディレクトリへのパス
  public static readonly string AUDIO_DIRECTORY_PATH = "SE";
  
  //ボリューム倍率調整をするか
  [SerializeField]
  private bool _shouldAdjustVolumeRate = true;

  //=================================================================================
  //初期化
  //=================================================================================

  //起動時に実行される
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  static void Initialize(){
    if (AudioManagerSetting.Entity.IsAutoGenerateSEManager) {
      new GameObject("SEManager", typeof(SEManager));
    }
  }
  
  protected override void Init() {
    base.Init();
    var setting = AudioManagerSetting.Entity;
    
    LoadAudioClip(AUDIO_DIRECTORY_PATH, setting.SECacheType, setting.IsReleaseSECache);

    _shouldAdjustVolumeRate = setting.ShouldAdjustSeVolumeRate;
    ChangeBaseVolume(setting.SEBaseVolume);
    if (!setting.IsDestroySEManager) {
      DontDestroyOnLoad(gameObject);
    }
  }

  //=================================================================================
  //再生
  //=================================================================================

  /// <summary>
  /// 再生
  /// </summary>
  public void Play(AudioClip audioClip, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = false, Action callback = null) {
    volumeRate = AdjustVolumeRate(volumeRate, audioClip.name);
    if (volumeRate > 0) {
      RunPlayer(audioClip, volumeRate, delay, pitch, isLoop, callback);
    }
  }
  
  /// <summary>
  /// 再生
  /// </summary>
  public void Play(string audioPath, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = false, Action callback = null) {
    volumeRate = AdjustVolumeRate(volumeRate, audioPath);
    if (volumeRate > 0) {
      RunPlayer(audioPath, volumeRate, delay, pitch, isLoop, callback);
    }
  }

  //=================================================================================
  //取得
  //=================================================================================

  //ボリュームの倍率を調整(同じものが再生されていたらボリュームを下げて、音が爆発しないように)
  private float AdjustVolumeRate(float volumeRate, string audioPathOrName) {
    if (!_shouldAdjustVolumeRate) {
      return volumeRate;
    }
    
    var audioName = PathToName(audioPathOrName);
    
    //指定したものと同じものを再生しているプレイヤーを取得、なければそのままのボリュームを返す
    var targetAudioPlayers = _audioPlayerList.FindAll(player => player.CurrentAudioName == audioName);
    if (targetAudioPlayers.Count == 0) {
      return volumeRate;
    }

    //同じSEが鳴ってすぐならボリュームを下げる
    foreach (var targetAudioPlayer in _audioPlayerList.FindAll(player => player.CurrentAudioName == audioName)) {
      if (targetAudioPlayer.CurrentVolume <= 0) {
        continue;
      }

      float playedTime = targetAudioPlayer.PlayedTime;
      if (targetAudioPlayer.CurrentState == AudioPlayer.State.Delay) {
        playedTime += targetAudioPlayer.ElapsedDelay;
      }

      if (playedTime <= 0.025f) {
        return 0;
      }
      else if (playedTime <= 0.05f) {
        volumeRate *= 0.8f;
      }
      else if (playedTime <= 0.1f) {
        volumeRate *= 0.9f;
      }
    }

    return volumeRate;
  }

}
}