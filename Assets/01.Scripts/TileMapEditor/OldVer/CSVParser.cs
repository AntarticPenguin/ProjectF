using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CSVParser
{
	public void SaveCSV(int width, int height, List<List<GameObject>> tileDatas, string path)
	{
		List<List<string>> rowData = new List<List<string>>();
		{
			//ex)mapSize width, height
			List<string> rowTemp = new List<string>();
			rowTemp.Add("mapSize");
			rowTemp.Add(width.ToString());
			rowTemp.Add(height.ToString());
			rowData.Add(rowTemp);
		}
		{
			//ex)mapData
			List<string> rowTemp = new List<string>();
			rowTemp.Add("mapData");
			rowData.Add(rowTemp);
		}
		{
			for (int y = 0; y < height; y++)
			{
				List<string> rowTemp = new List<string>();
				for (int x = 0; x < width; x++)
				{
					string tileName = tileDatas[y][x].GetComponent<GridTile>()._spriteName;
					float offset = tileDatas[y][x].GetComponent<GridTile>()._offset;
					bool canMove = tileDatas[y][x].GetComponent<GridTile>().CanMove();
					if (!tileName.Equals("none"))
						rowTemp.Add(tileName + "@" + offset.ToString() + "@" + canMove.ToString());
					else
						rowTemp.Add(tileName);
				}
				rowData.Add(rowTemp);
			}
		}
		{
			//output with delimeter ","
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < rowData.Count; i++)
			{
				for (int j = 0; j < rowData[i].Count; j++)
				{
					if (j != rowData[i].Count - 1)
					{
						sb.Append(rowData[i][j] + ",");
					}
					else
					{
						sb.AppendLine(rowData[i][j]);
					}
				}
			}

			StreamWriter outStream = File.CreateText(path);
			outStream.Write(sb);
			outStream.Close();

			Debug.Log("Save completed: " + path);
		}
	}

	int[] _mapSize = new int[2];
	public int[] GetMapSize()
	{
		return _mapSize;
	}

	List<List<string>> _mapData = new List<List<string>>();
	public List<List<string>> GetMapData()
	{
		return _mapData;
	}

	public void ReadCSV(string path)
	{
		StreamReader sr = new StreamReader(path);
		{
			//parsing map size
			string records = sr.ReadLine();
			string[] tokens = records.Split(',');
			_mapSize[0] = int.Parse(tokens[1]);
			_mapSize[1] = int.Parse(tokens[2]);
		}
		{
			//parsing map data
			sr.ReadLine();          //skip tag line
			while (!sr.EndOfStream)
			{
				string records = sr.ReadLine();
				string[] tokens = records.Split(',');
				List<string> rowData = new List<string>();
				for (int i = 0; i < tokens.Length; i++)
				{
					rowData.Add(tokens[i]);
				}
				_mapData.Add(rowData);
			}
		}
		sr.Close();
	}

	public void ReadMapCSV(string name)
	{
		string path = Application.dataPath + "/Resources/MapData/" + name + ".csv";
		//Debug.Log("CSVPaser::ReadCSV: " + path);
		ReadCSV(path);
	}

	//public List<sPortalInfo> ReadMapInfo(string name)
	//{
	//	string path = Application.dataPath + "/Resources/MapData/" + name + "Info.csv";
	//	List<sPortalInfo> info = new List<sPortalInfo>();
	//	try
	//	{
	//		StreamReader sr = new StreamReader(path);
	//		sr.ReadLine();  //pass first row

	//		while (!sr.EndOfStream)
	//		{
	//			string record = sr.ReadLine();
	//			string[] tokens = record.Split(',');
	//			sPortalInfo portal = new sPortalInfo();
	//			portal.portalName = tokens[0];
	//			portal.nextMap = tokens[1];
	//			portal.tileX = int.Parse(tokens[2]);
	//			portal.tileY = int.Parse(tokens[3]);

	//			info.Add(portal);
	//		}
	//	}
	//	catch (IOException exception)
	//	{
	//		Debug.Log("<color=red> Can't find MapInfo File. </color>");
	//		return null;
	//	}
	//	return info;
	//}
}
