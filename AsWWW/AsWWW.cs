using UnityEngine;
using UnityEngine.Flash;
using System.Collections;


 
public class AsWWW{
	
	public Texture2D texture;
	public string text;
	public string error;
	public bool isDone = false;
	public delegate void DataReponse(string text);
	public delegate void TextureReponse(Texture2D asWww);
	public delegate void Error(string error);
	private AsWWWBridge asWww;
	
	
	public AsWWW(string url){
			ActionScript.Import("com.unity.AsWWWBridge");
			asWww = new AsWWWBridge(url);
	}
	
	
	public void AddField(string index, string value){
			asWww.AddField(index, value);
	}
	
	public IEnumerator loadData(DataReponse reponse = null, Error errorReponse=null){	
			if(reponse == null)reponse = this.setLoadData;
			if(errorReponse == null)errorReponse = setError;
			asWww.load(); 
			while(! asWww.isDone )yield return null;	
		
			if(asWww.error == null)reponse(asWww.text);
			else errorReponse(asWww.error);
	}
	
	public IEnumerator loadTexture(TextureReponse reponse = null, Error errorReponse=null){
			if(reponse == null)reponse = this.setLoadTextureVar;
			if(errorReponse == null)errorReponse = setError;
			asWww.load(); 
			while(! asWww.isDone )yield return null;	
			Texture2D texture = new Texture2D(asWww.height,asWww.width,TextureFormat.RGB24,false);
			asWww.setToTexture(texture, texture.GetNativeTextureID());
			if(asWww.error == null)reponse(texture);
			else errorReponse(asWww.error);
	}
	
	private void setLoadData(string text){
			this.isDone = true;
			this.text = text;	
	}
	
	private void setLoadTextureVar(Texture2D texture){
		    this.isDone = true;
			this.texture = texture;
	}
	
	private void setError(string error){
			this.isDone = true;
			this.error = error;
	}
}
