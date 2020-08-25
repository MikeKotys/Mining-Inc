using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GreenPandaAssets.Scripts.SaveSystem
{
	/// <summary>Handles save game logic.</summary>
	public class SaveSystem : MonoBehaviour
	{
		public void Save()
		{
			string saveFileText = "";

			// Get all GameObjects in scene with the UniqueID component on them.
			var allSavableGOs = GameObject.FindObjectsOfType<UniqueID>();
			for (int i = 0; i < allSavableGOs.Length; i++)
			{
				var identifier = allSavableGOs[i];
				saveFileText += identifier.uniqueId + "\n";

				// Get all components on the current GameObject
				var allComponents = identifier.GetComponents<Component>();
				for (int j = 0; j < allComponents.Length; j++)
				{
					var component = allComponents[j];
					var savable = component as ISavable;

					// If a GameObject has an ISavable interface - it must be saved.
					if (savable != null)
					{
						saveFileText += savable.GetType().ToString() + "\n";
						savable.Save(ref saveFileText);
					}
				}
				saveFileText += "|\n";
			}
			File.WriteAllText(Application.dataPath + "/Save.txt", saveFileText);
		}

		public void Load()
		{
			StreamReader reader = new StreamReader(Application.dataPath + "/Save.txt");
			if (reader != null)
			{
				// Build a dictionary of all savable objects.
				Dictionary<string, GameObject> allSavables = new Dictionary<string, GameObject>();

				var allSavableGOs = GameObject.FindObjectsOfType<UniqueID>();
				for (int i = 0; i < allSavableGOs.Length; i++)
					allSavables.Add(allSavableGOs[i].uniqueId, allSavableGOs[i].gameObject);

				bool forceExit = false;

				while (!reader.EndOfStream)
				{
					string id = reader.ReadLine();

					if (!allSavables.ContainsKey(id))
					{
#if UNITY_EDITOR
						Debug.LogError("Could not find a GameObject with UniqueID: " + id);
#endif
						break;
					}

					var go = allSavables[id];
					while (true)
					{
						string line = reader.ReadLine();

						if (line == "|" || reader.EndOfStream)
							break;

						var component = go.GetComponent(Type.GetType(line)) as MonoBehaviour;
						var savable = component as ISavable;

						if (savable != null)
						{
							component.StopAllCoroutines();
							if (!savable.Load(reader))
							{
								forceExit = true;
								break;
							}
						}
					}

					if (forceExit)
						break;
				}

				// LateLoad() is needed for objects that rely on values of other objects to function properly.
				foreach (var pair in allSavables)
				{
					var allComponents = pair.Value.GetComponents<Component>();
					for (int j = 0; j < allComponents.Length; j++)
					{
						var component = allComponents[j];
						var savable = component as ISavable;

						if (savable != null)
							savable.LateLoad();
					}
				}
			}
			reader.Close();
			reader.Dispose();
		}
	}
}
