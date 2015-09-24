#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include "udp_client.h"


int main (int argc, char* argv[])
{
    char buf_in[128];
    int port;
    if(argc!=3){
        printf("USAGE: client [server_ip] [server_port]\n");
        exit(1);
    }else{
        sscanf (argv[2],"%d",&port);
        if(port < 0 && port > 49152){
            printf("Wrong port number!\n");
            exit(1);
        }
        printf("Server: %s\n", argv[1]);
        printf("Port: %i", port);
    }
    
    int sock = open_client_sock(argv[1], port);
    
    while (1)
    {
        sleep(2);
        char buf[]  = "\x65""Ala ma kota";
        write(sock, buf, sizeof(buf));
        printf("Sent: \"%s\"\n", buf);  
        int length = recv(sock, buf_in, 100, MSG_DONTWAIT);
        if(length==-1) {
            //TODO: probably errno is set in here, needs to be cleared
            //printf("\n");
        } else {
            buf_in[length] = '\0';
            printf("Received %d bytes:\n", length);
            char message_id = buf_in[0];
            printf("message_id: %d\n", (int)message_id);
            printf("data: %s\n", buf_in + 1);
        }
    }
    
    return 0;
}
