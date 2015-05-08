#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void delete_image(int handle)
{
	if (image_store::exists(handle))
	{
		image i = image_store::get(handle);
		delete[] i.data;
		image_store::erase(handle);
	}
}
