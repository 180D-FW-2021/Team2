# MQTT Latency Testing

Latency found by sending messages from a publisher python script to Unity which echos the message back and the publisher prints the round trip message time. Unity must be running mqtt script with echo back code commented in. Unity mqtt script can be independently through the mqtt unit test. The unit test can be run in the unity test runner which can be found under the window tab in the general section. The mqtt uniit test is called MqttTest1.

Tested Average latency with a single message for 10 messages where the latency is the time the publisher establishes a connection to the broker to the time it recieves the echoed message and the connection is terminated.

Tested average latency for 10 consequtive messages over a single connection where the latency was the difference in time between two messages being recieved.