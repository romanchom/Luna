#ifndef QLOG_H
#define QLOG_H
#include <ctime>
#include <iostream>
#include <errno.h>
#include <string.h>

std::string now( const char* format = "%c" );

#define ILOG(str) \
{\
std::cout << "I " << now() << ": " << str << std::endl;\
}

#define ELOG(str) \
{\
std::cerr << "E " << now() << ": " << str << std::endl;\
}

#define ERNOLOG(str) \
{\
std::cerr << "E " << now() << ": " << str << " (" << strerror(errno) << ")" << std::endl;\
}


#endif
