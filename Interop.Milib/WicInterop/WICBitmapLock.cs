using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace WICInterop
{
	public struct WICBitmapLock : IDisposable
	{
		internal WICBitmapLock(object wicBitmapLockSafeHandle)
		{
			this._isDisposed = false;
			this._wicBitmapBuffer = null;
			this._wicBitmapLockSafeHandle = wicBitmapLockSafeHandle;			
		}

		public WICBitmapBuffer Data
		{
			get
			{
				if (_isDisposed)
				{
					throw new ObjectDisposedException("_wicBitmapLockSafeHandle");
				}
				if (_wicBitmapBuffer == null)
				{
					object[] args = new object[] { _wicBitmapLockSafeHandle, null, null };
					int result = (int)GetDataPointerMethod.Invoke(null, args);
					if (result != 0)
					{
						throw new Win32Exception(result, "Failed to get data pointer");
					}
					_wicBitmapBuffer = new WICBitmapBuffer((IntPtr)args[2], (uint)args[1]);
				}
				return _wicBitmapBuffer.Value;
			}
		}

		#region Dispose Pattern

		public void Dispose()
		{
			if (_isDisposed)
			{
				return;
			}
			_wicBitmapBuffer = null;
			SafeHandle safeHandle = (SafeHandle)_wicBitmapLockSafeHandle;
			safeHandle.Close();
			safeHandle.Dispose();
		}

		#endregion

		static WICBitmapLock()
		{
			Type internalWICBitmapLockType = Type.GetType("MS.Win32.PresentationCore.UnsafeNativeMethods+WICBitmapLock, PresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
			GetDataPointerMethod = internalWICBitmapLockType.GetMethod("GetDataPointer", BindingFlags.NonPublic | BindingFlags.Static);			
		}

		private bool _isDisposed;
		private readonly object _wicBitmapLockSafeHandle;
		private WICBitmapBuffer? _wicBitmapBuffer;
		private static readonly MethodInfo GetDataPointerMethod;
	}
}
