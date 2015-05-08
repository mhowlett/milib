#pragma once
#include "series.h"

namespace series_store
{
	series get(int handle);
	int set(series);
	bool exists(int handle);
	void erase(int handle);
}
