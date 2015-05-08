#include "stdafx.h"
#include <cmath>
#include "milib.h"
#include "image_store.h"
#include "series_store.h"

void cardscan_determine_card_bounds(int handle, int threshold, int* min_x, int* max_x, int* min_y, int* max_y)
{
	image im = image_store::get(handle);

	int sh;

	average_row_intensity(handle, &sh);
	series rs = series_store::get(sh);

	average_column_intensity(handle, &sh);
	series cs = series_store::get(sh);

	*min_y = rs.length-1;
	for (int i=0; i<rs.length; ++i)
	{
		if (rs.data[i] > threshold)
		{
			*min_y = i;
			break;
		}
	}

	*max_y = 0;
	for (int i=rs.length-1; i>=0; --i)
	{
		if (rs.data[i] > threshold)
		{
			*max_y = i;
			break;
		}
	}
	
	if (*max_y < *min_y)
	{
		*max_y = rs.length-1;;
		*min_y = 0;
	}


	*min_x = rs.length-1;
	for (int i=0; i<cs.length; ++i)
	{
		if (cs.data[i] > threshold)
		{
			*min_x = i;
			break;
		}
	}

	*max_x = 0;
	for (int i=cs.length-1; i>=0; --i)
	{
		if (cs.data[i] > threshold)
		{
			*max_x = i;
			break;
		}
	}
	
	if (*max_x < *min_x)
	{
		*max_x = rs.length-1;;
		*min_x = 0;
	}
}
