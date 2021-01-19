from dataclasses import dataclass
from enum import Enum
import utilz as u


class OrderStatus(Enum):
    PROCESSING = 'processing'
    CLOSE = 'close'
    OPEN = 'open'

    @staticmethod
    def on_add():
        return list(map(lambda x: x.value, [OrderStatus.OPEN, OrderStatus.CLOSE, OrderStatus.PROCESSING]))

    @staticmethod
    def on_change():
        return list(map(lambda x: x.value, [OrderStatus.CLOSE, OrderStatus.PROCESSING]))


class Role:
    FREELANCER = 'freelancer'
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
    def login_data(self):
        return {
            'login': self.login,
            'password': self.password,
        }

    @property
    def token_header(self):
        return {
            'Authorization': f'{self.token}'
        }
