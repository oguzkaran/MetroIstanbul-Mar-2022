/* Client.c */

#pragma comment(lib,"WS2_32")

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <tchar.h>
#include <winsock2.h>

#define DEST_PORT			5050
#define HOSTNAME			"127.0.0.1"

void ExitSys(LPCSTR lpszMsg, DWORD dwLastError);

int main(void)
{	
	SOCKET clientSock;
	char buf[4096];
	int msgLen;
	int result;
		
	if ((result = ConnectToServer(HOSTNAME, DEST_PORT, &clientSock)) == SOCKET_ERROR)
		ExitSys("ConnectToServer", WSAGetLastError());

	printf("connected...\n");
	for (;;) {
		printf("Text:");
		gets(buf);

		msgLen = strlen(buf) + 1;
		if (WriteSocket(clientSock, &msgLen, sizeof(msgLen)) != sizeof(msgLen))
			ExitSys("WriteSocket", WSAGetLastError());

		if (WriteSocket(clientSock, buf, msgLen) != msgLen)
			ExitSys("WriteSocket", WSAGetLastError());		
		
		if (!strcmp(buf, "quit"))
			break;

		if ((result = ReadSocket(clientSock, &msgLen, sizeof(msgLen))) != sizeof(msgLen))
			ExitSys("ReadSocket", WSAGetLastError());

		if ((result = ReadSocket(clientSock, buf, msgLen)) != msgLen)
			ExitSys("ReadSocket", WSAGetLastError());

		if (result == 0)
			break;

		puts(buf);
	}

	CloseSocket(clientSock, SD_BOTH);

	return 0;
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