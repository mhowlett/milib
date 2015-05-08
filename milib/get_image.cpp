#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void get_image(int handle, unsigned char** bytes, int* width, int* height)
{
	image i = image_store::get(handle);

	*bytes = i.data;
	*width = i.width;
	*height = i.height;
}
