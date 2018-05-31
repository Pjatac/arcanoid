using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace arcanoid
{
	class XMLIO
	{
		public static async Task<int[][]> ReadObjectFromXmlFileAsync(string name)
		{
			int[][] objectFromXml;
			var serializer = new XmlSerializer(typeof(int[][]));
			StorageFolder folder = ApplicationData.Current.LocalFolder;
			var _File = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/" + name + ".xml"));
			Stream stream = await _File.OpenStreamForReadAsync();
			objectFromXml = (int[][])serializer.Deserialize(stream);
			stream.Dispose();
			return objectFromXml;
		}

		public static async Task SaveObjectToXml(int[][] ToSave, string name)
		{
			var serializer = new XmlSerializer(typeof(int[][]));
			StorageFolder folder = ApplicationData.Current.LocalFolder;

			string fileName = folder.Path +"/"+  name + ".xml";
			if (File.Exists(fileName))
				File.Delete(fileName);

			await folder.CreateFileAsync(name + ".xml");
			var _File = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/" + name + ".xml"));
			Stream stream = await _File.OpenStreamForWriteAsync();

			using (stream)
			{
				serializer.Serialize(stream, ToSave);
			}
		}
		static public async void Load(string playerName, GameBoard Game, Canvas myCanvas)
		{
			int[][] FromLoad = await XMLIO.ReadObjectFromXmlFileAsync(playerName);
			Game._score = FromLoad[0][0];
			Game._lives = FromLoad[0][1];
			Game._level = FromLoad[0][2];
			Game._gameObjects[0].TopLeftPosistionX = FromLoad[1][1];
			for (int i = 2; i < FromLoad.Length; i++)
			{
				if (FromLoad[i][0] == 2)
				{
					var monster = new MonsterObject(myCanvas, FromLoad[i][1], FromLoad[i][2], Game._level);
					Game._gameObjects.Add(monster);
				}
				if (FromLoad[i][0] == 3)
				{
					var bullet = new BulletObject(myCanvas, FromLoad[i][1], FromLoad[i][2]);
					Game._gameObjects.Add(bullet);
				}
			}
		}
		static public int[][] Trans(GameBoard Game, List<IGameObject> OnBoard)
		{
			int i = 0;
			int[][] ToSave = new int[OnBoard.Count + 1][];
			for (; i < OnBoard.Count + 1; i++)
				ToSave[i] = new int[3];
			ToSave[0][0] = Game._score;
			ToSave[0][1] = Game._lives;
			ToSave[0][2] = Game._level;
			i = 1;
			foreach (IGameObject obj in OnBoard)
			{
				if (obj is PlayerObject)
				{
					ToSave[i][0] = 1;
					ToSave[i][1] = obj.TopLeftPosistionX;
					ToSave[i][2] = obj.TopLeftPosistionY;
					i++;
				}
				if (obj is MonsterObject)
				{
					ToSave[i][0] = 2;
					ToSave[i][1] = obj.TopLeftPosistionX;
					ToSave[i][2] = obj.TopLeftPosistionY;
					i++;
				}
				if (obj is BulletObject)
				{
					ToSave[i][0] = 3;
					ToSave[i][1] = obj.TopLeftPosistionX;
					ToSave[i][2] = obj.TopLeftPosistionY;
					i++;
				}
			}
			return ToSave;
		}
	}
}
