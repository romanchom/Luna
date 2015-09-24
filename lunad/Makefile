CC=g++-4.8
#g++
CC_FLAGS = -Wall -std=c++11 -o2

# -mfloat-abi=hard -o3
LD_FLAGS = -lm -lrt
# -mfloat-abi=hard
REMOTE_PI=root@192.168.1.105
PI_FOLDER=~/romek/quadro/

EXEC=lunad
SOURCES=$(wildcard *.cpp)
OBJECTS=$(SOURCES:.cpp=.o)

$(EXEC):$(OBJECTS)
	$(CC) $(OBJECTS) $(LD_FLAGS) -o $(EXEC)

%.o:%.cpp
	$(CC) -c $(CC_FLAGS) $< -o $@

clean:
	rm -f $(EXEC) $(OBJECTS)

#crosscompile:
#	make clean
#	make CC=/rpi/gcc-x64/bin/arm-linux-gnueabihf-$(CC) \
#		CC_FLAGS='$(CC_FLAGS) -I /rpi/usr/include -I /rpi/usr/include/arm-linux-gnueabihf' \
#		LD_FLAGS='$(LD_FLAGS) -L /rpi/usr/lib -L /rpi/lib' \
#		 -j 4
#	scp -r ./$(EXEC) $(REMOTE_PI):$(PI_FOLDER)exec/
