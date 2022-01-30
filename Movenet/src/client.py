from enums import Position
import socket

""" References: 
- lab 3 spec code for socket commuincation
"""


class Client():
    def __init__(self, ip="127.0.0.1", port=8081):
        # self.ip = socket.gethostbyname(socket.gethostname())
        self.client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.client.connect((ip, port))

    # send data to Unity
    def send(self, player_pos):
        if player_pos == Position.JUMP_START:
            self.transmit("j")
        elif player_pos == Position.DUCK_START:
            self.transmit("d")
        # transition from duck/or out-of-frame to stationary
        elif (
            player_pos == Position.DUCK_STATIONARY
            or player_pos == Position.OUT_FRAME_STATIONARY
        ):
            self.transmit("s")
        ''' do not send out-of-frame for now
        elif player_pos == Position.OUT_OF_FRAME:
            self.transmit("o")
        '''
        
    def transmit(self, data):
        # client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # client.connect((self.ip, self.port))
        self.client.send(data.encode())
        # client.close()

    def close(self):
        self.client.close()


if __name__ == "__main__":
    client = Client()