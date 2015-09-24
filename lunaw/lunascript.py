import socket
import sys

class LunaS:
	def __init__(self, ip, port):
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
		#server_address = (ip, port)
		#self.sock.bind(server_address)
		self.sock.sendto("\x01LunaDaemon", (ip, port))
		data, server = self.sock.recvfrom(4096)
		self.sock.connect(server)
	def __del__(self):
		self.sock.send("\x63" + "Goodbye")

	def send_color(self, color):
		self.sock.send("\x65" + color)
