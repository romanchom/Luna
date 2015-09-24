#ifndef MESSAGEHANDLER_H
#define MESSAGEHANDLER_H

#include <stack>
#include "message.h"

class MessageHandler
{
    int  work_call;
    std::stack<message> out_messages;

    bool message_not_implemented(int, char *);
    bool message_keep_alive(int, char *);
    bool message_echo(int, char *);
    bool message_hello(int, char *);
    bool message_goodbye(int, char *);

    template<typename... Args>
    void send(Args... args);
protected:
    std::string greetings_token = "MessageHandler";

    bool (MessageHandler::*message_handlers[256])(int, char *);

public:
    bool connected;

    MessageHandler();
    unsigned char process_input_message(int, char *);
    int           process_output_message(int, char *);
    void work(void);
};

#endif // MESSAGEHANDLER_H
