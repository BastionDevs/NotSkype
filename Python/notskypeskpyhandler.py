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
    def __init__(self, username, password, server_url):
        super(SkypeListener, self).__init__(username, password)
        self.server_url = server_url

    def onEvent(self, event):
        if isinstance(event, SkypeNewMessageEvent):
            sender_username = event.msg.userId
            sender_display_name = self.contacts[sender_username].name
            message = event.msg.content
            self.send_message_to_server(sender_username, sender_display_name, message)

    def send_message_to_server(self, username, display_name, message):
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

    def send_message_to_recipient(self, recipient, message):
        try:
            chat = self.contacts[recipient].chat
            chat.sendMsg(message)
            print("Message sent to {} successfully.".format(recipient))
        except KeyError:
            print("Recipient {} not found.".format(recipient))
        except Exception as e:
            print("An error occurred while sending message to {}: {}".format(recipient, e))

    def get_display_name(self, skype_id):
        try:
            contact = self.contacts[skype_id]
            display_name = contact.name
            self.send_display_name_to_server(skype_id, display_name)
        except KeyError:
            print("Skype ID {} not found.".format(skype_id))
        except Exception as e:
            print("An error occurred while getting display name for {}: {}".format(skype_id, e))

    def send_display_name_to_server(self, skype_id, display_name):
        try:
            response = requests.post("{}/displayname".format(self.server_url), data=display_name, headers={'Content-Type': 'text/plain'})
            if response.status_code == 200:
                print("Display name {} sent to server successfully.".format(display_name))
            else:
                print("Failed to send display name to server. Status code: {}".format(response.status_code))
        except Exception as e:
            print("An error occurred while sending display name to server: {}".format(e))

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

    @app.route('/shutdown', methods=['POST'])
    def shutdown():
        func = request.environ.get('werkzeug.server.shutdown')
        if func is None:
            raise RuntimeError('Not running with the Werkzeug Server')
        func()
        return 'Server shutting down...'

    app.run(port=port)

def main():
    if len(sys.argv) != 5:
        print("Usage: python skype_listener.py <username> <password> <server_url> <flask_port>")
        sys.exit(1)

    username = sys.argv[1]
    password = sys.argv[2]
    server_url = sys.argv[3]
    flask_port = int(sys.argv[4])

    skype_listener = SkypeListener(username, password, server_url)

    # Start Flask app in a separate thread
    flask_thread = Thread(target=start_flask_app, args=(skype_listener, flask_port))
    flask_thread.daemon = True
    flask_thread.start()

    # Start Skype event loop
    skype_listener.loop()

if __name__ == "__main__":
    main()
