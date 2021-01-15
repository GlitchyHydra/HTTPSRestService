import client.src.data as d
import client.src.utilz as u


def add_order(client, order):
    path = 'orders/'
    data = order
    return u.post(path, data=data, header=client.token_header)


def get_orders(client, status):
    path = 'orders/'
    data = {
        'status': status
    }
    return u.get(path, data=data, headers=client.token_header)


def change_order_status(client, id, new_status):
    path = f'orders/{id}/'
    data = {
        'status': new_status
    }
    return u.put(path, data=data, headers=client.token_status)


def application_approval(client, application_id):
    path = f'applications/{application_id}/'
    return u.put(path, headers=client.token_header)


def work_done(client, work_id):
    path = f'works/{work_id}/'
    data = {
        'status': 'Close'
    }
    return u.put(path, data=data, headers=client.token_header)


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
            if u.needed_relogin(r, client):
                break

            print(f'Order {order} added successfuly')

        elif inp == 'get orders':
            status = ''
            while True:
                inp_status = input(f'status [{d.OrderStatus.on_add}]: ')
                if not inp_status:
                    status = 'open'.title()
                    break
                if inp_status.lower() in d.OrderStatus.on_add:
                    status = inp_status.title()
                    break

            r = get_orders(client, status)
            if u.needed_relogin(r, client):
                break

            print_orders(r.json())

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

            while not (inp_status := input(f'status [{d.OrderStatus.on_change}]: ')):
                if inp_status.lower() in d.OrderStatus.on_add:
                    status = inp_status.title()
                    break

            r = change_order_status(client, id, status)
            if u.needed_relogin(r, client):
                break

            print(f'Order with id={id} status changed to {status}')

        elif inp == 'approve application':
            application_id = -1

            while not (inp_appcliation_id := input('application id: ')):
                try:
                    inp_appcliation_id = int(inp_appcliation_id)
                    if inp_appcliation_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                application_id = inp_appcliation_id

            r = application_approval(client, application_id)
            if u.needed_relogin(r, client):
                break
            
            print(f'Application with id={application_id} approved')

        elif inp == 'mark work':
            work_id = -1

            while not (inp_work_id := input('work id: ')):
                try:
                    inp_work_id = int(inp_work_id)
                    if inp_work_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                work_id = inp_work_id

            r = work_done(client, work_id)
            if u.needed_relogin(r, client):
                break
            
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
