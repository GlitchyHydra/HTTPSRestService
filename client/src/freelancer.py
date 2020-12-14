import client.src.client as c

def get_orders(client):
    path = 'orders/'
    data = {
        'token': client.token,
    }
    pass

def get_applications(client):
    path = 'applications/'
    data = {
        'token': client.token,
    }
    pass

def add_application(client, id):
    path = f'orders/{id}'
    data = {
        'token': client.token,
    }
    pass

def application_done(client, id):
    path = f'applications/{id}'
    data = {
        'token': client.token,
    }a
    pass

def print_orders(orders):
    for order in orders:
        print(
            f'\tid: {order["id"]} name: {order["name"]}\n'
            f'\t\tdescription: {order["desc"]}\n'
        )

def print_applications(applications):
    for application in applcations:
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
            if not needed_relogin(r, client):
                break

            print_orders(r)

        elif inp == 'get applications':
            r = get_applications(client)
            if not needed_relogin(r, client):
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
            if not needed_relogin(r, client):
                break
            print('Application added')

        elif inp == 'done':
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
            if not needed_relogin(r, client):
                break
            print('Application done')

        else:
            print(
                '"get orders" to get list of orders\n'
                '"get applications" to get your application list\n'
                '"add application" to add application for specific order\n'
                '"done" to mark specific application as done\n'
                '"help" to see this help'
            )