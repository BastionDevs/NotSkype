import sys
import requests
import threading
from flask import Flask, request, jsonify
from skpy import Skype, SkypeEventLoop, SkypeNewMessageEvent
import json

app = Flask(__name__)

shutdown_flag = False

# Initialize Skype without autoAck
sk = Skype(sys.argv[1], sys.argv[2])
server_url = sys.argv[3]
port = int(sys.argv[4])
intended_user = None

# Custom event loop to handle Skype events
class CustomSkypeEventLoop(SkypeEventLoop):
    def __init__(self, skype):
        super().__init__(skype)

    def onEvent(self, event):
        if isinstance(event, SkypeNewMessageEvent):
            if event.msg.userId == intended_user:
                # Send plaintext POST request to server_url
                response = requests.post(server_url, data=event.msg.content)
                print(f"Sent message from intended user to {server_url}, response: {response.status_code}")
            else:
                # Send JSON POST request to {server_url}/othermsg
                payload = {
                    'user': event.msg.userId,
                    'message': event.msg.content
                }
                response = requests.post(f"{server_url}/othermsg", json=payload)
                print(f"Sent message from other user to {server_url}/othermsg, response: {response.status_code}")

# Flask endpoint to shut down the server
@app.route('/shutdown', methods=['POST'])
def shutdown():
    global shutdown_flag
    shutdown_flag = True
    return "Server shutting down..."

# Flask endpoint to set intended user
@app.route('/intendeduser', methods=['POST'])
def set_intended_user():
    global intended_user
    intended_user = request.data.decode('utf-8')
    return "Intended user set to {}".format(intended_user)

# Flask endpoint to get display name of another user
@app.route('/displayname', methods=['POST'])
def get_display_name():
    username = request.data.decode('utf-8')
    print(f"Received username: {username}")  # Add logging
    try:
        user = sk.contacts[username] if username in sk.contacts else sk.contacts.user(username)
        print(f"Retrieved user: {user}")  # Add logging

        if user is None:
            return "Error: User not found", 404

        display_name = ""
        if user.name.first:
            display_name += user.name.first
        if user.name.last:
            display_name += " " + user.name.last

        return display_name.strip()
    except Exception as e:
        return "Error: {}".format(str(e)), 400

# Flask endpoint to get username of current user
@app.route('/currentusername', methods=['POST'])
def get_current_username():
    return sk.user.id

# Flask endpoint to get display name of current user
@app.route('/currentdisplayname', methods=['POST'])
def get_current_display_name():
    user = sk.user
    display_name = ""
    if user.name.first:
        display_name += user.name.first
    if user.name.last:
        display_name += " " + user.name.last

    return display_name.strip()

# Flask endpoint to send a message
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

def run_flask():
    app.run(port=port)

if __name__ == '__main__':
    # Start the Flask app in a separate thread
    flask_thread = threading.Thread(target=run_flask)
    flask_thread.start()

    # Start the Skype event loop
    loop = CustomSkypeEventLoop(sk)
    threading.Thread(target=loop).start()

    # Wait for shutdown
    while not shutdown_flag:
        pass

    # Stop the loop and exit
    loop.stop()
    sys.exit(0)
