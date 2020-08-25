using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace GreenPandaAssets.Scripts.SaveSystem
{
	/// <summary>Every MonoBehaviour that implements this interface and has UniqueID component on the same GameObject will be saved.</summary>
	public interface ISavable
	{
		void Save(ref string file);
		bool Load(StreamReader reader);
		void LateLoad();
	}
}
