from lunascript import LunaS
from flask import Flask, jsonify, render_template, request
app = Flask(__name__)

@app.route("/")
def index():
	return render_template('index.html')

@app.route('/_manual_color')
def manual_color():
	color = request.args.get('color')
	lunas = LunaS("127.0.0.1", 4243)
	lunas.send_color(color);
	return jsonify(result=color)

if __name__ == "__main__":
	app.run()
