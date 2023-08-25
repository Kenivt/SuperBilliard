using UnityEngine;
using GameFramework;
using GameFramework.Sound;
using UnityGameFramework.Runtime;

namespace SuperBilliard
{
    public static class SoundComponentExtension
    {

        private static int? _musicSerilizeId = null;

        public static void PlayMusic(this SoundComponent soundComponent, EnumSound musicId, Entity bindingEntity = null, object userdata = null)
        {
            if (musicId == EnumSound.None)
            {
                return;
            }
            if (_musicSerilizeId != null)
            {
                soundComponent.StopSound(_musicSerilizeId.Value);
            }
            _musicSerilizeId = soundComponent.PlaySound(musicId, bindingEntity, null, userdata);
        }

        public static void StopMusic(this SoundComponent soundComponent)
        {
            if (_musicSerilizeId == null)
            {
                return;
            }
            soundComponent.StopSound(_musicSerilizeId.Value);
        }
        public static int PlaySound(this SoundComponent soundComponent, EnumSound soundId)
        {
            return soundComponent.PlaySound(soundId, null);
        }

        public static int PlaySound(this SoundComponent soundComponent, EnumSound soundId, Entity bindingEntity, float? volume = null, object userdata = null)
        {
            int id = (int)soundId;
            if (GameEntry.DataBundle.GetData<SoundDataBundle>().GetSoundData(id, out SoundData soundData))
            {
                PlaySoundParams playSoundParams = PlaySoundParams.Create();
                playSoundParams.Time = soundData.Params.Time;
                playSoundParams.MuteInSoundGroup = soundData.Params.Mute;
                playSoundParams.Loop = soundData.Params.Loop;
                playSoundParams.Priority = soundData.Params.Priority;
                playSoundParams.VolumeInSoundGroup = volume == null ? soundData.Params.Volume : volume.Value;
                playSoundParams.FadeInSeconds = soundData.Params.FadeInSeconds;
                playSoundParams.Pitch = soundData.Params.Pitch;
                playSoundParams.PanStereo = soundData.Params.PanStereo;
                playSoundParams.SpatialBlend = soundData.Params.SpatialBlend;
                playSoundParams.MaxDistance = soundData.Params.MaxDistance;
                playSoundParams.DopplerLevel = soundData.Params.DopplerLevel;

                return soundComponent.PlaySound(soundData.AssetPathName, soundData.Group.Name, Constant.AssetPriority.MusicAsset, playSoundParams, bindingEntity, userdata);
            }
            else
            {
                Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
            }

            return -1;
        }

        public static int PlaySound(this SoundComponent soundComponent, EnumSound soundId, Vector3 position, float? volume = null, object userdata = null)
        {
            int id = (int)soundId;
            if (GameEntry.DataBundle.GetData<SoundDataBundle>().GetSoundData(id, out SoundData soundData))
            {
                PlaySoundParams playSoundParams = new PlaySoundParams();
                playSoundParams.Time = soundData.Params.Time;
                playSoundParams.MuteInSoundGroup = soundData.Params.Mute;
                playSoundParams.Loop = soundData.Params.Loop;
                playSoundParams.Priority = soundData.Params.Priority;
                playSoundParams.VolumeInSoundGroup = volume == null ? soundData.Params.Volume : volume.Value;
                playSoundParams.FadeInSeconds = soundData.Params.FadeInSeconds;
                playSoundParams.Pitch = soundData.Params.Pitch;
                playSoundParams.PanStereo = soundData.Params.PanStereo;
                playSoundParams.SpatialBlend = soundData.Params.SpatialBlend;
                playSoundParams.MaxDistance = soundData.Params.MaxDistance;
                playSoundParams.DopplerLevel = soundData.Params.DopplerLevel;

                return soundComponent.PlaySound(soundData.AssetPathName, soundData.Group.Name, Constant.AssetPriority.MusicAsset, playSoundParams, position, userdata);
            }
            else
            {
                Log.Warning("Can not load sound '{0}' from data table.", soundId.ToString());
            }
            return -1;
        }


        public static float GetVolume(this SoundComponent soundComponent, string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return 0f;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return 0f;
            }

            return soundGroup.Volume;
        }

        public static void SetVolume(this SoundComponent soundComponent, string soundGroupName, float volume)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                Log.Warning("Sound group is invalid.");
                return;
            }

            ISoundGroup soundGroup = soundComponent.GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                Log.Warning("Sound group '{0}' is invalid.", soundGroupName);
                return;
            }

            soundGroup.Volume = volume;

            GameEntry.Setting.SetFloat(Utility.Text.Format(SuperBilliard.Constant.GameSetting.SoundGroupVolume, soundGroupName), volume);
            GameEntry.Setting.Save();
        }

        public static void SetMusicVolume(this SoundComponent soundComponent, float volume)
        {
            soundComponent.SetVolume("Music", volume);
        }
        public static void SetSFXVolume(this SoundComponent soundComponent, float volume)
        {
            soundComponent.SetVolume("SFX", volume);
        }
        public static void SetUISoundVolume(this SoundComponent soundComponent, float volume)
        {
            soundComponent.SetVolume("SFX/UI", volume);
        }
        public static float GetMusicVoluem(this SoundComponent soundComponent)
        {
            return soundComponent.GetVolume("Music");
        }
        public static float GetSFXVolume(this SoundComponent soundComponent)
        {
            return soundComponent.GetVolume("SFX");
        }
        public static float GetUISoundVolume(this SoundComponent soundComponent)
        {
            return soundComponent.GetVolume("SFX/UI");
        }
    }
}