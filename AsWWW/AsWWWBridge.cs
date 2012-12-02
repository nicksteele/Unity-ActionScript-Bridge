using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[NotConverted]
[NotRenamed]
public class AsWWWBridge {

#if UNITY_EDITOR
	private WWW www;
	private string url;
	private WWWForm formGet=null;
	private Texture2D texture;
#else
	private WWW www;
#endif
	
	[NotRenamed]
	public AsWWWBridge(string url){
		#if UNITY_EDITOR
			this.url = url;
		#endif	
	}
	
	[NotRenamed]
	public void AddField(string index, string value){
		#if UNITY_EDITOR
			if(formGet == null)formGet = new WWWForm();
			formGet.AddField(index, value);
		#endif
	}
	
	[NotRenamed]
	public void load(){
		#if UNITY_EDITOR	
			if(formGet != null)www = new WWW(url, formGet);
			else www = new WWW(url);
		#endif	
	}
	
	[NotRenamed]
	public bool isDone
	//#if UNITY_EDITOR
		{
			get {return www.isDone;}
		}
	//#else
		//;
	//#endif
	
	[NotRenamed]
	public string text
	//#if UNITY_EDITOR
		{
			get {return isDone ? www.text : "";}
		}
	//#else
	//	;
	//#endif
	
	[NotRenamed]
	public string error
	//#if UNITY_EDITOR
		{
			get {return isDone ? www.error : "";}
		}
	//#else
	//	;
	//#endif
	
	[NotRenamed]
	public void setToTexture(Texture2D tex, int nativeId)
	{
	#if UNITY_EDITOR
		www.LoadImageIntoTexture(tex);
	#endif
	}
	
	[NotRenamed]
	public int width
	//#if UNITY_EDITOR
	{
		get {return isDone ? www.texture.width : 0;}
	}
	//#else
	//;
	//#endif
	
	[NotRenamed]
	public int height
	//#if UNITY_EDITOR
	{
		get {return isDone ? www.texture.height : 0;}
	}
	//#else
	//	;
	//#endif
}
