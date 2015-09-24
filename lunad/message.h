#ifndef MESSAGE_H
#define MESSAGE_H

#include <memory.h>
#include <errno.h>
#include <system_error>

#include "log.h"

#define MESSAGE_BUF_SIZE 1024

class message
{
private:
    char buf[MESSAGE_BUF_SIZE];
    int ptr;


    template<typename T>
    void build_message(T obj) {
        int size = sizeof(T);
        if(ptr + size > MESSAGE_BUF_SIZE) {
            ELOG("Message buffer too small");
            throw std::system_error(ENOBUFS, std::system_category());
        }
        memcpy(buf+ptr, &obj, size);
        ptr += size;
    }

    template<typename First, typename... Rest>
    void build_message(First first, Rest... rest) {
        build_message(first);
        build_message(rest...);
    }

public:

    template<typename... Args>
    message(Args... args): buf{0}, ptr(0)
    {
        build_message(args...);
    }

    int get(int buffer_len, char * out_buff);

    template<typename First, typename... Rest>
    static void retrive(int buffer_len, char * in_buffer,First& first, Rest&... rest)
    {
        if(buffer_len < 0) {
            ELOG("Buffer is not long enough for given parameters");
            return;
        }
        retrive(buffer_len, in_buffer, first);
        retrive(buffer_len - sizeof(First), in_buffer + sizeof(First), rest...);
    }

    template<typename T>
    static void retrive(int buffer_len, char * in_buffer, T &obj)
    {
        if(buffer_len < 0) {
            ELOG("Buffer is not long enough for given parameters");
            return;
        }
        obj = *(T*)in_buffer;
    }
};

#endif // MESSAGE_H
