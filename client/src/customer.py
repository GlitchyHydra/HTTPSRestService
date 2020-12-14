import client.src.data as d
from client.src.utilz import needed_relogin


def add_order(client, order):
    path = 'orders/'
    data = order
    data['token'] = client.token
    pass


def get_orders(client):
    path = 'orders/'
    data = {
        'token': client.token,
    }
    pass


def change_order_status(client, id, new_status):
    path = f'orders/{id}'
    data = {
        'status': new_status,
        'token': client.token,
    }
    pass


def application_approval(client, order_id, application_id, done=False):
    path = f'orders/{order_id}/applications/{application_id}'
    data = {
        'token': client.token,
    }
    pass


def print_orders(orders):
    for order in orders:
        print(
            f'id: {order["id"]} name: {order["name"]}\n'
            f'\tdescription: {order["desc"]}\n'
            f'\tapplication ids: {order["appls"]}'
        )


def eloop(client):
    while True:
        inp = ''
        while not inp:
            inp = input('> ').lower().strip()

        if inp == 'add order':
            order = {}
            while not (name := input('name: ')):
                order['name'] = name
            order['desc'] = input('description: ')
            r = add_order(client, order)
            if not needed_relogin(r, client):
                break

        elif inp == 'get orders':
            r = get_orders(client)
            if not needed_relogin(r, client):
                break

            print_orders(r)

        elif inp == 'change order status':
            id = 0
            status = ''
            while not (inp_id := input('id: ')):
                try:
                    inp_id = int(inp_id)
                    if inp_id <= 0:
                        raise
                except:
                    print('Wrong formant. Only positive int required.')
                    continue

                id = inp_id
            accepted_statuses = d.OrderStatus.statuses
            accepted_statuses_text = ''
            for i, status in accepted_statuses.items():
                accepted_statuses_text += f'({i}) {status}, '
            accepted_statuses_text = accepted_statuses_text[:-2]

            print('Choose status (int):')
            while not (inp_status := input(f'status [{accepted_statuses_text}]: ')):
                try:
                    inp_status = int(inp_status)
                except:
                    print('Int required. Try again')
                    continue

                if not accepted_statuses.get(inp_status):
                    print(
                        f'Status with {inp_status} does not exist. Try again')
                    continue

                status = accepted_statuses[inp_status]
            r = change_order_status(client, id, status)
            if not needed_relogin(r, client):
                break

        elif inp == 'approve application':
            order_id = -1
            application_id = -1
            done = False
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

            while not (inp_done := input('apply or done: ').lower().strip()):
                if inp_done == 'apply':
                    done = False
                elif inp_done == 'done':
                    done = True
                else:
                    print('Put apply or done')
                    continue

            r = application_approval(clinet, order_id, application_id, done)
            if not needed_relogin(r, client):
                break

        else:
            print(
                '"add order" to add order\n'
                '"get orders" to see your order list\n'
                '"change order status" to change specific order status\n'
                '"approve application" to approce specific application (apply or mark as done)\n'
                '"help" to see this help'
            )
