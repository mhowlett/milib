#include "stdafx.h"
#include <cmath>
#include "milib.h"
#include "image_store.h"
#include "series_store.h"

void cardscan_rectify_raw_image_high_gradient_error(int imageHandle)
{
	int seriesHandle;
	average_column_gradient(imageHandle, &seriesHandle);

	image im = image_store::get(imageHandle);
	series se = series_store::get(seriesHandle);

	int nothingCounter = 0;
	int i = 1;
	for (; i<100 && i <se.length; ++i)
	{
		if (se.data[i] < 4)
		{
			nothingCounter += 1;
		}
		else
		{
			break;
		}
	}

	if (nothingCounter < 10)
	{
		return;
	}

	int highValueHit = 0;
	int somethingCounter = 0;
	int newNothingCounter = 0;
	for (int j=i; j<i+40 && j < se.length; ++j)
	{
		if (se.data[j] > 30)
		{
			highValueHit = 1;
		}

		if (se.data[j] < 4)
		{
			newNothingCounter += 1;
		}
		else
		{
			somethingCounter += 1;
		}
	}

	if (highValueHit == 0)
	{
		return;
	}

	if (newNothingCounter < somethingCounter)
	{
		return;
	}

	for (int j=0; j<i+12 && j < se.length; ++j)
	{

		for (int k=0; k<im.height; ++k)
		{
			im.data[k*im.width+j] = 0;
		}
	}

}