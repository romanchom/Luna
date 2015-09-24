#include <fcntl.h>

#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <memory.h>
#include <unistd.h>
#include <sys/time.h>
#include <system_error>
#include <sys/ioctl.h>
#include "udp_server.h"
#include "log.h"
#include "messages.h"

extern bool halt;

UdpServer::UdpServer(int in_port, MessageHandler *in_message_handler)
{
    message_handler = in_message_handler;
    port = in_port;

    in_buffer  = (char *)malloc(buffer_size+1);
    out_buffer = (char *)malloc(buffer_size+1);

    memset(&iwr, 0, sizeof(iwr) );
    strcpy(iwr.ifr_name, "wlan0");

    memset(&ifr, 0, sizeof(ifr) );
    strcpy(ifr.ifr_name, "wlan0" );

    iwr.u.data.pointer = &iwstats;
    iwr.u.data.length = sizeof(struct iw_statistics);
    iwr.u.data.flags = 1;

    start();
}

void UdpServer::start()
{
    ILOG("Setting new sockets on port: " << port);

    size = sizeof (struct sockaddr);

    server_sockfd = socket (PF_INET, SOCK_DGRAM, 0);
    client_sockfd = socket (PF_INET, SOCK_DGRAM, 0);

    memset (&server_address, 0, sizeof (server_address));
    memset (&client_address, 0, sizeof (client_address));
    client_address.sin_family = PF_INET;
    server_address.sin_family = PF_INET;
    server_address.sin_port = htons (port);
    server_address.sin_addr.s_addr = htonl (INADDR_ANY);

    if (bind (server_sockfd, (struct sockaddr *) &server_address, sizeof (struct sockaddr_in))) {
        ERNOLOG ("Bind");
        throw std::system_error(errno, std::system_category());
    }

    int level;
    int noise;
    int qual;
    check_wifi_quality(true, level, noise, qual);
}

UdpServer::~UdpServer()
{
    stop();
    free(in_buffer);
    free(out_buffer);
}

void UdpServer::check_wifi_quality(bool log, int &level, int &noise, int &qual)
{
    if( ioctl( server_sockfd, SIOCGIFFLAGS, &ifr ) != -1 )
    {
        if((ifr.ifr_flags & ( IFF_UP | IFF_RUNNING )) == ( IFF_UP | IFF_RUNNING )) {
            is_wifi = true;
            if(log) {
                ILOG("WiFi is up and running.");
            }

            if(ioctl(server_sockfd, SIOCGIWSTATS, &iwr) != -1) {
                // if(((iw_statistics *)iwr.u.data.pointer)->qual.updated & IW_QUAL_RCPI){
                if(log) {
                ILOG("WiFi quality:" << std::endl
                     << "level:   " << (int)(((iw_statistics *)iwr.u.data.pointer)->qual.level) << std::endl
                     << "noise:   " << (int)(((iw_statistics *)iwr.u.data.pointer)->qual.noise) << std::endl
                     << "qual:    " << (int)(((iw_statistics *)iwr.u.data.pointer)->qual.qual));
                    //("updated: 0x%X\n",((iw_statistics *)iwr.u.data.pointer)->qual.updated);
                } else {
                    level = (int)(((iw_statistics *)iwr.u.data.pointer)->qual.level);
                    noise = (int)(((iw_statistics *)iwr.u.data.pointer)->qual.noise);
                    qual  = (int)(((iw_statistics *)iwr.u.data.pointer)->qual.qual);
                }
            } else {
                ERNOLOG("ioctl error");
                throw std::system_error(errno, std::system_category());
            }
        } else {
            if(!log) {
                ELOG("WiFi is NOT up and running. Stopping sending and receiving messages");
                connected = false;
            }
        }
    }
    else
    {
        is_wifi = false;
        if(log) {
            ILOG("Connection is not Wireless. Skipping quality checks.");
        }
        //ERNOLOG("ioctl error");
        //throw std::system_error(errno, std::system_category());
    }
}

void UdpServer::stop()
{
    ILOG ("Closing socket");
    close(server_sockfd);
}

void UdpServer::run(int frequency)
{
    ILOG("Server started");

    int loop_duration = MICROINSECOND / frequency;
    int time_difference;
    int time_to_sleep;
    
    int last_in_message = 0;

    struct timeval start_time;
    struct timeval gettime_now;
    while (true) {
        gettimeofday(&start_time, NULL);
        if(connected) {

            last_in_message += MICROINSECOND / frequency;
            int in_message_len = -1;

            while((in_message_len = recv(client_sockfd, in_buffer, buffer_size, MSG_DONTWAIT)) != -1) {      
		unsigned char message_id = message_handler->process_input_message(in_message_len, in_buffer);
                memset(in_buffer, 0, buffer_size);
                last_in_message = 0;
                if (message_id == MSG_IN_GOODBYE) {
                    connected = false;
                }
            }

            if(last_in_message > time_out) {
                ILOG("No incoming communication since " << last_in_message / MICROINSECOND << "s. Restarting server");
                stop();
                start();
                connected = false;
                message_handler->connected = false;
            }
        }

        message_handler->work();

        if(connected){
            int out_message_len;

            if(is_wifi) {
                int level;
                int noise;
                int qual;

                check_wifi_quality(false, level, noise, qual);
                out_message_len = message((unsigned char)MSG_OUT_SEND_WIFI_QUALITY, level, noise, qual).get(buffer_size, out_buffer);
                send(client_sockfd, out_buffer, out_message_len, 0);
            }

            try {
                while((out_message_len = message_handler->process_output_message(buffer_size, out_buffer))) {
                    //Write back sensor data for expernal program
                    send(client_sockfd, out_buffer, out_message_len, 0);
                }
            } catch (std::system_error& error) {
                ELOG("Communication error (" << error.code() << "): " << error.what());
            }

        } else{
            //ILOG("Waiting for incoming connections.\n");
            size = sizeof(client_address);

            ssize_t num_bytes = recvfrom (server_sockfd, in_buffer, buffer_size, MSG_DONTWAIT/**/, (struct sockaddr *) &client_address, &size);

            if (0 < num_bytes && message_handler->process_input_message(num_bytes, in_buffer)) {
                if(connect (client_sockfd, (struct sockaddr *) &client_address, size)) {
                    ERNOLOG("Cannot connect");
                }
                ILOG("Connected to "<< inet_ntoa(client_address.sin_addr) << ":" << ntohs (client_address.sin_port));
                memcpy(in_buffer, "Hello", strlen("Hello") + 1);
                if (write (client_sockfd, in_buffer, strlen("Hello") + 1) < 0) ERNOLOG ("Cannot write to socket");
                connected       = 1;
                last_in_message = 0;
                message_handler->connected = true;
            } else if(num_bytes == -1) {
                if(errno != EAGAIN && errno != EWOULDBLOCK) ERNOLOG("Cannot accept incoming connections");
            }
        }


        gettimeofday(&gettime_now, NULL);
        time_difference = MICROINSECOND * (gettime_now.tv_sec - start_time.tv_sec) + (gettime_now.tv_usec - start_time.tv_usec);
        time_to_sleep = loop_duration - time_difference;

        if(time_to_sleep > 0) {
            if(usleep(time_to_sleep)) {
                ERNOLOG("Cannot sleep");
            }
        } else {
            ELOG("Main procedures take too long!!!");
        }

        if(halt) {
            ILOG("Halting main loop");
            return;
        }
    }
}

