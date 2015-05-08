#include "stdafx.h"
#include "milib.h"
#include "image_store.h"
#include <cmath>

void hough_transform(int handle, int* outHandle, float minAngle, float maxAngle, int numberAngles)
{
	image im = image_store::get(handle);

	int totalAngles = numberAngles;
	const int totalRs = 200;
	const float maxR = (im.width > im.height ? im.width : im.height)*1.2;

	int* result = new int[totalAngles*totalRs];
	for (int i=0; i<totalAngles*totalRs; ++i)
	{
		result[i] = 0;
	}

	for (int k=0; k<totalAngles; ++k)
	{
		float angle = ((float)k / (float)(totalAngles - 1)) * (maxAngle - minAngle) + minAngle;
		float angleRadians = angle / 180.0f * 3.14159265358f;

		float cosAngle = (float)cos((double)angleRadians);
		float sinAngle = (float)sin((double)angleRadians);

		for (int i=0; i<im.width; ++i)
		{
			for (int j=0; j<im.height; ++j)
			{
				if (im.data[im.width*j + i] <40)
				{
					continue;
				}

				float r = (i)*cosAngle + (j)*sinAngle;

				if (r < 0)
				{
					r = -r;
				}

				int rBin = r/maxR*totalRs;
				if (rBin >= totalRs)
				{
					rBin = totalRs-1;
				}

				result[totalAngles*rBin + k] += 1;
			}
		}
	}

	int maxRbin = 0;
	for (int i=0; i<totalAngles*totalRs; ++i)
	{
		if (result[i] > maxRbin)
		{
			maxRbin = result[i];
		}
	}

	image r;
	r.data = new unsigned char[totalAngles*totalRs];
	r.width = totalAngles;
	r.height = totalRs;

	for (int i=0; i<totalAngles*totalRs; ++i)
	{
		r.data[i] = (unsigned char) ((float)result[i] / (float)maxRbin * 255.0f);
	}

	*outHandle = image_store::set(r);
}
