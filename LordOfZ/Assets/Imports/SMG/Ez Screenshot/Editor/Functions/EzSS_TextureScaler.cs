/// <summary>
/// Thanks to:
/// Petersvp https://pastebin.com/qkkhWs2J
/// Eric Haines http://wiki.unity3d.com/index.php/TextureScale
/// </summary>
using System.Threading;
using UnityEngine;
using System;

namespace SMG.EzScreenshot
{
	public class EzSS_TextureScaler
	{ 
		public static void ScaleTexture(Texture2D source, int targetWidth, int targetHeight, bool gpuScaling, FilterMode mode)
		{
			if(gpuScaling)
			{
				GpuScaling(source, targetWidth, targetHeight, mode);
			}
			else
			{
				switch(mode)
				{
				case FilterMode.Point:
					Point(source, targetWidth, targetHeight);
					break;
				
				case FilterMode.Bilinear:
					Bilinear(source, targetWidth, targetHeight);
					break;
				
				case FilterMode.Trilinear:
					Bilinear(source, targetWidth, targetHeight);
					break;
				}
			}
		}
		
		public static void ScaleTextureProportionally(Texture2D source, int amount, bool gpuScaling, FilterMode mode)
		{
			// Figure out the ratio
			double _ratioX = (double)(source.width + amount) / (double) source.width;
			double _ratioY = (double) (source.height + amount) / (double) source.height;
			// Use wichever multiplier is smaller
			double _ratio = _ratioX < _ratioY ? _ratioX : _ratioY;
			// Calculates the new width and height
			int _newWidth = Convert.ToInt32(source.width * _ratio);
			int _newHeight = Convert.ToInt32(source.height * _ratio);

			if(gpuScaling)
			{
				GpuScaling(source, _newWidth, _newHeight, mode);
			}
			else
			{
				switch(mode)
				{
				case FilterMode.Point:
					Point(source, _newWidth, _newHeight);
					break;
				
				case FilterMode.Bilinear:
					Bilinear(source, _newWidth, _newHeight);
					break;
				
				case FilterMode.Trilinear:
					Bilinear(source, _newWidth, _newHeight);
					break;
				}
			}
		}
    	
		private static void GpuScaling(Texture2D tex, int width, int height, FilterMode mode)
		{
			Rect texR = new Rect(0,0,width,height);
			GpuScalingProcess(tex,width,height,mode);
			// Update new texture
			tex.Resize(width, height);
			tex.ReadPixels(texR,0,0,true);
			// tex.Apply(true);        //Remove this if you hate us applying textures for you :)
		}
               
		// Internal unility that renders the source texture into the RTT - the scaling method itself.
		private static void GpuScalingProcess(Texture2D src, int width, int height, FilterMode fmode)
		{
			//We need the source texture in VRAM because we render with it
			src.filterMode = fmode;
			// src.Apply(true);       
                               
			//Using RTT for best quality and performance. Thanks, Unity 5
			RenderTexture rtt = new RenderTexture(width, height, 32);
               
			//Set the RTT in order to render to it
			Graphics.SetRenderTarget(rtt);
               
			//Setup 2D matrix in range 0..1, so nobody needs to care about sized
			GL.LoadPixelMatrix(0,1,1,0);
               
			//Then clear & draw the texture to fill the entire RTT.
			GL.Clear(true,true,new Color(0,0,0,0));
			Graphics.DrawTexture(new Rect(0,0,1,1),src);
		}
    	
	    public class ThreadData
	    {
		    public int start;
		    public int end;
		    public ThreadData (int s, int e) {
			    start = s;
			    end = e;
		    }
	    }
 
	    private static Color[] texColors;
	    private static Color[] newColors;
	    private static int w;
	    private static float ratioX;
	    private static float ratioY;
	    private static int w2;
	    private static int finishCount;
	    private static Mutex mutex;
 
	    private static void Point (Texture2D tex, int newWidth, int newHeight)
	    {
		    ThreadedScale (tex, newWidth, newHeight, false);
	    }
 
		private static void Bilinear (Texture2D tex, int newWidth, int newHeight)
	    {
		    ThreadedScale (tex, newWidth, newHeight, true);
	    }
 
		private static void ThreadedScale (Texture2D tex, int newWidth, int newHeight, bool useTrilinear)
	    {
		    texColors = tex.GetPixels();
		    newColors = new Color[newWidth * newHeight];
		    if (useTrilinear)
		    {
			    ratioX = 1.0f / ((float)newWidth / (tex.width-1));
			    ratioY = 1.0f / ((float)newHeight / (tex.height-1));
		    }
		    else {
			    ratioX = ((float)tex.width) / newWidth;
			    ratioY = ((float)tex.height) / newHeight;
		    }
		    w = tex.width;
		    w2 = newWidth;
		    var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
		    var slice = newHeight/cores;
 
		    finishCount = 0;
		    if (mutex == null) {
			    mutex = new Mutex(false);
		    }
		    if (cores > 1)
		    {
			    int i = 0;
			    ThreadData threadData;
			    for (i = 0; i < cores-1; i++) {
				    threadData = new ThreadData(slice * i, slice * (i + 1));
				    ParameterizedThreadStart ts = useTrilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
				    Thread thread = new Thread(ts);
				    thread.Start(threadData);
			    }
			    threadData = new ThreadData(slice*i, newHeight);
			    if (useTrilinear)
			    {
				    BilinearScale(threadData);
			    }
			    else
			    {
				    PointScale(threadData);
			    }
			    while (finishCount < cores)
			    {
				    Thread.Sleep(1);
			    }
		    }
		    else
		    {
			    ThreadData threadData = new ThreadData(0, newHeight);
			    if (useTrilinear)
			    {
				    BilinearScale(threadData);
			    }
			    else
			    {
				    PointScale(threadData);
			    }
		    }
 
		    tex.Resize(newWidth, newHeight);
		    tex.SetPixels(newColors);
		    // tex.Apply();
 
		    texColors = null;
		    newColors = null;
	    }
 
		private static void BilinearScale (System.Object obj)
	    {
		    ThreadData threadData = (ThreadData) obj;
		    for (var y = threadData.start; y < threadData.end; y++)
		    {
			    int yFloor = (int)Mathf.Floor(y * ratioY);
			    var y1 = yFloor * w;
			    var y2 = (yFloor+1) * w;
			    var yw = y * w2;
 
			    for (var x = 0; x < w2; x++) {
				    int xFloor = (int)Mathf.Floor(x * ratioX);
				    var xLerp = x * ratioX-xFloor;
				    newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor+1], xLerp),
					    ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor+1], xLerp),
					    y*ratioY-yFloor);
			    }
		    }
 
		    mutex.WaitOne();
		    finishCount++;
		    mutex.ReleaseMutex();
	    }
 
	    private static void PointScale (System.Object obj)
	    {
		    ThreadData threadData = (ThreadData) obj;
		    for (var y = threadData.start; y < threadData.end; y++)
		    {
			    var thisY = (int)(ratioY * y) * w;
			    var yw = y * w2;
			    for (var x = 0; x < w2; x++) {
				    newColors[yw + x] = texColors[(int)(thisY + ratioX*x)];
			    }
		    }
 
		    mutex.WaitOne();
		    finishCount++;
		    mutex.ReleaseMutex();
	    }
 
	    private static Color ColorLerpUnclamped (Color c1, Color c2, float value)
	    {
		    return new Color (c1.r + (c2.r - c1.r)*value, 
			    c1.g + (c2.g - c1.g)*value, 
			    c1.b + (c2.b - c1.b)*value, 
			    c1.a + (c2.a - c1.a)*value);
	    }
    }
}