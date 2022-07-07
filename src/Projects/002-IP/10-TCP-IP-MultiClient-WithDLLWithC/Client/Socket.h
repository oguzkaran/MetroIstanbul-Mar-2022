/*----------------------------------------------------------------------
FILE        : Socket.h
AUTHOR      : Oguz Karan
LAST UPDATE : 15/01/2018
PLATFORM    : Windows

General Socket Library header file

Copyleft (c) 1993 by C and System Programmers Association (CSD)
All Rights Free
-----------------------------------------------------------------------*/
#ifndef SOCKET_H_
#define SOCKET_H_

#ifdef DLLEXPORT
#define DLLSPEC		__declspec(dllexport)
#else 
#define DLLSPEC		__declspec(dllimport)
#endif

#include <WinSock2.h>

typedef struct tagPARAM {
	SOCKET socket;
	void *param;
} PARAM, *PPARAM;

typedef DWORD(*PCLIENT_ROUTINE)(PPARAM param);

#ifdef __cplusplus
extern "C" {
	DLLSPEC int StartTCPConcurrentServer(SOCKET socket, PCLIENT_ROUTINE lpStartAddress, void *param);
	DLLSPEC int ConnectToServer(const char *hostName, USHORT destPort, SOCKET *pSocket);
	DLLSPEC void CloseSocket(SOCKET socket, int how);
	DLLSPEC int ReadSocket(SOCKET sock, void *buf, int count);
	DLLSPEC int WriteSocket(SOCKET sock, const void *buf, int count);
}
#else
DLLSPEC int StartTCPConcurrentServer(SOCKET socket, PCLIENT_ROUTINE lpStartAddress, void *param);
DLLSPEC int ConnectToServer(const char *hostName, USHORT destPort, SOCKET *pSocket);
DLLSPEC void CloseSocket(SOCKET socket, int how);
DLLSPEC int ReadSocket(SOCKET sock, void *buf, int count);
DLLSPEC int WriteSocket(SOCKET sock, const void *buf, int count);
#endif


#endif /* SOCKET_H_ */
