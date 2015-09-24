#include "message_handler.h"

#include <cmath>
#include <memory.h>
#include <stdio.h>
#include <system_error>

#include "log.h"
#include "messages.h"

MessageHandler::MessageHandler() :
  work_call(0),
  out_messages(),
  connected(false)
{
    for(int i = 0; i < 256; i++)
        message_handlers[i] = &MessageHandler::message_not_implemented;

    message_handlers[MSG_IN_KEEP_ALIVE]  = &MessageHandler::message_keep_alive;
    message_handlers[MSG_IN_ECHO]        = &MessageHandler::message_echo;
    message_handlers[MSG_IN_HELLO]       = &MessageHandler::message_hello;
    message_handlers[MSG_IN_GOODBYE]     = &MessageHandler::message_goodbye;
}

int MessageHandler::process_output_message(int buffer_len, char * buffer)
{
    if(out_messages.empty()) {
        return 0;
    } else {
        int message_len = out_messages.top().get(buffer_len, buffer);
        out_messages.pop();
        return message_len;
    }
}

template<typename... Args>
void MessageHandler::send(Args... args)
{
    out_messages.push(message(args...));
}

unsigned char MessageHandler::process_input_message(int buffer_len, char * buffer)
{
    if(buffer_len >= 1) {
        unsigned char message_id = buffer[0];
#ifdef DEBUG
        ILOG("Message received "<< (int)message_id);
#endif
        if(connected || (message_id == MSG_IN_HELLO)) {
            if ((this->*message_handlers[message_id])(buffer_len - 1, buffer + 1)) {
                return message_id;
            } else {
                return 0;
            }
        } else {
            return 0;
        }
    } else {
        ELOG("Message lenght less than 1");
        throw std::system_error(EINVAL, std::system_category());
    }
}

void MessageHandler::work()
{
    if(connected){
        try {
            //do something
        } catch (std::system_error& error) {
            ELOG("Communication error (" << error.code() << "): " << error.what());
        }
    }
}

bool MessageHandler::message_not_implemented(int buffer_len, char * buffer)
{
    ELOG("Message not implemented");
    return false;
}

bool MessageHandler::message_keep_alive(int buffer_len, char * buffer) {return true;}

bool MessageHandler::message_echo(int buffer_len, char * buffer)
{
    ILOG("Echo message received: " << buffer);
    this->process_output_message(buffer_len, buffer);
    return true;
}

bool MessageHandler::message_hello(int buffer_len, char * buffer)
{
    if(greetings_token == std::string(buffer)) {
        ILOG("Succesfully connected");
        this->process_output_message(buffer_len, buffer);
        return true;
    } else {
        ELOG("Connection declined. Token given: (" << buffer << ")");
        return false;
    }
}

bool MessageHandler::message_goodbye(int buffer_len, char * buffer)
{
    ILOG("Succesfully disconnected");
    return true;
}
