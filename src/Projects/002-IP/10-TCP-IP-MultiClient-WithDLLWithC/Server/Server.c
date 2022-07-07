/* Server.c */

#pragma comment(lib,"WS2_32")

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <winsock2.h>
#include "Socket.h"

#define PORTNO			5050

DWORD ClientProc(PPARAM param);
void DisplayMessage(LPCSTR lpszMsg, DWORD dwLastError);
void ExitSys(LPCSTR lpszMsg, DWORD dwLastError);

int main(void)
{
	SOCKET listenSock;
	int result;
	
	if ((result = InitTCPServer(PORTNO, 512, INADDR_ANY, &listenSock)) == SOCKET_ERROR)
		ExitSys("InitServer", result);
	
	if ((result = StartTCPConcurrentServer(listenSock, ClientProc, NULL)) == NULL)
		ExitSys("StartConcurrentServer", result);

	return 0;
}

DWORD ClientProc(PPARAM param)
{
	int result;
	char buf[4096];
	SOCKET clientSock = param->socket;
	int msgLen;	

	for (;;) {
		if ((result = ReadSocket(clientSock, &msgLen, sizeof(msgLen))) != sizeof(msgLen)) {
			DisplayMessage("ReadSocket", WSAGetLastError());
			goto EXIT;
		}

		if ((result = ReadSocket(clientSock, buf, msgLen)) != msgLen) {
			DisplayMessage("ReadSocket", WSAGetLastError());
			goto EXIT;
		}

		if (result == 0)
			break;

		buf[result] = '\0';
		if (!strcmp(buf, "quit"))
			break;

		puts(buf);
		strrev(buf);
		
		msgLen = strlen(buf) + 1;
		if (WriteSocket(clientSock, &msgLen, sizeof(msgLen)) != sizeof(msgLen)) {
			DisplayMessage("WriteSocket", WSAGetLastError());
			goto EXIT;
		}

		if (WriteSocket(clientSock, buf, msgLen) != msgLen) {
			DisplayMessage("WriteSocket", WSAGetLastError());
			goto EXIT;
		}
	}
EXIT:
	return 0;
}

void DisplayMessage(LPCSTR lpszMsg, DWORD dwLastError)
{
	LPTSTR lpszErr;

	if (FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM, NULL, dwLastError,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpszErr, 0, NULL)) {
		fprintf(stderr, "%s: %s\n", lpszMsg, lpszErr);
		LocalFree(lpszErr);
	}
}
void ExitSys(LPCSTR lpszMsg, DWORD dwLastError)
{
	LPTSTR lpszErr;

	if (FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM, NULL, dwLastError,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPTSTR)&lpszErr, 0, NULL)) {
		fprintf(stderr, "%s: %s\n", lpszMsg, lpszErr);
		LocalFree(lpszErr);
	}

	exit(EXIT_FAILURE);
}


