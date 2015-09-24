#ifndef UDP_SERVER_H
#define UDP_SERVER_H

#include <arpa/inet.h>
#include <linux/wireless.h>

#include "message_handler.h"

#define MICROINSECOND 1000000

class UdpServer
{
private:
    const int buffer_size = 1024;
    const int time_out = 5000000;

    int connected = 0;
    int port      = 0;
    bool is_wifi  = false;

    int server_sockfd;
    int client_sockfd;

    socklen_t size;

    struct sockaddr_in server_address;
    struct sockaddr_in client_address;

    struct iwreq iwr;
    struct ifreq ifr;
    struct iw_statistics iwstats;

    char * in_buffer;
    char * out_buffer;

    MessageHandler * message_handler;

    void check_wifi_quality(bool log, int &level, int &noise, int &qual);
    void stop();
    void start();
public:
    UdpServer(int in_port, MessageHandler * in_message_handler);
    ~UdpServer();
    void run(int frequency);
};


#endif
