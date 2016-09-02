///
/// Original created by : Damar Inderajati
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace DI.SoundManager {

	[CreateAssetMenu(fileName = "SoundData", menuName = "Sound/Data", order = 99)]
	public class SoundData : ScriptableObject {
		[System.Serializable]
		public class SMSound {
			public string id = "--Empty--";
			public AudioClip music;
		}

		public SMSound[] backgroundMusics;
		public SMSound[] soundFX;

		public AudioClip GetBackgroundMusic(string id) {
			foreach (SMSound smsound in backgroundMusics) {
				if (smsound.id.Equals(id))
					return smsound.music;
			}
			Debug.Log("BG music with id : " + id + " NOT FOUND");
			return null;
		}
		public AudioClip GetSoundFX(string id)
		{
			foreach (SMSound smsound in soundFX)
			{
				if (smsound.id.Equals(id))
					return smsound.music;
			}
			Debug.Log("Sound FX with id : " + id + " NOT FOUND");
			return null;
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(SoundData))]
	public class SoundDataEditor : Editor {
		ReorderableList backgroundMusics, soundFX;
		string[] sceneNames;

		void OnEnable() {
			sceneNames = ReadNames();

			backgroundMusics = new ReorderableList(serializedObject, serializedObject.FindProperty("backgroundMusics"), true, true, true, true);
			backgroundMusics.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var elemet = backgroundMusics.serializedProperty.GetArrayElementAtIndex(index);
				//EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), elemet.FindPropertyRelative("music"), GUIContent.none);
				elemet.FindPropertyRelative("music").objectReferenceValue = EditorGUI.ObjectField(new Rect(rect.x, rect.y, 2 * rect.width / 3, rect.height * .8f), elemet.FindPropertyRelative("music").objectReferenceValue, typeof(AudioClip), true);
				int indexChoosen = FindIndexID(elemet.FindPropertyRelative("id").stringValue);
				int newIndexChoosen = EditorGUI.Popup(new Rect(rect.x + 2 * rect.width / 3, rect.y, rect.width / 3, rect.height), indexChoosen, sceneNames);
				if (indexChoosen != newIndexChoosen && newIndexChoosen >= 0 && newIndexChoosen < sceneNames.Length)
					elemet.FindPropertyRelative("id").stringValue = sceneNames[newIndexChoosen];
				else if (newIndexChoosen < 0 && newIndexChoosen >= sceneNames.Length)
					elemet.FindPropertyRelative("id").stringValue = "--Empty--";
			};
			backgroundMusics.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(new Rect(rect.x, rect.y, 2 * rect.width / 3, rect.height), "Clip");
				EditorGUI.LabelField(new Rect(rect.x + 2 * rect.width / 3, rect.y, rect.width/3, rect.height), "Scene");
			};

			soundFX = new ReorderableList(serializedObject, serializedObject.FindProperty("soundFX"), true, true, true, true);
			soundFX.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var elemet = soundFX.serializedProperty.GetArrayElementAtIndex(index);
				//EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width / 2, rect.height), elemet.FindPropertyRelative("music"), GUIContent.none);
				elemet.FindPropertyRelative("music").objectReferenceValue = EditorGUI.ObjectField(new Rect(rect.x, rect.y, 2 * rect.width / 3, rect.height * .8f), elemet.FindPropertyRelative("music").objectReferenceValue, typeof(AudioClip), true);
				EditorGUI.PropertyField(new Rect(rect.x + 2 * rect.width / 3, rect.y, rect.width / 3, rect.height * .8f), elemet.FindPropertyRelative("id"), GUIContent.none);
			};
			soundFX.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(new Rect(rect.x, rect.y, 2 * rect.width / 3, rect.height), "Clip");
				EditorGUI.LabelField(new Rect(rect.x + 2 * rect.width / 3, rect.y, rect.width / 3, rect.height), "ID");
			};
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			serializedObject.Update();

			EditorGUILayout.HelpBox("If 2 data or more with the same ID/Scene, prior is the top one", MessageType.Info);

			EditorPrefs.SetBool("BackgroundMusicFold", EditorGUILayout.Foldout(EditorPrefs.GetBool("BackgroundMusicFold", false), "Background Musics"));
			if(EditorPrefs.GetBool("BackgroundMusicFold"))
				backgroundMusics.DoLayoutList();
			EditorPrefs.SetBool("SoundFxFold", EditorGUILayout.Foldout(EditorPrefs.GetBool("SoundFxFold", false), "Sound FX"));
			if (EditorPrefs.GetBool("SoundFxFold"))
				soundFX.DoLayoutList();

			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
		}

		public int FindIndexID(string id) {
			if (id == "")
				return sceneNames.Length - 1;
			int i = 0;
			foreach (string str in sceneNames) {
				if (str.Equals(id))
					return i;
				i += 1;
			}
			Debug.Log("ID sound data not found : " + id + ", returning -1");
			return -1;
		}

		public static string[] ReadNames()
		{
			List<string> temp = new List<string>();
			foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
			{
				if (S.enabled)
				{
					string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
					name = name.Substring(0, name.Length - 6);
					temp.Add(name);
				}
			}
			temp.Add("--Empty--");
			return temp.ToArray();
		}
	}
#endif
}
