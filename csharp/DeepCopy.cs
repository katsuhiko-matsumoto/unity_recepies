using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class deepcopy : MonoBehaviour {

		public static T		DeepCopy<T>( T target )
		{
				T result;
				BinaryFormatter b = new BinaryFormatter();
				MemoryStream mem = new MemoryStream();

				try {
						b.Serialize(mem, target);
						mem.Position = 0;
						result = (T)b.Deserialize(mem);
				}
				finally {
						mem.Close();
				}

				return result;
		}

		public static object		DeepCopy(  object target )
		{
				object result;
				BinaryFormatter b = new BinaryFormatter();
				MemoryStream mem = new MemoryStream();

				try {
						b.Serialize(mem, target);
						mem.Position = 0;
						result = b.Deserialize(mem);
				}
				finally {
						mem.Close();
				}

				return result;
		}
}
