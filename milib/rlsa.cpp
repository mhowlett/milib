#include "stdafx.h"
#include "milib.h"
#include "image_store.h"

void apply_rlsa_right(image im, int c)
{
	int off_edge = 0;
	unsigned char* row = new unsigned char[im.width];
	for (int j=0; j<im.height; ++j)
	{
		int cc = c + 1;
		for (int i=0; i<im.width; ++i)
		{
			if (im.data[j*im.width + i] == 0)
			{
				if (off_edge == 1)
				{
					row[i] = 0;
					cc = 0;
				}
				else 
				{
					cc = c + 1;
					row[i] = 255;
				}
			}
			else
			{
				off_edge = 1;
				cc += 1;
				if (cc < c)
				{
					row[i] = 0;
				}
				else 
				{
					row[i] = 255;
				}
			}
		}

		for (int i=0; i<im.width; ++i)
		{
			im.data[j*im.width + i] = row[i];
		}
	}
	delete[] row;
}

void apply_rlsa_left(image im, int c)
{
	int off_edge = 0;
	unsigned char* row = new unsigned char[im.width];
	for (int j=0; j<im.height; ++j)
	{
		int cc = c + 1;
		for (int i=im.width-1; i>=0; --i)
		{
			if (im.data[j*im.width + i] == 0)
			{
				if (off_edge)
				{
					row[i] = 0;
					cc = 0;
				}
				else
				{
					cc = c + 1;
					row[i] = 255;
				}
			}
			else
			{
				off_edge = 1;
				cc += 1;
				if (cc < c)
				{
					row[i] = 0;
				}
				else 
				{
					row[i] = 255;
				}
			}
		}

		for (int i=0; i<im.width; ++i)
		{
			im.data[j*im.width + i] = row[i];
		}
	}
	delete[] row;
}

void apply_rlsa_down(image im, int c)
{
	int off_edge = 0;
	unsigned char* column = new unsigned char[im.height];
	for (int i=0; i<im.width; ++i)
	{
		int cc = c + 1;
		for (int j=0; j<im.height; ++j)
		{
			if (im.data[j*im.width + i] == 0)
			{
				if (off_edge)
				{
					column[j] = 0;
					cc = 0;
				}
				else
				{
					cc = c + 1;
					column[i] = 255;
				}
			}
			else
			{
				off_edge = true;
				cc += 1;
				if (cc < c)
				{
					column[j] = 0;
				}
				else 
				{
					column[j] = 255;
				}
			}
		}

		for (int j=0; j<im.height; ++j)
		{
			im.data[j*im.width + i] = column[j];
		}
	}
	delete[] column;
}

void apply_rlsa_up(image im, int c)
{
	int off_edge = 0;
	unsigned char* column = new unsigned char[im.height];
	for (int i=0; i<im.width; ++i)
	{
		int cc = c + 1;
		for (int j=im.height-1; j>=0; --j)
		{
			if (im.data[j*im.width + i] == 0)
			{
				if (off_edge)
				{
					column[j] = 0;
					cc = 0;
				}
				else
				{
					cc = c + 1;
					column[i] = 255;
				}
			}
			else
			{
				off_edge = 1;
				cc += 1;
				if (cc < c)
				{
					column[j] = 0;
				}
				else 
				{
					column[j] = 255;
				}
			}
		}

		for (int j=0; j<im.height; ++j)
		{
			im.data[j*im.width + i] = column[j];
		}
	}
	delete[] column;
}

void rlsa(int handle, int c, int dir)
{
	image im = image_store::get(handle);

	if (dir == 0)
	{
		apply_rlsa_right(im, c);
	}
	else if (dir == 1)
	{
		apply_rlsa_left(im, c);
	}
	else if (dir == 2)
	{
		apply_rlsa_down(im, c);
	}
	else if (dir == 3)
	{
		apply_rlsa_up(im, c);
	}
}
