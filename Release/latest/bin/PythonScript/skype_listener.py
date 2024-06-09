import sys
import requests
from flask import Flask, request, jsonify
from threading import Thread
from skpy import Skype, SkypeEventLoop, SkypeNewMessageEvent

try:
    import simplejson as json
except ImportError:
    import json

app = Flask(__name__)

class SkypeListener(SkypeEventLoop):
    def __init__(self, username, password, server_url, intended_user):
        super(SkypeListener, self).__init__(username, password)
        self.server_url = server_url
        self.intended_user = intended_user

    def onEvent(self, event):
        if isinstance(event, SkypeNewMessageEvent):
            sender_username = event.msg.userId
            sender_display_name = self.contacts[sender_username].name
            message = event.msg.content
            if sender_username != self.intended_user:
                self.send_message_to_other_server(sender_username, sender_display_name, message)
            else:
                self.send_message_to_recipient(message)

    def send_message_to_other_server(self, username, display_name, message):
        payload = {
            'username': username,
            'display_name': display_name,
            'message': message
        }
        try:
            response = requests.post("{}/othermsg".format(self.server_url), json=payload, headers={'Content-Type': 'application/json'})
            if response.status_code == 200:
                print("Message sent to server successfully.")
            else:
                print("Failed to send message to server. Status code: {}".format(response.status_code))
        except Exception as e:
            print("An error occurred while sending message to server: {}".format(e))

    def send_message_to_recipient(self, message):
        # Code to send message to recipient

    # Other methods like get_display_name, send_display_name_to_server, etc. remain unchanged

def start_flask_app(skype_listener, port):
    @app.route('/send_message', methods=['POST'])
    def send_message():
        data = request.json
        recipient = data.get('recipient')
        message = data.get('message')
        if not recipient or not message:
            return jsonify({'error': 'Recipient and message are required'}), 400
        try:
            skype_listener.send_message_to_recipient(recipient, message)
            return jsonify({'status': 'Message sent successfully'}), 200
        except Exception as e:
            return jsonify({'error': str(e)}), 500

    @app.route('/getdisplayname', methods=['POST'])
    def get_display_name():
        skype_id = request.data.decode('utf-8')
        if not skype_id:
            return jsonify({'error': 'Skype ID is required'}), 400
        try:
            skype_listener.get_display_name(skype_id)
            return jsonify({'status': 'Display name retrieval in process'}), 200
        except Exception as e:
            return jsonify({'error': str(e)}), 500

    @app.route('/get_current_user_displayname', methods=['GET'])
    def get_current_user_displayname():
        try:
            current_user = skype_listener.user
            display_name = current_user.name
            return display_name, 200
        except Exception as e:
            return jsonify({'error': str(e)}), 500

    @app.route('/shutdown', methods=['POST'])
    def shutdown():
        func = request.environ.get('werkzeug.server.shutdown')
        if func is None:
            raise RuntimeError('Not running with the Werkzeug Server')
        func()
        return 'Server shutting down...'

    app.run(port=port)

def main():
    if len(sys.argv) != 6:
        print("Usage: python skype_listener.py <username> <password> <server_url> <flask_port> <intended_user>")
        sys.exit(1)

    username = sys.argv[1]
    password = sys.argv[2]
    server_url = sys.argv[3]
    flask_port = int(sys.argv[4])
    intended_user = sys.argv[5]

    skype_listener = SkypeListener(username, password, server_url, intended_user)

    # Start Flask app in a separate thread
    flask_thread = Thread(target=start_flask_app, args=(skype_listener, flask_port))
    flask_thread.daemon = True
    flask_thread.start()

    # Start Skype event loop
    skype_listener.loop()

if __name__ == "__main__":
    main()
