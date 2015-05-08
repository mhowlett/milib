#include "stdafx.h"
#include "milib.h"
#include "series_store.h"

void get_series(int handle, float** bytes, int* length)
{
	series i = series_store::get(handle);

	*bytes = i.data;
	*length = i.length;
}
