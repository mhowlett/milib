#include "stdafx.h"
#include "milib.h"
#include "series_store.h"

void delete_series(int handle)
{
	if (series_store::exists(handle))
	{
		series s = series_store::get(handle);
		delete[] s.data;
		series_store::erase(handle);
	}
}
