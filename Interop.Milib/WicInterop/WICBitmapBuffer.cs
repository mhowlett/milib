using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace WICInterop
{
	public struct WICBitmapBuffer
	{
		internal WICBitmapBuffer(IntPtr buffer, uint size)
		{
			this._buffer = buffer;
			this._size = size;
		}

		public bool CopyFrom(IntPtr source, uint size)
		{
			if (size > _size)
			{
				throw new ArgumentException("Size is to big to fit in buffer", "size");
			}
			return CopyBuffer(_buffer, source, size);
		}

		public void CopyTo(IntPtr target)
		{
			this.CopyTo(target, _size);
		}

		public void CopyTo(IntPtr target, uint size)
		{
			if (target == IntPtr.Zero)
			{
				throw new ArgumentException("The target buffer must not be zer", "target");
			}
			if (size > _size)
			{
				throw new ArgumentException("Size is to big to fit in buffer", "size");
			}
			CopyMemory(target, _buffer, size);
		}

		public IntPtr Buffer
		{
			get { return _buffer; }
		}		

		public uint Size
		{
			get { return _size; }
		}

		private static bool CopyBuffer(IntPtr target, IntPtr source, uint size)
		{
			CopyMemory(target, source, size);
			int result = Marshal.GetLastWin32Error();
			if (result != 0)
			{
				// throw new Win32Exception(result, "Failed to copy memory");
				return false;
			}
			return true;
		}

		[DllImport("Kernel32.dll", EntryPoint = "CopyMemory", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

		private readonly IntPtr	_buffer;
		private readonly uint	_size;
	}
}
