import data as d
import utilz as u


def add_order(client, order):
    path = 'orders/'
    data = order
    return u.post(path, data=data, headers=client.token_header)


def get_orders(client, status):
    path = 'orders/'
    params = {
        'status': status
    }
    return u.get(path, params=params, headers=client.token_header)


def change_order_status(client, id, new_status):
    path = f'orders/{id}/'
    data = {
        'status': new_status
    }
    return u.put(path, data=data, headers=client.token_header)


def application_approval(client, application_id):
    path = f'applications/{application_id}/'
    return u.put(path, headers=client.token_header)


def work_done(client, id):
    path = f'orders/{id}/'
    data = {
        'status': 'Close'
    }
    return u.put(path, data=data, headers=client.token_header)


def print_orders(orders):
    if not orders:
        print('Orders list empty')
    for order in orders:
        print(
            f'id: {order["id"]} name: {order["name"]}\n'
            f'\tstatus: {order["status"]}\n'
            f'\tdescription: {order["desc"] or ""}\n'
            f'\tapplications: '
        )
        print_applications(order.get('applications'), '\t\t')


def print_applications(applications, a=''):
    if not applications:
        print(a + 'Applications list is empty')
    for application in applications:
        print(
            f'{a}id: {application["id"]} order_id: {application["orderId"]}\n'
            f'{a}\tstatus: {application["status"]}'
        )


def eloop(client):
    while True:
        inp = ''
        while not (inp := input('> ').lower().strip()):
            pass

        if inp == 'add order':
            order = {}
            name_inp = ''
            while True:
                name_inp = input('name: ')
                if name_inp:
                    order['name'] = name_inp
                    break
            order['desc'] = input('description: ')
            r = add_order(client, order)
            if u.needed_relogin(r, client):
                continue

            print(f'Order {order} added successfully')

        elif inp == 'get orders':
            status = ''
            while True:
                inp_status = input(f'status {d.OrderStatus.on_add()}: ')
                if not inp_status:
                    status = 'open'
                    break
                if inp_status.lower() in d.OrderStatus.on_add():
                    status = inp_status
                    break

            r = get_orders(client, status)
            if u.needed_relogin(r, client):
                continue

            print_orders(r)

        elif inp == 'change order status':
            id = 0
            status = ''
            while True:
                inp_id = input('id: ')
                try:
                    inp_id = int(inp_id)
                    if inp_id <= 0:
                        raise
                except:
                    print('Wrong formant. Only positive int required.')
                    continue

                if inp_id:
                    id = inp_id
                    break

            while True:
                inp_status = input(f'status {d.OrderStatus.on_change()}: ')
                if inp_status.lower() in d.OrderStatus.on_change():
                    status = inp_status
                    break

            r = change_order_status(client, id, status)
            if u.needed_relogin(r, client):
                continue

            print(f'Order with id={id} status changed to {status}')

        elif inp == 'approve application':
            application_id = -1

            while True:
                inp_application_id = input('application id: ')
                try:
                    inp_application_id = int(inp_application_id)
                    if inp_application_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                if inp_application_id:
                    application_id = inp_application_id
                    break

            r = application_approval(client, application_id)
            if u.needed_relogin(r, client):
                continue

            print(f'Application with id={application_id} approved')

        elif inp == 'mark work':
            work_id = -1

            while True:
                inp_work_id = input('work id: ')
                try:
                    inp_work_id = int(inp_work_id)
                    if inp_work_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                if inp_work_id:
                    work_id = inp_work_id
                    break

            r = work_done(client, work_id)
            if u.needed_relogin(r, client):
                continue

            print(f'Work with id={work_id} marked as done (closed)')

        elif inp == 'quit':
            break

        else:
            print(
                '"add order" to add order\n'
                '"get orders" to see your order list\n'
                '"change order status" to change specific order status\n'
                '"approve application" to approve specific application\n'
                '"mark work" to mark work as done (close)\n'
                '"quit" to quit the application\n'
                '"help" to see this help'
            )
