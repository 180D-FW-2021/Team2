from enums import Position
import socket

""" References: 
- lab 3 spec code for socket commuincation
"""


class Client():
    def __init__(self):
        self.ip = socket.gethostbyname(socket.gethostname())

    # send data to Unity
    def send(self, player_pos):
        if player_pos == Position.JUMP_START:
            self.transmit("j")
        elif player_pos == Position.DUCK_START:
            self.transmit("d")
        elif player_pos == Position.OUT_OF_FRAME:
            self.transmit("o")
        # transition from duck/or out-of-frame to stationary
        elif (
            player_pos == Position.DUCK_STATIONARY
            or player_pos == Position.OUT_FRAME_STATIONARY
        ):
            self.transmit("s")
        
    def transmit(self, data):
        client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        client.connect((self.ip, 8081))
        client.send(data.encode())
        client.close()


if __name__ == "__main__":
    client = Client()
