import client.src.customer as cust
import client.src.freelancer as free
import client.src.data as d
from client.src.utilz import login


def main():
    print('Please login')
    client = d.Client()
    while True:
        while not (login := input('login: ')):
            client.login = login
        while not (password := input('password: ')):
            client.passowrd = password

        r = login(client)
        if not r.get('error'):
            client.token = r['token']
            client.role = r['role']
            break
        else:
            print(r['error']['text'])
    if client.role == d.Role.CUSTOMER:
        cust.eloop(client)
    else:
        free.eloop(client)

#main()
    