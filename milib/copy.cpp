#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void copy(int* new_handle, int handle_to_copy)
{
	image toCopy = image_store::get(handle_to_copy);
	
	image im;
	im.width = toCopy.width;
	im.height = toCopy.height;
	im.data = new unsigned char[im.width*im.height];
	memcpy(im.data, toCopy.data, im.width*im.height);

	*new_handle = image_store::set(im);
}
