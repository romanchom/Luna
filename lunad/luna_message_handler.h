#ifndef LUNAMESSAGEHANDLER_H
#define LUNAMESSAGEHANDLER_H

#include <stdio.h>
#include <termios.h>

#include "message_handler.h"

#define MSG_IN_RAW_LED 101
#define MSG_IN_SINGLE_COLOR 102

class LunaMessageHandler : public MessageHandler
{
private:
    int _serial;
    termios config;
public:
    LunaMessageHandler(std::string);
    ~LunaMessageHandler();
    bool message_raw_led(int, char *);
    bool message_single_color(int, char *);
};

#endif // LUNAMESSAGEHANDLER_H

