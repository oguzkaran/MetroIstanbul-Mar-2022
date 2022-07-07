/*----------------------------------------------------------------------
FILE        : Socket.c
AUTHOR      : Oguz Karan
LAST UPDATE : 15/01/2018
PLATFORM    : Windows

General Socket Library implementation

Copyleft (c) 1993 by C and System Programmers Association (CSD)
All Rights Free
-----------------------------------------------------------------------*/

#pragma comment(lib,"WS2_32")

#define DLLEXPORT

#include <stdio.h>
#include <stdlib.h>
#include "Socket.h"

typedef struct tagSOCKETPARAM {
	PARAM param;
	PCLIENT_ROUTINE lpStartAddress;
} SOCKETPARAM;

static DWORD __stdcall clientProc(void *param)
{
	SOCKETPARAM *pSp = (SOCKETPARAM *)param;
	DWORD result;	
	
	result = pSp->lpStartAddress(&pSp->param);	
	shutdown(pSp->param.socket, SD_BOTH);
	closesocket(pSp->param.socket);
	free(param);
	CloseHandle(GetCurrentThread());

	return result;
}

DLLSPEC int InitTCPServer(USHORT portNo, int backLog, ULONG ipAddr, SOCKET *pSocket)
{
	WSADATA wsaData;
	SOCKET listenSock;
	struct sockaddr_in sinServer;
	int result;

	if ((result = WSAStartup(MAKEWORD(2, 2), &wsaData)) != 0)
		goto EXIT;

	if ((listenSock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) == INVALID_SOCKET) {
		result = SOCKET_ERROR;
		goto EXIT;
	}

	sinServer.sin_family = AF_INET;
	sinServer.sin_addr.s_addr = htonl(ipAddr);
	sinServer.sin_port = htons(portNo);

	if ((result = bind(listenSock, (struct sockaddr *)&sinServer, sizeof(sinServer))) == SOCKET_ERROR) {
		closesocket(listenSock);
		goto EXIT;
	}

	if ((result = listen(listenSock, backLog)) == SOCKET_ERROR) {
		closesocket(listenSock);
		goto EXIT;
	}

	*pSocket = listenSock;
EXIT:
	return result;
}

DLLSPEC int StartTCPConcurrentServer(SOCKET socket, PCLIENT_ROUTINE lpStartAddress, void *param)
{
	struct sockaddr_in sinClient;
	SOCKET clientSock;
	int addrLen;
	int result;
	HANDLE hThread;
	DWORD dwThreadId;
	SOCKETPARAM *pparam;	

	result = 0;
	for (;;) {
		printf("waiting for connection...\n");
		addrLen = sizeof(sinClient);
		if ((clientSock = accept(socket, (struct sockaddr *)&sinClient, &addrLen)) == INVALID_SOCKET) {
			result = SOCKET_ERROR;
			goto EXIT;
		}
		if ((pparam = (SOCKETPARAM *)malloc(sizeof(SOCKETPARAM))) == NULL) {
			result = SOCKET_ERROR;
			goto EXIT;
		}

		printf("Client connected: %s:%d\n", inet_ntoa(sinClient.sin_addr), ntohs(sinClient.sin_port));

		pparam->param.socket = clientSock;
		pparam->param.param = param;
		pparam->lpStartAddress = lpStartAddress;
		if ((hThread = CreateThread(NULL, 0, clientProc, (void *)pparam, 0, &dwThreadId)) == NULL) {
			result = SOCKET_ERROR;
			goto EXIT;
		}
	}
EXIT:
	closesocket(socket);
	WSACleanup();

	return result;
}

DLLSPEC int ConnectToServer(const char *hostName, USHORT destPort, SOCKET *pSocket)
{
	WSADATA wsaData;
	int result;
	struct sockaddr_in sinServer;
	struct hostent *host;

	if ((result = WSAStartup(MAKEWORD(2, 2), &wsaData)) != 0) {
		result = SOCKET_ERROR;		
		goto EXIT;
	}

	if ((*pSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) == INVALID_SOCKET) {
		result = SOCKET_ERROR;
		goto EXIT;
	}

	sinServer.sin_family = AF_INET;
	sinServer.sin_port = htons(destPort);
	sinServer.sin_addr.s_addr = inet_addr(hostName);
	if (sinServer.sin_addr.s_addr == INADDR_NONE) {
		if ((host = gethostbyname(hostName)) == NULL) {
			result = SOCKET_ERROR;
			goto EXIT;
		}
		memcpy(&sinServer.sin_addr.s_addr, host->h_addr_list[0], host->h_length);
	}	
	if ((result = connect(*pSocket, (struct sockaddr *) &sinServer, sizeof(sinServer))) == SOCKET_ERROR)
		closesocket(*pSocket);
EXIT:
	return result;
}

DLLSPEC void CloseSocket(SOCKET socket, int how)
{
	shutdown(socket, how);
	closesocket(socket);
	WSACleanup();
}

DLLSPEC int ReadSocket(SOCKET sock, void *buf, int count)
{
	int result;
	int left = count, index = 0;

	while (left > 0) {
		if ((result = recv(sock, (char *)buf + index, left, 0)) == SOCKET_ERROR)
			return SOCKET_ERROR;
		if (result == 0)
			break;
		index += result;
		left -= result;
	}

	return index;
}

DLLSPEC int WriteSocket(SOCKET sock, const void *buf, int count)
{
	int result;
	int left = count, index = 0;

	while (left > 0) {
		if ((result = send(sock, (char *)buf + index, left, 0)) == SOCKET_ERROR)
			return SOCKET_ERROR;
		if (result == 0)
			break;
		index += result;
		left -= result;
	}

	return index;
}
