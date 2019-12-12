namespace KanKikuchi.AudioManager {

using UnityEngine;

/// <summary>
/// BGM関連の管理をするクラス
/// </summary>
public class BGMManager : AudioManager<BGMManager> {

  //AudioPlayerの数(同時再生可能数)
  protected override int _audioPlayerNum => AudioManagerSetting.Entity.BGMAudioPlayerNum;

  //再生に使ってるプレイヤークラス
  private AudioPlayer _audioPlayer => _audioPlayerList[0];

  //オーディオファイルが入ってるディレクトリへのパス
  public static readonly string AUDIO_DIRECTORY_PATH = "BGM";

  //=================================================================================
  //初期化
  //=================================================================================

  //起動時に実行される
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  static void Initialize(){
    if (AudioManagerSetting.Entity.IsAutoGenerateBGMManager) {
      new GameObject("BGMManager", typeof(BGMManager));
    }
  }
  
  protected override void Init() {
    base.Init();
    var setting = AudioManagerSetting.Entity;
    
    LoadAudioClip(AUDIO_DIRECTORY_PATH, setting.BGMCacheType, setting.IsReleaseBGMCache);
    
    ChangeBaseVolume(setting.BGMBaseVolume);
    if (!setting.IsDestroyBGMManager) {
      DontDestroyOnLoad(gameObject);
    }
  }

  //=================================================================================
  //再生
  //=================================================================================

  /// <summary>
  /// 再生
  /// </summary>
  public void Play(AudioClip audioClip, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = true, bool allowsDuplicate = false) {
    //重複が許可されてない場合は、既に再生しているものを止める
    if (!allowsDuplicate) {
      Stop();
    }
    RunPlayer(audioClip, volumeRate, delay, pitch, isLoop);
  }
  
  /// <summary>
  /// 再生
  /// </summary>
  public void Play(string audioPath, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = true, bool allowsDuplicate = false) {
    //重複が許可されてない場合は、既に再生しているものを止める
    if (!allowsDuplicate) {
      Stop();
    }
    RunPlayer(audioPath, volumeRate, delay, pitch, isLoop);
  }

}
}