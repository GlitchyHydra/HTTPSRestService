import customer as cust
import freelancer as free
import data as d
from utilz import login


def main():
    print('Please login')
    client = d.Client()
    while True:
        while True:
            login_inp = input('login: ')
            if login_inp:
                client.login = login_inp
                break

        while True:
            password_inp = input('password: ')
            if password_inp:
                client.password = password_inp
                break

        r = login(client)
        if not r.get('error'):
            client.token = r['token']
            client.role = r['role']
            print(f'Login successfully like {client.role}')
            break
        else:
            print(r['error'])

    if client.role == d.Role.CUSTOMER:
        cust.eloop(client)
    else:
        free.eloop(client)

    print('Goodbye')


main()
