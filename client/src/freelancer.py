import client.src.data as c
import client.src.utilz as u

def get_orders(client):
    path = 'orders/'
    return u.get(path, header=client.token_header)


def get_applications(client):
    path = 'applications/'
    return u.get(path, header=client.token_header)


def add_application(client, id):
    path = f'orders/{id}/'
    return u.post(path, header=client.token_header)


def application_done(client, id):
    path = f'works/{id}/'
    data = {
        'status': 'Done'
    }
    return u.put(path, data=data, header=client.token_header)


def print_orders(orders):
    for order in orders:
        print(
            f'\tid: {order["id"]} name: {order["name"]}\n'
            f'\t\tdescription: {order["desc"]}\n'
        )


def print_applications(applications):
    for application in applications:
        print(
            f'\tid: {application["id"]} order_id: {application["order_id"]}'
        )


def eloop(client):
    while True:
        inp = ''

        while not inp:
            inp = input('> ').lower().strip()

        if inp == 'get orders':
            r = get_orders(client)
            if u.needed_relogin(r, client):
                break

            print_orders(r.json())

        elif inp == 'get applications':
            r = get_applications(client)
            if u.needed_relogin(r, client):
                break

            print_applications(r)

        elif inp == 'add application':
            order_id = -1
            while not (inp_order_id := input('order id: ')):
                try:
                    inp_order_id = int(inp_order_id)
                    if inp_order_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                order_id = inp_order_id
            r = add_application(client, order_id)
            if u.needed_relogin(r, client):
                break
            print(f'Application to order with id={order_id} was added')

        elif inp == 'mark done':
            order_id = -1
            application_id = -1
            while not (inp_order_id := input('order id: ')):
                try:
                    inp_order_id = int(inp_order_id)
                    if inp_order_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                order_id = inp_order_id

            while not (inp_appcliation_id := input('application id: ')):
                try:
                    inp_appcliation_id = int(inp_appcliation_id)
                    if inp_appcliation_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                application_id = inp_appcliation_id

            r = application_done(client, order_id, application_id)
            if u.needed_relogin(r, client):
                break
            print(f'Application with id={application_id} to order with id={order_id} marked as done')

        elif inp == 'quit':
            break

        else:
            print(
                '"get orders" to get list of orders\n'
                '"get applications" to get your application list\n'
                '"add application" to add application for specific order\n'
                '"mark done" to mark specific application as done\n'
                '"quit" to quit the application\n'
                '"help" to see this help'
            )
