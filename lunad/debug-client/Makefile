CC=gcc
CC_FLAGS = -Wall
LD_FLAGS =

EXEC=debug-client
SOURCES=$(wildcard *.c)
OBJECTS=$(SOURCES:.c=.o)

$(EXEC):$(OBJECTS)
	$(CC) $(OBJECTS) $(LD_FLAGS) -o $(EXEC)

%.o:%.c
	$(CC) -c $(CC_FLAGS) $< -o $@

clean:
	rm -f $(EXEC) $(OBJECTS)
