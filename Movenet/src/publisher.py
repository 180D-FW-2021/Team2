import paho.mqtt.client as mqtt
from enums import Position

""" References: 
- https://github.com/eclipse/paho.mqtt.python/blob/master/examples/client_sub-class.py
- lab 3 spec code for mqtt
"""


class Publisher(mqtt.Client):

    # callback functions

    def on_connect(self, client, userdata, flags, rc):
        print("Connection returned result:", str(rc))

    def on_disconnect(self, client, userdata, rc):
        if rc != 0:
            print("Unexpected Disconnect")
        else:
            print("Expected Disconnect")

    def on_message(self, client, userdata, message):
        print(
            "Received message",
            str(message.payload),
            "on topic",
            message.topic,
            "with QoS",
            str(message.qos.decode()),
        )

    # send data to Unity
    def send(self, player_pos):
        topic = "topic/pose/"+self.username
        if player_pos == Position.JUMP_START:
            self.publish(topic, "j", qos=1)
        elif player_pos == Position.DUCK_START:
            self.publish(topic, "d", qos=1)
        elif player_pos == Position.OUT_OF_FRAME:
            self.publish(topic, "o", qos=1)
        # transition from duck/or out-of-frame to stationary
        elif (
            player_pos == Position.DUCK_STATIONARY
            or player_pos == Position.OUT_FRAME_STATIONARY
        ):
            self.publish(topic, "s", qos=1)

    # connect/disconnect functions

    def run(self, username):
        self.connect_async("test.mosquitto.org")
        self.loop_start()
        self.username = username


    def stop(self):
        self.loop_stop()
        self.disconnect()

    username = "demo"


if __name__ == "__main__":
    client = Publisher()
    client.run()
    client.publish("ece180d/test/message", "i love 180da", qos=1)
    client.stop()
