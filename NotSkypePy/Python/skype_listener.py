import sys
import requests
import threading
from flask import Flask, request, jsonify
from skpy import Skype, SkypeEventLoop, SkypeNewMessageEvent, SkypeAuthException

app = Flask(__name__)

shutdown_flag = False

if len(sys.argv) != 5:
    print("Usage: python skype_listener.py <email> <password> <server_url> <port>")
    sys.exit(1)

email = sys.argv[1]
password = sys.argv[2]
server_url = sys.argv[3]
port = int(sys.argv[4])

# Initialize Skype with password authentication
token_file = "skype_token.txt"
try:
    sk = Skype(email, password, tokenFile=token_file)  # Load or create a session
    print("Authenticated successfully!")
except SkypeAuthException as e:
    print(f"Skype authentication failed: {e}")
    sys.exit(1)

intended_user = None

class CustomSkypeEventLoop(SkypeEventLoop):
    def __init__(self, skype):
        super().__init__(skype)

    def onEvent(self, event):
        if isinstance(event, SkypeNewMessageEvent):
            if event.msg.userId == intended_user:
                response = requests.post(server_url, data=event.msg.content)
                print(f"Sent message from intended user to {server_url}, response: {response.status_code}")
            else:
                payload = {
                    'user': event.msg.userId,
                    'message': event.msg.content
                }
                response = requests.post(f"{server_url}/othermsg", json=payload)
                print(f"Sent message from other user to {server_url}/othermsg, response: {response.status_code}")

@app.route('/shutdown', methods=['POST'])
def shutdown():
    global shutdown_flag
    shutdown_flag = True
    return "Server shutting down..."

@app.route('/intendeduser', methods=['POST'])
def set_intended_user():
    global intended_user
    intended_user = request.data.decode('utf-8')
    return "Intended user set to {}".format(intended_user)

@app.route('/displayname', methods=['POST'])
def get_display_name():
    username = request.data.decode('utf-8')
    try:
        user = sk.contacts[username] if username in sk.contacts else sk.contacts.user(username)
        if user is None:
            return "Error: User not found", 404
        display_name = " ".join(filter(None, [user.name.first, user.name.last]))
        return display_name
    except Exception as e:
        return "Error: {}".format(str(e)), 400

@app.route('/currentusername', methods=['POST'])
def get_current_username():
    return sk.user.id

@app.route('/currentdisplayname', methods=['POST'])
def get_current_display_name():
    user = sk.user
    display_name = " ".join(filter(None, [user.name.first, user.name.last]))
    return display_name

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

    # Token verification and loop initialization
    try:
        sk.conn.verifyToken(sk.conn.tokens.get("skype", sk.conn.tokens.get("reg")))  # Use the existing Skype or registration token
        print("Token verified successfully!")
    except SkypeAuthException as e:
        print(f"Token verification failed: {e}")
        sys.exit(1)

    # Start the Skype event loop in a separate thread
    loop = CustomSkypeEventLoop(sk)
    loop_thread = threading.Thread(target=loop.loop)
    loop_thread.start()

    # Main thread waits for shutdown
    try:
        while not shutdown_flag:
            pass
    except KeyboardInterrupt:
        pass

    # Stop the loop and exit
    loop.stop()
    sys.exit(0)
