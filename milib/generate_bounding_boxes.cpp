#include "stdafx.h"
#include "milib.h"
#include "image_store.h"
#include "series_store.h"
#include <stack>
#include <vector>
#include <utility>

void determine_box(image im, unsigned char* used, int start_x, int start_y, int* minx, int* miny, int* maxx, int* maxy)
{
	std::stack<std::pair<int, int> > points;
	points.push( std::pair<int,int>(start_x, start_y) );

	int min_x = start_x;
	int max_x = start_x;
	int min_y = start_y;
	int max_y = start_y;

	while (!points.empty())
	{
		std::pair<int, int> current = points.top();
		points.pop();

		// extend bounding box to include if necessary.
		if (current.first < min_x)
		{
			min_x = current.first;
		}
		if (current.first > max_x)
		{
			max_x = current.first;
		}
		if (current.second < min_y)
		{
			min_y = current.second;
		}
		if (current.second > max_y)
		{
			max_y = current.second;
		}


		if (current.first > 0)
		{
			if (used[im.width*current.second + current.first-1] == 0)
			{
				used[im.width*current.second + current.first-1] = 1;
				if (im.data[im.width*current.second + current.first-1] == 0)
				{
					points.push(std::pair<int, int>(current.first-1, current.second));
				}
			}
			else
			{
				used[im.width*current.second + current.first-1] = 1;
			}
		}

		if (current.first < im.width-1)
		{
			if (used[im.width*current.second + current.first+1] == 0)
			{
				used[im.width*current.second + current.first+1] = 1;
				if (im.data[im.width*current.second + current.first+1] == 0)
				{
					points.push(std::pair<int, int>(current.first+1, current.second));
				}
			}
			else
			{
				used[im.width*current.second + current.first+1] = 1;
			}
		}

		if (current.second > 0)
		{
			if (used[im.width*(current.second-1) + current.first] == 0)
			{
				used[im.width*(current.second-1) + current.first] = 1;
				if (im.data[im.width*(current.second-1) + current.first] == 0)
				{
					points.push(std::pair<int, int>(current.first, current.second-1));
				}
			}
			else
			{
				used[im.width*(current.second-1) + current.first] = 1;
			}
		}

		if (current.second < im.height-1)
		{
			if (used[im.width*(current.second+1) + current.first] == 0)
			{
				used[im.width*(current.second+1) + current.first] = 1;
				if (im.data[im.width*(current.second+1) + current.first] == 0)
				{
					points.push(std::pair<int, int>(current.first, current.second+1));
				}
			}
			else
			{
				used[im.width*(current.second+1) + current.first] = 1;
			}
		}
	}

	*minx = min_x;
	*miny = min_y;
	*maxx = max_x;
	*maxy = max_y;
}


void generate_bounding_boxes(int imageHandle, int* dataHandle)
{
	image im = image_store::get(imageHandle);
	
	unsigned char* used = new unsigned char[im.width*im.height];

	for (int i=0; i<im.width*im.height; ++i)
	{
		used[i] = 0;
	}

	std::vector<int> result;
	for (int i=0; i<im.width; ++i)
	{
		for (int j=0; j<im.height; ++j)
		{
			if (used[im.width*j + i] == 0)
			{
				if (im.data[im.width*j + i] == 255)
				{
					used[im.width*j + i] = 1;
				}
				else
				{
					used[im.width*j + i] = 1;
					int minx, miny, maxx, maxy;
					determine_box(im, used, i, j, &minx, &miny, &maxx, &maxy);
					result.push_back(minx);
					result.push_back(miny);
					result.push_back(maxx);
					result.push_back(maxy);
				}
			}
		}
	}

	delete[] used;

	float* lastResultMemory = new float[result.size()];
	for (int i=0; i<result.size(); ++i)
	{
		lastResultMemory[i] = (float)result[i];
	}

	series se;
	se.data = lastResultMemory;
	se.length = result.size();

	*dataHandle = series_store::set(se);
}
