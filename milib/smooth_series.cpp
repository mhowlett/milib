#include "stdafx.h"
#include "milib.h"
#include "series_store.h"

/*
void smooth_series(int handle, int kernel_type)
{
	series se = series_store::get(handle);

	int kernel_length = 0;
	float kernel[10];
	if (kernel_type == 0)
	{
		kernel_length = 3;
		kernel[0] = 0.33333f;
		kernel[1] = 0.33333f;
		kernel[2] = 0.33333f;
	}
	else if (kernel_type = 1)
	{
		kernel_length = 3;
		kernel[0] = 0.25f;
		kernel[1] = 0.5f;
		kernel[2] = 0.25f;
	}
	else
	{
		kernel_length = 5;
		kernel[0] = 0.1f;
		kernel[1] = 0.2f;
		kernel[2] = 0.4f;
		kernel[3] = 0.2f;
		kernel[4] = 0.1f;
	}

	float* ns = new float[se.length - kernel_length + 1];
	for (int i=0; i< se.length - kernel_length; ++i)
	{
	}

	delete[] se.data;
	se.data = ns;
	se.length = se.length - kernel_length;
}
*/
