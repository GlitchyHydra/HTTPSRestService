from dataclasses import dataclass
from enum import Enum
import client.src.utilz as u

class OrderStatus(Enum):
    PROCESSING = 'processing'
    CLOSE = 'close'
    OPEN = 'open'

    @property
    def on_add(self):
        return [OrderStatus.OPEN, OrderStatus.CLOSE, OrderStatus.PROCESSING]

    @property
    def on_change(self):
        return [OrderStatus.CLOSE, OrderStatus.PROCESSING]

class Role:
    FREELANCER = 'freelances'
    CUSTOMER = 'customer'

    @staticmethod
    def roles():
        return (Role.FREELANCER, Role.CUSTOMER)


@dataclass
class Client:
    token: str = ''
    role: str = ''
    login: str = ''
    password: str = ''

    @property
    def logined(self):
        return bool(self.token)

    @property
    def login_Data(self):
        return {
            'login': self.login,
            'password': self.password,
        }

    @property
    def token_header(self):
        return {
            'Authorization': f'Bearer {token}'
        }
