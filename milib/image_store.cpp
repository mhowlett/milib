#include "stdafx.h"
#include "image.h"
#include <map>

namespace image_store
{
	std::map<int, image> image_collection;
	int current_handle = 1;

	image get(int handle)
	{
		return image_collection[handle];
	}

	int set(image i)
	{
		image_collection[current_handle] = i;
		return current_handle++;
	}
	
	void set(int handle, image im)
	{
		image_collection[handle] = im;
	}

	bool exists(int handle)
	{
		if (image_collection.find(handle) != image_collection.end())
		{
			return true;
		}
		return false;
	}

	void erase(int handle)
	{
		image_collection.erase(handle);
	}
}
