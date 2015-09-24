#include "message.h"

int message::get(int buffer_len, char * out_buff) {
    memset(out_buff, 0, buffer_len);

    if(ptr > buffer_len) {
        ELOG("Send buffer too small");
        throw std::system_error(ENOBUFS, std::system_category());
    }
    memcpy(out_buff, buf, ptr);
    return ptr;
}
