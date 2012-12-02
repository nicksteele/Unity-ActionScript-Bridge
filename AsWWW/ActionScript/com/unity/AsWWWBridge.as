package com.unity
{

	import com.unity.*;
	
	import flash.display.*;
	import flash.system.LoaderContext;
	import flash.events.ErrorEvent;
	import flash.events.Event;
	import flash.events.IOErrorEvent;
	import flash.net.URLRequest;
	import flash.net.URLVariables;
	import flash.net.URLRequestMethod;
	import flash.net.URLLoader;
	import flash.display3D.textures.Texture;
	import flash.geom.Matrix;
	
	public class AsWWWBridge
	{
		public var error:String;
		public var text:String;
		
		private var url:String;
		private var urlLoader:URLLoader;
		private var urlRequest:URLRequest;
		private var variables:URLVariables;
		private var isPic:Boolean;
		private static var flipMatrix:Matrix;
		private var loader:Loader;
		private var _isDone:Boolean;
			
			
		public function AsWWWBridge(url:String)
		{
			this.url = url;
			// picture or text data ?
			var elements : Array = new Array();
			elements = url.split(".");

			if(in_array(elements[elements.length-1], new Array("jpg","jpeg","png","gif")) == true){
			    // Is pic
				isPic = true;
			}else{
				// or is text data
				isPic = false;
				variables = new URLVariables();
			}

		}
				
		private function in_array(needle :String, haystack :Array):Boolean{
			var b:Boolean = false
			for(var m:String in haystack){
				if(haystack[m]==needle){
					b = true
					break
				}
			}
			return b
		}
		
		public function AddField(index:String, value:String):void{
			variables[index] = value;
		}
		
		public function load():void{
			if(isPic == true){
				init();
				loadTexture(url);	
			}else{
				urlRequest = new URLRequest(url);
				urlRequest.method = URLRequestMethod.POST; 
				urlRequest.data = this.variables; 
				urlLoader = new URLLoader();
				urlLoader.addEventListener(Event.COMPLETE, onComplete);
				urlLoader.addEventListener(IOErrorEvent.IO_ERROR, onError);
				urlLoader.load(urlRequest);
			}
		}
		private function init():void
		{
			flipMatrix = new Matrix();
			
			loader = new Loader();
			loader.contentLoaderInfo.addEventListener(Event.COMPLETE, onComplete);
			loader.contentLoaderInfo.addEventListener(IOErrorEvent.IO_ERROR, onError);
		}
		
		protected function onError(event:Event):void
		{
			_isDone = true;
			error = event.toString();
		}
		
		protected function onComplete(event:Event):void
		{   
			if(isPic == false)text = urlLoader.data;
			_isDone = true;
			error = null;
		}
		
		public function get width():int
		{
			return loader.width;
		}
		
		public function get height():int
		{
			return loader.height;
		}
		
		public function get isDone():Boolean
		{
			return _isDone;
		}
		
		public function loadTexture(url:String):void
		{
			var loaderContext:LoaderContext = new LoaderContext ();
			loaderContext.checkPolicyFile = true;
			loader.load(new URLRequest(url),loaderContext);	
		}
		
		public function setToTexture(unusedByFlash:*, nativeId:int):void
		{
			var texture:Texture = Stage3DObjectMap.getInstance().getTexture(UnityNative.nativeTextureIDMap[nativeId]) as Texture;
			texture.uploadFromBitmapData(getFlippedBitmapForLoader(loader),0);
		}
		
		private static function getFlippedBitmapForLoader(loader:Loader):BitmapData
		{
			var bmp:BitmapData = new BitmapData(loader.width, loader.height, false, 0);
			flipMatrix.identity();
			flipMatrix.scale(1,-1);
			flipMatrix.translate(0,bmp.height);
			bmp.draw(loader,flipMatrix);
			return bmp;
		}
		
	}
}
