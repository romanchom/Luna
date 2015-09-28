import os
import tornado.ioloop
import tornado.web
import tornado.websocket

from tornado.options import define, options, parse_command_line

define("port", default=8888, help="run on the given port", type=int)

class IndexHandler(tornado.web.RequestHandler):
	@tornado.web.asynchronous
	def get(self):

class WebSocketHandler(tornado.websocket.WebSocketHandler):
	def check_origin(self, origin):
		return True

	def open(self):
		print "Client connected"
		self.write_message("Hello World")

	def on_message(self, message):
		#lunas = LunaS("127.0.0.1", 4243)
		self.write_message(message)
	    #lunas.send_color(message);

	def on_close(self):
		print "Client disconnected"

app = tornado.web.Application([
	(r'/', IndexHandler),
	(r'/ws', WebSocketHandler),
	(r'/static/(.*)', tornado.web.StaticFileHandler, {'path': 'static'})
])

if __name__ == '__main__':
	parse_command_line()
	app.listen(options.port)
	tornado.ioloop.IOLoop.instance().start()
