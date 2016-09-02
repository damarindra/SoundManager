# SoundManager

Easy ways to manage your audio game on unity3D

## Getting Started

Import package, drag and drop SoundManager.prefab to your very first scene loaded. Open / Click SoundData.asset (found at SoundManager/Resources), and configure / drag and drop your audio asset to inspector.

**Note : Add your scene to Build Settings, scene info on SoundData.asset is take from Build Settings**

## SoundManager

1. Auto Change BG Music : auto change the background music when loaded new scene.
2. Restart BG Music When Loaded : restart the music when loaded new scene, this option is only when you want to restart the same audio clip
3. Sound Fx Source Size : Sound FX pooled gameObject created at the very first.

## Code Guidelines

1. SoundManager.instance.ChangeBackgroundMusic(string id) : call when you want to change the background music, string id is scene name.
2. SoundManager.instance.ChangeBackgroundMusic(AudioClip clip) : call when you want to change the background music
3. PlayClip(string id, Vector3 position) : playing a sound FX with a specific position. Only used for 3D. string id is pre-cofigured on GameData.asset
4. PlayClip2D(string id) : playing a sound FX with 2D blend, string id is pre-cofigured on GameData.asset
5. PlayClip2D(AudioClip) : playing a soundFX with 2D blend

## Musics credits to

Cheer up (PSG Version) : [Snabisch](http://opengameart.org/users/snabisch)