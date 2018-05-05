using UnityEngine;
using UnityEngine.UI;

public class httprequest : MonoBehaviour {
		bool isLoaded = false;

	// Use this for initialization
	void Start () {
				//Debug.Log ("call start");
	}
	
	// Update is called once per frame
		int cnt = 0;
		bool once = false;
		bool once2 = false;
	void Update () {
				if(isLoaded){
						cnt++;
				}
				if(isLoaded){
						if (once == false) {
								if (cnt > 400) {
										GameObject.Find ("RewardText").GetComponent<Text> ().enabled = false;
										GameObject.Find ("reward").GetComponent<Text> ().enabled = false;
										once = true;
								}

						}
						if(once2 == false){
							if(cnt > 400){
									GameObject.Find ("RewardImage").GetComponent<Image> ().enabled = false;
									once2 = true;
							}
							
						}
				}
	}

		public void callReward(){
				StartCoroutine("GetText");
				StartCoroutine("GetImage");
		}

		IEnumerator GetText() {
				//Debug.Log ("get txt");
				UnityWebRequest request = UnityWebRequest.Get("http://binary.orz.hm:8080/unity_reward/txt.php");
				// 下記でも可
				// UnityWebRequest request = new UnityWebRequest("http://example.com");
				// methodプロパティにメソッドを渡すことで任意のメソッドを利用できるようになった
				// request.method = UnityWebRequest.kHttpVerbGET;

				// リクエスト送信
				yield return request.Send();
				//request.Send();
				//Debug.Log ("get txt2");
				string text;
				// 通信エラーチェック
				if (request.isNetworkError) {
						Debug.Log(request.error);
				} else {
						if (request.responseCode == 200) {
								// UTF8文字列として取得する
								 text = request.downloadHandler.text;

								// バイナリデータとして取得する
								byte[] results = request.downloadHandler.data;

								//widhth 600 height 30 font 14
								GameObject.Find("RewardText").GetComponent<Text>().text = text;
								GameObject.Find ("RewardText").GetComponent<Text> ().transform.localPosition = new Vector3 (-0,100,0);
								GameObject.Find ("RewardText").GetComponent<Text> ().enabled = true;

								//widht 600 height 60 
								GameObject.Find ("reward").GetComponent<Text> ().text = "CONGRATULATIONS(TODAY'S REWARD)";
								GameObject.Find ("reward").GetComponent<Text> ().transform.localPosition = new Vector3 (0,130,0);
								GameObject.Find ("reward").GetComponent<Text> ().enabled = true;
						}
				}
	}


		IEnumerator GetTexture() {
				Texture myTexture;
				using(UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://binary.orz.hm:8080/unity_reward/pic.php")) {
						yield return www.Send();


						if (www.isNetworkError) {
								Debug.Log(www.error);
						} else {
								// DownloadHandlerを継承したDownloadHandlerTexture経由で取得できる
								myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
						}


				}
	}

		IEnumerator GetImage()
		{
				//Debug.Log ("get img");
				Texture2D tex = new Texture2D(4, 4, TextureFormat.DXT1 , false);
				WWW Link = new WWW ("http://binary.orz.hm:8080/unity_reward/pic.php");
				yield return Link;
				//Debug.Log ("get img2");
				Link.LoadImageIntoTexture (tex);
				//Image Web_image;
				//Web_image.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0, 0));
				GameObject.Find("RewardImage").GetComponent<Image>().sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0, 0));
				// why width and height do not work? ......so scale..
				GameObject.Find ("RewardImage").transform.localScale = new Vector3 (3,3,3);
				GameObject.Find ("RewardImage").GetComponent<Image> ().enabled = true;

				isLoaded = true;
		}
}
