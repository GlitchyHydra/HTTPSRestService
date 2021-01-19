import data as d
import utilz as u


def get_orders(client):
    path = 'orders/'
    params = {
        'status': 'open'
    }
    return u.get(path, params=params, headers=client.token_header)


def get_works(client):
    path = 'works/'

    return u.get(path, headers=client.token_header)


def get_applications(client):
    path = 'applications/'
    return u.get(path, headers=client.token_header)


def add_application(client, id):
    path = f'orders/{id}/'
    return u.post(path, headers=client.token_header)


def application_done(client, id):
    path = f'works/{id}/'
    data = {
        'status': 'Done'
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


def print_works(works):
    if not works:
        print('Orders list empty')
    for work in works:
        print(
            f'id: {work["id"]} order_id: {work["orderId"]}\n'
            f'\tstatus: {work["status"]}'
        )


def eloop(client):
    while True:
        inp = ''

        while not (inp := input('> ').lower().strip()):
            pass

        if inp == 'get orders':
            r = get_orders(client)
            if u.needed_relogin(r, client):
                continue

            print_orders(r)

        elif inp == 'get works':
            r = get_works(client)
            if u.needed_relogin(r, client):
                continue

            print_works(r)

        elif inp == 'get applications':
            r = get_applications(client)
            if u.needed_relogin(r, client):
                continue

            print_applications(r)

        elif inp == 'add application':
            order_id = -1
            while True:
                inp_order_id = input('order id: ')
                try:
                    inp_order_id = int(inp_order_id)
                    if inp_order_id <= 0:
                        raise
                except:
                    print('Positive int required. Try again')
                    continue
                if inp_order_id:
                    order_id = inp_order_id
                    break

            r = add_application(client, order_id)
            if u.needed_relogin(r, client):
                continue

            print(f'Application to order with id={order_id} was added')

        elif inp == 'mark done':
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

            r = application_done(client, work_id)
            if u.needed_relogin(r, client):
                continue

            print(f'Work with id={work_id} marked as done')

        elif inp == 'quit':
            break

        else:
            print(
                '"get orders" to get list of orders\n'
                '"get works" to get list of works\n'
                '"get applications" to get your application list\n'
                '"add application" to add application for specific order\n'
                '"mark done" to mark specific application as done\n'
                '"quit" to quit the application\n'
                '"help" to see this help'
            )
