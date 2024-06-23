from flask import Flask, request, jsonify
from threading import Thread
import atexit
import requests
import time
from skpy import Skype, SkypeNewMessageEvent, SkypeAuthException
import sys
import os

# Ensure minimum Python version
if sys.version_info < (3, 4, 10):
    raise RuntimeError("This script requires Python 3.4.10 or newer")

app = Flask(__name__)

if len(sys.argv) != 5:
    print("Usage: script.py <email> <password> <server_url> <port>", file=sys.stderr)
    sys.exit(1)

email = sys.argv[1]
password = sys.argv[2]
server_url = sys.argv[3]
port = int(sys.argv[4])

intended_user = None
shutdown_flag = False  # Flag to signal server shutdown

try:
    sk = Skype(email, password)
except SkypeAuthException as e:
    print("Failed to authenticate with Skype: ", e)
    sys.exit(1)

def run_flask():
    app.run(host='0.0.0.0', port=port)

def shutdown_server():
    func = request.environ.get('werkzeug.server.shutdown')
    if func is None:
        raise RuntimeError('Not running with the Werkzeug Server')
    func()

def run_skype_listener():
    global shutdown_flag
    while not shutdown_flag:
        try:
            for event in sk.getEvents():
                if isinstance(event, SkypeNewMessageEvent):
                    msg = event.msg
                    if msg.type == 'RichText' or msg.type == 'Text':
                        print("Received message from {}: {}".format(msg.userId, msg.content))
                        if intended_user and msg.userId == intended_user:
                            requests.post(server_url, data=msg.content.encode('utf-8'))
                        else:
                            requests.post(server_url + "/othermsg", data=msg.content.encode('utf-8'))
        except Exception as e:
            print("Error: ", e)
            time.sleep(10)

@app.route('/shutdown', methods=['POST'])
def shutdown():
    print("Received shutdown request")
    os._exit(0)  # Forcefully terminate the application

@app.route('/intendeduser', methods=['POST'])
def set_intended_user():
    global intended_user
    intended_user = request.data.decode('utf-8')
    return "Intended user set to: {}".format(intended_user)

@app.route('/currentuserdisplayname', methods=['POST'])
def get_current_user_display_name():
    try:
        first_name = sk.user.name.first if sk.user.name.first else ""
        last_name = sk.user.name.last if sk.user.name.last else ""
        full_name = (first_name + " " + last_name).strip()
        return full_name if full_name else "Unnamed User"
    except Exception as e:
        return "Error: {}".format(str(e)), 500

@app.route('/currentusername', methods=['POST'])
def get_current_username():
    try:
        return sk.user.id
    except Exception as e:
        return "Error: {}".format(str(e)), 500

@app.route('/displayname', methods=['POST'])
def get_display_name():
    try:
        username = request.get_data().decode('utf-8')
        user = sk.contacts.user(username)
        if user:
            first_name = user.name.first if user.name.first else ""
            last_name = user.name.last if user.name.last else ""
            full_name = (first_name + " " + last_name).strip()
            return full_name if full_name else "Unnamed User"
        else:
            return "Error: User not found", 404
    except Exception as e:
        return "Error: {}".format(str(e)), 500

@app.route('/sendmessage', methods=['POST'])
def send_message():
    try:
        data = request.get_json()
        if not data:
            return jsonify({"status": "error", "message": "No JSON data provided"}), 400

        recipient = data.get('recipient')
        message = data.get('message')

        if not recipient or not message:
            return jsonify({"status": "error", "message": "'recipient' and 'message' fields are required"}), 400

        sk.contacts[recipient].chat.sendMsg(message)

        return jsonify({"status": "success", "message": "Message sent successfully"})
    except Exception as e:
        return jsonify({"status": "error", "message": str(e)}), 500

flask_thread = Thread(target=run_flask)
flask_thread.start()

skype_thread = Thread(target=run_skype_listener)
skype_thread.start()

def on_exit():
    global shutdown_flag
    shutdown_flag = True
    try:
        requests.post('http://127.0.0.1:{}/shutdown'.format(port))
    except Exception as e:
        print("Error during shutdown: ", e)

atexit.register(on_exit)
flask_thread.join()
skype_thread.join()
