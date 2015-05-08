#include "stdafx.h"
#include "series.h"
#include <map>

namespace series_store
{
	std::map<int, series> series_collection;
	int current_handle = 1;

	series get(int handle)
	{
		return series_collection[handle];
	}

	int set(series s)
	{
		series_collection[current_handle] = s;
		return current_handle++;
	}

	void set(int handle, series s)
	{
		series_collection[handle] = s;
	}

	bool exists(int handle)
	{
		if (series_collection.find(handle) != series_collection.end())
		{
			return true;
		}
		return false;
	}

	void erase(int handle)
	{
		series_collection.erase(handle);
	}
}
