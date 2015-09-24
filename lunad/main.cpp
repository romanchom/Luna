/*#include <sys/types.h>
#include <sys/socket.h>
#include <sys/poll.h>
#include <fcntl.h>

#include <netinet/in.h>
#include <arpa/inet.h>
#include <signal.h>

#include <stdlib.h>

#include <string.h>*/

#include <unistd.h>
#include <stdlib.h>
#include <signal.h>
#include <stdio.h>
#include <system_error>
#include <execinfo.h>

#include "luna_message_handler.h"
#include "udp_server.h"
#include "log.h"

bool halt = false;

void cleanup(void)
{
    ILOG("LunaDaemon terminated");
}

void terminate(int param)
{
    ILOG("Recived signal \"" << strsignal(param) << "\". Exiting");
    halt = true;
}

void handler(int sig) {
    void *array[10];
    size_t size;

    // get void*'s for all entries on the stack
    size = backtrace(array, 10);

    // print out all the frames to stderr
    fprintf(stderr, "Error: signal %d:\n", sig);
    backtrace_symbols_fd(array, size, STDERR_FILENO);
    exit(1);
}

int
main (int argc, char* argv[])
{
    int port;
    if(argc!=2) {
        std::cout << "USAGE: server [port]" << std::endl;
        return 1;
    } else {
        sscanf (argv[1],"%d",&port);
        if(port < 0 && port > 49152) {
            std::cerr << "Wrong port number!" << std::endl;
            return 1;
        }
    }

    signal(SIGTERM, terminate);
    signal(SIGABRT, terminate);
    signal(SIGINT,  terminate);
    signal(SIGSEGV, handler);
    atexit(cleanup);

    try {
        LunaMessageHandler mh("/dev/ttyACM0");

    printf("#%d#\n", port);

        UdpServer server(port, &mh);
        server.run(300);
    } catch (std::system_error& error) {
        ELOG("System Error (" << error.code() << "): " << error.what());
    }

    return 0;
}
