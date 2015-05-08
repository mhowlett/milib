using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel;

namespace WICInterop
{
	[Flags]
	public enum LockFlags
	{
		Sync = 1,
		Read = 2,
		Write = 4,
		ReadWrite = 6
	}

	public sealed class WICBitmap : IDisposable
	{
		public WICBitmap(BitmapSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this._pixelWidth = source.PixelWidth;
			this._pixelHeight = source.PixelHeight;
			Type bitmapSource = typeof(BitmapSource);
			FieldInfo wicSourceField = bitmapSource.GetField(
				"_wicSource", BindingFlags.NonPublic | BindingFlags.Instance);
			_wicBitmap = wicSourceField.GetValue(source);			
		}

		public int PixelWidth
		{
			get { return _pixelWidth; }
		}

		public int PixelHeight
		{
			get { return _pixelHeight; }
		}

		public bool TryLock(out WICBitmapLock wicImageLock)
		{
			return this.TryLock(0, 0, PixelWidth, PixelHeight, out wicImageLock);
		}

		public WICBitmapLock Lock()
		{
			return this.Lock(PixelWidth, PixelHeight);
		}

		public WICBitmapLock Lock(int width, int height)
		{
			return this.Lock(0, 0, width, height);
		}

		public WICBitmapLock Lock(int x, int y, int width, int height)
		{
			Int32Rect lockRect = new Int32Rect(x, y, width, height);
			object[] args = new object[] { _wicBitmap, lockRect, LockFlags.Read, null };
			int result = (int)LockMethod.Invoke(null, args);
			if (result != 0)
			{
				throw new Win32Exception(result, "Failed to lock bytes");
			}
			WICBitmapLock wicImageLock = new WICBitmapLock(args[3]);
			return wicImageLock;
		}

		public bool TryLock(int x, int y, int width, int height, out WICBitmapLock wicImageLock)
		{
			Int32Rect lockRect = new Int32Rect(x, y, width, height);
			object[] args = new object[] { _wicBitmap, lockRect, LockFlags.Read, null };
			int result = (int)LockMethod.Invoke(null, args);
			wicImageLock = new WICBitmapLock(args[3]);
			return result == 0;
		}

		#region Dispose Pattern

		~WICBitmap()
		{
			Dispose(false);
		}

		public void Dispose()
		{			
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (_isDisposed)
			{
				return;
			}
			if (isDisposing)
			{
				// No managed instances to dispose
			}
			SafeHandle safeHandle = (SafeHandle)_wicBitmap;
			safeHandle.Close();
			safeHandle.Dispose();
		}

		#endregion

		static WICBitmap()
		{
			Type internalWICBitmapType = Type.GetType("MS.Win32.PresentationCore.UnsafeNativeMethods+WICBitmap, PresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
			LockMethod = internalWICBitmapType.GetMethod("Lock", BindingFlags.NonPublic | BindingFlags.Static);
		}

		private bool _isDisposed = false;
		private readonly int _pixelWidth;
		private readonly int _pixelHeight;
		private readonly object _wicBitmap;
		
		private static readonly MethodInfo LockMethod;		
	}
}
