#pragma once
#include <windows.h> 
#include <conio.h>

static bool isServerRunning = false;
static HANDLE clientPipeHandle = INVALID_HANDLE_VALUE;
VOID startup()
{
	LPTSTR appName = TEXT("d:\\Arcgis\\bin\\NamedPipeImageServer.exe");
	// additional information
	STARTUPINFO si;
	PROCESS_INFORMATION pi;

	// set the size of the structures
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));

	// start the program up
	CreateProcess(appName,   // the path
		NULL,        // Command line
		NULL,           // Process handle not inheritable
		NULL,           // Thread handle not inheritable
		FALSE,          // Set handle inheritance to FALSE
		0,              // No creation flags
		NULL,           // Use parent's environment block
		NULL,           // Use parent's starting directory 
		&si,            // Pointer to STARTUPINFO structure
		&pi             // Pointer to PROCESS_INFORMATION structure (removed extra parentheses)
		);
	// Close process and thread handles. 
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);
	isServerRunning = true;
	::Sleep(1000);
}


int DisplayImage(int width, int height, int channels, int format, byte* pixelData)
{
	if (!isServerRunning)
		startup();

	LPTSTR lpszPipename = TEXT("\\\\.\\pipe\\ANKsImageViewer");
	
	while (clientPipeHandle == INVALID_HANDLE_VALUE)
	{
		clientPipeHandle = CreateFile(
			lpszPipename,   // pipe name 
			GENERIC_WRITE,
			0,              // no sharing 
			NULL,           // default security attributes
			OPEN_EXISTING,  // opens existing pipe 
			0,              // default attributes 
			NULL);          // no template file 

		// Break if the pipe handle is valid. 

		if (clientPipeHandle != INVALID_HANDLE_VALUE)
			break;

		// Exit if an error other than ERROR_PIPE_BUSY occurs. 

		if (GetLastError() != ERROR_PIPE_BUSY)
		{
			_tprintf(TEXT("Could not open pipe. GLE=%d\n"), GetLastError());
			return -1;
		}

		// All pipe instances are busy, so wait for 20 seconds. 

		/*if (!WaitNamedPipe(lpszPipename, 20000))
		{
			printf("Could not open pipe: 20 second wait timed out.");
			return -1;
		}*/
	}
	DWORD cbWritten;
	BOOL fSuccess = WriteFile(
		clientPipeHandle,                  // pipe handle 
		&width,             // message 
		sizeof(width),              // message length 
		&cbWritten,             // bytes written 
		NULL);                  // not overlapped 

  if(!fSuccess)
  {
    clientPipeHandle = INVALID_HANDLE_VALUE;
    CloseHandle(clientPipeHandle);
    return -1;
  }

  if (!WriteFile(clientPipeHandle, &height, sizeof(height), &cbWritten, NULL))
  {
    clientPipeHandle = INVALID_HANDLE_VALUE;
    CloseHandle(clientPipeHandle);
    return -1;
  }

  if (!WriteFile(clientPipeHandle, &channels, sizeof(channels), &cbWritten, NULL))
  {
    clientPipeHandle = INVALID_HANDLE_VALUE;
    CloseHandle(clientPipeHandle);
    return -1;
  }
	if (!WriteFile(clientPipeHandle, pixelData, width * height * channels, &cbWritten, NULL))
  {
    clientPipeHandle = INVALID_HANDLE_VALUE;
    CloseHandle(clientPipeHandle);
    return -1;
  }
  CloseHandle(clientPipeHandle);
	return 1;
}

void DisplayPixelBlock(IPixelBlock* pPixelBlock)
{
  long width, height, channels;
  pPixelBlock->get_Height(&height);
  pPixelBlock->get_Width(&width);
  pPixelBlock->get_Planes(&channels);
  int size = height * width * channels;
  byte* pixelData = new byte[size];
  IPixelBlock3Ptr ipPixelBlock(pPixelBlock);
  VARIANT var;
  ipPixelBlock->get_PixelDataByRef(0, &var);
  byte* red = (BYTE*)(*var.pparray)->pvData;
  ipPixelBlock->get_PixelDataByRef(1, &var);
  byte* green = (BYTE*)(*var.pparray)->pvData;
  ipPixelBlock->get_PixelDataByRef(2, &var);
  byte* blue = (BYTE*)(*var.pparray)->pvData;

  for (int y = 0; y < height; ++y)
  {
    int yIndex = y * width * 3;
    for (int x = 0; x < width; ++x)
    {
      int xIndex = x * 3;
      pixelData[xIndex++ + yIndex] = red[y*width + x];
      pixelData[xIndex++ + yIndex] = green[y*width + x];
      pixelData[xIndex++ + yIndex] = blue[y*width + x];
    }
  }



  DisplayImage(width, height, channels, 0, pixelData);
  delete[]pixelData;
}