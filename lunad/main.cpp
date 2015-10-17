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

#include <getopt.h>

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
    int port          = 4243;
    int frequency     = 300;
    char serial[1024] = "/dev/ttyACM0";

    int c;
    static struct option long_options[] = {
        {"serial", 1, 0, 's'},
        {"port", 1, 0, 'p'},
        {"freq", 1, 0, 'f'},
        {"help", 1, 0, 'h'},
        {NULL, 0, NULL, 0}
    };
    int option_index = 0;
    while ((c = getopt_long(argc, argv, "spfh",
                 long_options, &option_index)) != -1) {
        switch (c) {
        case 'h':
            std::cout << "USAGE: lunad [OPTIONS]\n"
                      << "Example: sudo ./lunad -p 5000\n"
                      << "\n"
                      << "Options:\n"
                      << " -s, --serial=PATH       Path to serila port     Default: /dev/ttyACM0\n"
                      << " -p, --port=INT          UDP port to listen to   Default: 4243\n"
                      << " -f, --freq=INT          Main loop frequency     Default: 300\n"
                      << " -h, --help              Prints this message\n";
            return 0;
        case 's':
            if (optarg) {
                strncpy(serial, optarg, sizeof serial);
            } else {
                std::cerr << "Serial port must be given!" << std::endl;
                return 1;
            }
            break;
        case 'p':
            if (optarg) {
                sscanf (optarg,"%d",&port);
                if(port < 0 && port > 49152) {
                    std::cerr << "Wrong port number!" << std::endl;
                    return 1;
                }
            } else {
                std::cerr << "Port number must be given!" << std::endl;
                return 1;
            }
            break;
        case 'f':
            if (optarg) {
                sscanf (optarg,"%d",&frequency);
                if(frequency < 0 && frequency > 1000) {
                    std::cerr << "Wrong frequency!" << std::endl;
                    return 1;
                }
            } else {
                std::cerr << "Port number must be given!" << std::endl;
                return 1;
            }
            break;
        case '?':
            break;
        default:
            printf ("?? getopt returned character code 0%o ??\n", c);
        }
    }
    if (optind < argc) {
        printf ("non-option ARGV-elements: ");
        while (optind < argc)
            printf ("%s ", argv[optind++]);
        printf ("\n");
    }

    signal(SIGTERM, terminate);
    signal(SIGABRT, terminate);
    signal(SIGINT,  terminate);
    signal(SIGSEGV, handler);
    atexit(cleanup);

    try {
        LunaMessageHandler mh("/dev/ttyACM0");
        UdpServer server(port, &mh);
        server.run(frequency);
    } catch (std::system_error& error) {
        ELOG("System Error (" << error.code() << "): " << error.what());
    }

    return 0;
}
