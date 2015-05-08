#pragma once
#include "image.h"

namespace image_store
{
	image get(int handle);
	int set(image);
	void set(int handle, image im);
	bool exists(int handle);
	void erase(int handle);
}
