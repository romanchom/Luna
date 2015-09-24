#include "luna_message_handler.h"

#include <fcntl.h>
#include <unistd.h>
#include <cstdint>

LunaMessageHandler::LunaMessageHandler(std::string path)
{
    greetings_token = "LunaDaemon";

    message_handlers[MSG_IN_RAW_LED]       = static_cast<bool (MessageHandler::*)(int, char *)>(&LunaMessageHandler::message_raw_led);
    message_handlers[MSG_IN_SINGLE_COLOR]  = static_cast<bool (MessageHandler::*)(int, char *)>(&LunaMessageHandler::message_single_color);

    _serial = open(path.c_str(), O_RDWR | O_NOCTTY | O_NDELAY);
    cfmakeraw(&config);

    cfsetispeed(&config, B115200);
    tcsetattr(_serial, TCSANOW, &config);
    tcflush(_serial, TCIOFLUSH);
}

LunaMessageHandler::~LunaMessageHandler()
{
    close(_serial);
}

bool LunaMessageHandler::message_raw_led(int buffer_len, char * buffer)
{
    unsigned char in_buff[1024];

    write(_serial, buffer, buffer_len);
    fsync(_serial);
    int l = read(_serial, in_buff, 1024);
    if(l > 0)
        printf("%.*s\n", l, in_buff);
    return true;
}

bool LunaMessageHandler::message_single_color(int buffer_len, char * buffer)
{
    ILOG("Single Color: " << buffer);
    return true;
}
