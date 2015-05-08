#include <string>
#include "image.h"
#include "series.h"

extern "C" __declspec(dllexport) void blur_simple(int handle);

extern "C" __declspec(dllexport) void set_image(int* handle, unsigned char *bytes, int width, int height);
extern "C" __declspec(dllexport) void get_image(int handle, unsigned char** bytes, int* width, int* height);

extern "C" __declspec(dllexport) void get_series(int handle, float** bytes, int* length);

extern "C" __declspec(dllexport) void delete_image(int handle);
extern "C" __declspec(dllexport) void copy(int* new_handle, int handle_to_copy);
extern "C" __declspec(dllexport) void delete_series(int handle);

extern "C" __declspec(dllexport) void threshold(int handle, unsigned char threshold);
extern "C" __declspec(dllexport) void add_uniform_noise(int handle, unsigned char maxChange);
extern "C" __declspec(dllexport) void draw_rectangle(int handle, int x, int y, int width, int height, unsigned char intensity);

extern "C" __declspec(dllexport) void cross_modify(int handle, int size, int type);
extern "C" __declspec(dllexport) void crop(int handle, int x, int y, int width, int height);
extern "C" __declspec(dllexport) void size_halve(int handle, int type);
extern "C" __declspec(dllexport) void rlsa(int handle, int c, int dir);
extern "C" __declspec(dllexport) void pre_thin(int handle);
extern "C" __declspec(dllexport) void logical_and(int to_modify_handle, int mask_handle);
extern "C" __declspec(dllexport) void transpose(int handle);
extern "C" __declspec(dllexport) void shear(int handle, int dir, float lambda);
extern "C" __declspec(dllexport) void overlay(int to_modify_handle, int overlay_handle, int x, int y);

extern "C" __declspec(dllexport) void histogram(int imageHandle, int* seriesHandle);
extern "C" __declspec(dllexport) void average_column_intensity(int imageHandle, int* seriesHandle);
extern "C" __declspec(dllexport) void average_column_gradient(int imageHandle, int* seriesHandle);
extern "C" __declspec(dllexport) void average_row_intensity(int imageHandle, int* seriesHandle);
extern "C" __declspec(dllexport) void generate_bounding_boxes(int imageHandle, int* dataHandle);
extern "C" __declspec(dllexport) void cardscan_determine_card_bounds(int handle, int threshold, int* min_x, int* max_x, int* min_y, int* max_y);

extern "C" __declspec(dllexport) void size_double(int handle, int type);
extern "C" __declspec(dllexport) void hough_transform(int handle, int* outHandle, float minAngle, float maxAngle, int numberAngles);
extern "C" __declspec(dllexport) void hough_gradient(int handle, int* outSeriesHandle, float* angleOfMaxValue, float minAngle, float maxAngle, int numberAngles);
extern "C" __declspec(dllexport) void invert(int handle);

extern "C" __declspec(dllexport) void cardscan_rectify_raw_image_high_gradient_error(int imageHandle);

extern "C" __declspec(dllexport) void create(int* new_handle, int width, int height, unsigned char intensity);

// GetAllAllocatedImages.
