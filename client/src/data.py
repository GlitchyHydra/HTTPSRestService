from dataclasses import dataclass
from enum import Enum
import client.src.utilz as u

class OrderStatus(Enum):
    IN_WORK = 'in_work'
    CLOSE = 'close'
    OPEN = 'open'

    @property
    def statuses(self):
        statuses = {}
        for i, status in enumerate([OrderStatus.IN_WORK, OrderStatus.CLOSE]):
            statuses[i] = status
        return statuses


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


def login(client):
    path = f'login/{client.role}'
    r = u.get(path)
    if r:
        client.token = r.json()['token']
