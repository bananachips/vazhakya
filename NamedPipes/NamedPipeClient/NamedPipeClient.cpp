// NamedPipeClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h> 
#include <conio.h>

#include <string>
#include "ImageClient.h"
using namespace std;


void fillData(unsigned char* pixelData, int width, int height, int channels, unsigned char red, unsigned char green, unsigned char blue)
{
	for (int y = 0; y < height; ++y)
	{
		int yIndex = y * width * 3;
		for (int x = 0; x < width; ++x)
		{
			int xIndex = x * 3;
			pixelData[xIndex++ + yIndex] = red;
			pixelData[xIndex++ + yIndex] = green;
			pixelData[xIndex++ + yIndex] = blue;
		}
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	int  width = 200;
	int height = 200;
	int channels = 3;

	unsigned char red = 255;
	unsigned char green = 0;
	unsigned char blue = 0;
	unsigned char* pixelData = new unsigned char[width * height * channels];
	fillData(pixelData, width, height, channels, red, green, blue);
	DisplayImage(width, height, channels, 0, (byte*)pixelData);
	::Sleep(1000);

	red = 0; green = 255; blue = 0;
	fillData(pixelData, width, height, channels, red, green, blue);
	DisplayImage(width, height, channels, 0, (byte*)pixelData);
	::Sleep(1000);

	red = 0; green = 0; blue = 255;
	fillData(pixelData, width, height, channels, red, green, blue);
	DisplayImage(width, height, channels, 0, (byte*)pixelData);
	::Sleep(1000);

	red = 255; green = 255; blue = 0;
	fillData(pixelData, width, height, channels, red, green, blue);
	DisplayImage(width, height, channels, 0, (byte*)pixelData);
	::Sleep(1000);

	red = 0; green = 255; blue = 255;
	fillData(pixelData, width, height, channels, red, green, blue);
	DisplayImage(width, height, channels, 0, (byte*)pixelData);
	::Sleep(1000);

}
int xxxx()
{
	DisplayImage(0, 0, 0, 0, 0);
	/*LPTSTR appName = TEXT("c:\\ank\\NamedPipes\\NamedPipeImageServer\\bin\\Debug\\NamedPipeImageServer.exe");
	startup(appName);
*/
	::Sleep(5000);
	LPTSTR lpszPipename = TEXT("\\\\.\\pipe\\ANKsImageViewer");
	
	HANDLE hPipe = CreateFile(lpszPipename,
		GENERIC_WRITE,
		0,              // no sharing 
		NULL,           // default security attributes
		OPEN_EXISTING,  // opens existing pipe 
		0,              // default attributes 
		NULL);          // no template file )
	
	DWORD dwMode = PIPE_READMODE_MESSAGE;
	SetNamedPipeHandleState(
		hPipe,    // pipe handle 
		&dwMode,  // new pipe mode 
		NULL,     // don't set maximum bytes 
		NULL);    // don't set maximum time 

	int  width = 200;
	int height = 200;
	int channels = 3;
	
	DWORD cbWritten;
	BOOL fSuccess = WriteFile(
		hPipe,                  // pipe handle 
		&width,             // message 
		sizeof(width),              // message length 
		&cbWritten,             // bytes written 
		NULL);                  // not overlapped 
	fSuccess = WriteFile(
		hPipe,                  // pipe handle 
		&height,             // message 
		sizeof(height),              // message length 
		&cbWritten,             // bytes written 
		NULL);                  // not overlapped 
	fSuccess = WriteFile(
		hPipe,                  // pipe handle 
		&channels,             // message 
		sizeof(channels),              // message length 
		&cbWritten,             // bytes written 
		NULL);                  // not overlapped 

	unsigned char* pixelData = new unsigned char[width * height * channels];
	for (int y = 0; y < height; ++y)
	{
		int yIndex = y * width * 3;
		for (int x = 0; x < width; ++x)
		{
			int xIndex = x * 3;
			pixelData[xIndex++ + yIndex] = 255;
			pixelData[xIndex++ + yIndex] = 0;
			pixelData[xIndex++ + yIndex] = 0;
		}
	}

	fSuccess = WriteFile(
		hPipe,                  // pipe handle 
		pixelData,             // message 
		width * height * channels,              // message length 
		&cbWritten,             // bytes written 
		NULL);                  // not overlapped 
	//x = 5;
	//fSuccess = WriteFile(
	//	hPipe,                  // pipe handle 
	//	&x,             // message 
	//	1,              // message length 
	//	&cbWritten,             // bytes written 
	//	NULL);                  // not overlapped 
	//
	//fSuccess = WriteFile(
	//	hPipe,                  // pipe handle 
	//	&x,             // message 
	//	1,              // message length 
	//	&cbWritten,             // bytes written 
	//	NULL);                  // not overlapped 
	getch();
	return 0;
}

