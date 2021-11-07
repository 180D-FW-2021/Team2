import paho.mqtt.client as mqtt

''' References: 
https://github.com/eclipse/paho.mqtt.python/blob/master/examples/client_sub-class.py
lab 3 spec code for mqtt
'''

class Publisher(mqtt.Client):

    def on_connect(self, client, userdata, flags, rc):
        print("Connection returned result:", str(rc))

    def on_disconnect(self, client, userdata, rc):
        if rc != 0:
            print("Unexpected Disconnect")
        else:
            print("Expected Disconnect")

    def on_message(self, client, userdata, message):
        print("Received message", str(message.payload), "on topic", message.topic, "with QoS", str(message.qos.decode()))

    def run(self):
        self.connect_async("mqtt.eclipseprojects.io")
        self.loop_start()

    def stop(self):
        self.loop_stop()
        self.disconnect()

if __name__ == '__main__':
    client = Publisher()
    client.run()
    client.publish('ece180d/test/message', "i love 180da", qos=1)
    client.stop()