#include <fcntl.h>
#include <arpa/inet.h>
#include <stdio.h>

int open_client_sock(char* ip, int port){
	//const int nonBlocking = 1;
	char  hello[] = "\x01LunaDaemon";
	socklen_t size;
	int sock = socket (PF_INET, SOCK_DGRAM, 0);
	struct sockaddr_in addr, srv_addr;
	addr.sin_family = PF_INET;
	addr.sin_port = htons (port);
	addr.sin_addr.s_addr = inet_addr (ip);
	
	/*if ( fcntl( sock, F_SETFL, O_NONBLOCK, nonBlocking ) == -1 )
	{
		printf( "failed to set non-blocking socket\n" );
		return -1;
	}*/
	printf("Connecting to server\n");
	sendto (sock, hello, sizeof(hello), 0, (struct sockaddr *) &addr, sizeof (struct sockaddr));
	size = sizeof (struct sockaddr);
    printf("Waiting for response\n");
    recvfrom (sock, hello, 1, 0, (struct sockaddr *) &srv_addr, &size);
    printf("Server responded\n");
	connect (sock, (struct sockaddr *) &srv_addr, size);
	return sock;
}


