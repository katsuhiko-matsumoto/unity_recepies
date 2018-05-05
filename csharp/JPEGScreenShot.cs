using System.IO;
using System;
using System.Text;


public class JPGScreenShot : MonoBehaviour
{
	private WebCamDevice[] devices;
	private WebCamTexture webcamTextureOrg;
	private WebCamTexture webcamTexture;
	private int camx = appconfig.CAMERA_WIDTH;
	private int camy = appconfig.CAMERA_HEIGHT;
	private float savefps = appconfig.CAMERA_SAVE_FPS;
	private int CAMERAFPS = appconfig.CAMERA_FPS;
	private float jpegQuality = appconfig.JPEG_QUALITY;
	private float updateTime;
	
	private socket sc;
	// Use this for initialization
	void Start ()
	{
		GameObject player = GameObject.Find("3rd Person Controller");
		sc = (socket)player.GetComponent("socket");
		if(appconfig.IS_HEAT){
			savefps = appconfig.HEAT_CAMERA_SAVE_FPS;
		}

		//Debug.Log ("Taking JPGScreenShot");
		updateTime = Time.time;
		StartCoroutine (ScreenShot ());
		
	}

	IEnumerator ScreenShot ()
	{
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
		devices = WebCamTexture.devices;
		webcamTexture = new WebCamTexture(camx,camy,CAMERAFPS);
	    //webcamTexture = new WebCamTexture(devices[0].name, 100, 100, CAMERAFPS);
		int deviceLength = devices.Length;
	    if (deviceLength > 0) {
			//webcamTextureOrg.deviceName = devices[0].name;
	        webcamTexture.deviceName = devices[0].name;

			//webcamTextureOrg.Play();
			webcamTexture.Play();
			while (!webcamTexture.isPlaying) {
				yield return new WaitForSeconds (3f);
			}
			//Color[] colors =  webcamTexture.GetPixels();
			//Debug.Log("cl length:"+colors.Length);
	    } else {
	        Debug.LogError("no cams");		
		}
	}
	
	IEnumerator Save ()
	{
		if(webcamTexture != null && webcamTexture.GetPixels().Length > 256){
			Texture2D t2d = getWebcomShot(webcamTexture);
			
			//webcamTextureOrg.Stop();
			JPGEncoder encoder = new JPGEncoder (t2d, jpegQuality);

			while (!encoder.isDone) {
				yield return new WaitForSeconds (1f);
			}
			
			FileStream    BinaryFile = new FileStream("screenshot.jpg", FileMode.Create, FileAccess.ReadWrite);
	        //BinaryReader  Reader     = new BinaryReader(BinaryFile);
	        BinaryWriter  Writer     = new BinaryWriter(BinaryFile);
	        
	        Writer.Write(encoder.GetBytes());
		}
	}
	
	IEnumerator Send(){
		//generage 2d picuture
		if(webcamTexture != null && webcamTexture.width >= camx && webcamTexture.height >= camy){

			Texture2D t2d = getWebcomShot(webcamTexture);
			
			//generate jpeg
			JPGEncoder encoder = new JPGEncoder (t2d, jpegQuality);

			while (!encoder.isDone) {
				yield return new WaitForSeconds (1f);
			}
			
			//generate base64 encode
			string encodedText = Convert.ToBase64String (encoder.GetBytes());
			//generate json
			JSONObject sendData  = new JSONObject(JSONObject.Type.OBJECT);
			sendData.AddField("sc","userimage");
			sendData.AddField("sm","drawCamera");
			sendData.AddField("nickname",sc.getNickname());
			sendData.AddField("width",camx);
			sendData.AddField("height",camy);
			sendData.AddField("data",encodedText);

			//send to server
			if(sc.getHashedId() != ""){
				sc.emit("image", sendData.ToString());
			}
		}
	}

	Texture2D getWebcomShot(WebCamTexture wct){
		Color[] colors =  wct.GetPixels();
		//Debug.Log("webcamtexture length:"+colors.Length);
		//Debug.Log("camx:"+camx+" camy:"+camy);
		int xinc = (int)(wct.width / camx);
		int yinc = (int)(wct.height / camy);
		Texture2D t2d = new Texture2D(camx,camy); //hori vart
		//Debug.Log("colors length"+colors.Length);
		int num = 0;
		for(int y = 0, _y =0; _y <= camy; y+= yinc, _y++){	//vertical
			for(int x = 0, _x=0; _x <= camx; x+= xinc, _x++){ //horizental
				//t2d.SetPixel(x,y,colors[num]);
				t2d.SetPixel(_x,_y, wct.GetPixel(x,y));
				num++;
			}
		}
		return t2d;
	}
	
	void Update(){
		
		if((Time.time-updateTime)>savefps){
			//StartCoroutine (Save ());
			StartCoroutine (Send ());
			updateTime = Time.time;
		}
	}
	
}