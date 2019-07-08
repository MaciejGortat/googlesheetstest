using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Net;
using System.IO;

public class import : EditorWindow
{
	string GoogleSheetsKey = "1Ui7um_2d6HxLJWfDKaaTOLiB-KoAyh5bX5uP8ikdAJ8";
	string GID = "0";
	string data;

	[MenuItem("tools/import")]
	static void Init()
	{
		import window = (import)GetWindow(typeof(import));
		window.Show();
	}

	private static List<string> parseList(string data, string[] splt = null)
	{
		data = data.Replace('\n', ',');
		if (splt == null)
			splt = new string[] { "," };
		var dataList = data.Split(splt, System.StringSplitOptions.RemoveEmptyEntries)
			.Select(s => s.Substring(1, s.Length - 2))
			.ToList();
		dataList.RemoveAt(0);
		return dataList;
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField("import here");
		if (GUILayout.Button("click"))
		{
			Dictionary<string, string> map = new Dictionary<string, string>();
			data = "\"\"," + HttpGet(string.Format(
			"https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:csv&gid={1}&tq={2}",
			GoogleSheetsKey, GID,
			"select%20*%20LIMIT%200"
		)) + ",";
		/*
		List<string> dataList1 = parseList(data);
		int idx = 0;
		dataList1.ForEach(e => {
			map.Add(e, string.Empty + System.Convert.ToChar(idx + 65));
			++idx;
		});
		*/
			Debug.Log(data);
			//Debug.Log(dataList1);
		}
	}

	private static string HttpGet(string url)
	{
		WebClient client = new WebClient();

		client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

		Stream data = client.OpenRead(url);
		StreamReader reader = new StreamReader(data);
		string s = reader.ReadToEnd();
		data.Close();
		reader.Close();

		return s;
	}
}